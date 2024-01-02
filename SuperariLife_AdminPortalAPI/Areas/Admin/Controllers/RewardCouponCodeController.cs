using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.CouponCode;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Coupon.RewardCouponCode;
using SuperariLife.Service.JWTAuthentication;
using static SuperariLife.Common.EmailNotification.EmailNotification;

namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/reward-coupon-code")]
    [ApiController]
    [Authorize]
    public class RewardCouponCodeController : ControllerBase
    {
        #region Fields
        private readonly IRewardCouponCodeService _rewardCouponService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly IConfiguration _config;
        private readonly SMTPSettings _smtpSettings;
        private readonly AppSettings _appSettings;
        #endregion

        #region constructor
        public RewardCouponCodeController(IRewardCouponCodeService rewardCouponService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService,
            IOptions<SMTPSettings> smtpSettings,
            IOptions<AppSettings> appSettings,
            IConfiguration config
         )
        {
            _rewardCouponService = rewardCouponService;
            _smtpSettings = smtpSettings.Value;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
            _config = config;
        }
        #endregion

        /// <summary>
        /// Add Update Coupon
        /// </summary>
        /// <param name="CouponReqModel"></param>
        /// <returns></returns>

        [HttpPost("save")]
        public async Task<BaseApiResponse> InsertUpdateRewardCouponCode([FromBody] RewardCouponCodeReqModel model)
        {
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.UserId = tokenModel.Id;
            BaseApiResponse response = new BaseApiResponse();
            var result = await _rewardCouponService.InsertUpdateRewardCouponCode(model);
            if (result > StatusResult.Updated)
            {
                EmailSetting setting = new EmailSetting
                {
                    EmailEnableSsl = Convert.ToBoolean(_smtpSettings.EmailEnableSsl),
                    EmailHostName = _smtpSettings.EmailHostName,
                    EmailPassword = _smtpSettings.EmailPassword,
                    EmailAppPassword = _smtpSettings.EmailAppPassword,
                    EmailPort = Convert.ToInt32(_smtpSettings.EmailPort),
                    FromEmail = _smtpSettings.FromEmail,
                    FromName = _smtpSettings.FromName,
                    EmailUsername = _smtpSettings.EmailUsername,
                };

                string emailBody = string.Empty;
                string BasePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.EmailTemplate);

                using (StreamReader reader = new StreamReader(Path.Combine(BasePath, Constants.RewardCouponCode)))
                {
                    emailBody = reader.ReadToEnd();
                }
                var path = HttpContext.Request.Host.Value;
                emailBody = emailBody.Replace("##appreciation##", " Thank you for being valuable customer to us.. ");
                emailBody = emailBody.Replace("##couponCodeName##", (model.CouponCode).ToString());
                emailBody = emailBody.Replace("##LogoURL##", Constants.https + path + _appSettings.EmailLogo);
                emailBody = emailBody.Replace("##envelopicon##", Constants.https + path + _appSettings.EnvelopIcon);
                emailBody = emailBody.Replace("##facebookicon##", Constants.https + path + _appSettings.FacebookIcon);
                emailBody = emailBody.Replace("##instagramicon##", Constants.https + path + _appSettings.InstagramIcon);
                emailBody = emailBody.Replace("##linkedinicon##", Constants.https + path + _appSettings.LinkedIn);
                emailBody = emailBody.Replace("##recruitmentbannerimg##", Constants.https + path + _appSettings.RecurimentBanner);
                emailBody = emailBody.Replace("##ExpireDateOfCoupon##", model.ExpireDateOfCoupon.ToString());
                await Task.Run(() => SendMailMessage(model.CustomerEmail, null, null, "Reward Of Coupon Code", emailBody, setting, null));
                response.Message = ErrorMessages.RewardSaveCouponSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.Updated)
            {
                response.Message = ErrorMessages.RewardUpdateCouponSuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }
            return response;
        }

        /// <summary>
        /// Get reward coupon code list by admin 
        /// </summary>
        /// <param model="CommonPaginationModel" ></param>
        /// <returns></returns>
        [HttpPost("reward-list")]
        public async Task<ApiResponse<RewardCouponCodeResponseModel>> GetRewardCouponCodeListByAdmin(CommonPaginationModel info)
        {
            ApiResponse<RewardCouponCodeResponseModel> response = new ApiResponse<RewardCouponCodeResponseModel>() { Data = new List<RewardCouponCodeResponseModel>() };
            var result = await _rewardCouponService.GetRewardCouponCodeListByAdmin(info);
            if (result.Count != 0)
            {
                response.Data = result;
            }
            else
            {
                response.Message = ErrorMessages.NoSuchRecordFound;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Get customer list by admin 
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        [HttpGet("customer-list/{searchStr?}")]
        public async Task<ApiResponse<CustomerListForSendingCouponCodeRewardResponseModel>> GetCustomerListForSendingCouponCodeReward( string? searchStr)
        {
            searchStr = searchStr == null ? "" : searchStr;
            var Path = Constants.https + HttpContext.Request.Host.Value;
            ApiResponse<CustomerListForSendingCouponCodeRewardResponseModel> response = new ApiResponse<CustomerListForSendingCouponCodeRewardResponseModel>() { Data = new List<CustomerListForSendingCouponCodeRewardResponseModel>() };
            var result = await _rewardCouponService.GetCustomerListForSendingCouponCodeReward(searchStr);
            if (result.Count != 0)
            {
                for (var i = 0; i < result.Count; i++)
                {
                    result[i].CustomerImage = Path + _config["Path:CustomerProfileImagePath"] + result[i].CustomerEmail + '/' + result[i].CustomerImage;
                }
                response.Data = result;
            }
            else
            {
                response.Message = ErrorMessages.NoSuchRecordFound;
            }
            response.Success = true;
            return response;
        }
    }
}
