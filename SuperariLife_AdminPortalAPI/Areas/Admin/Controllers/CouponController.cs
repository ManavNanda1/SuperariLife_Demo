using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.Coupon;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.CouponCode;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;

namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/coupon")]
    [ApiController]
    [Authorize]
    public class CouponController : ControllerBase
    {
        #region Fields
        private readonly ICouponService _couponService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly IConfiguration _config;
        #endregion

        #region constructor
        public CouponController(ICouponService couponService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService,
            IConfiguration config
         )
        {
            _couponService = couponService;
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
        public async Task<BaseApiResponse> InsertUpdateCouponCode([FromBody] CouponCodeReqModel model)
        {
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.UserId = tokenModel.Id;
            BaseApiResponse response = new BaseApiResponse();
            var result = await _couponService.InsertUpdateCouponCode(model);
            if (result > StatusResult.Updated)
            {
                response.Message = ErrorMessages.SaveCouponSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.Updated)
            {
                response.Message = ErrorMessages.UpdateCouponSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.AlreadyExists)
            {
                response.Message = ErrorMessages.CouponExist;
                response.Success = false;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }

            return response;
        }

        /// <summary>
        /// Get Coupon List 
        /// </summary>
        /// <param model="CommonPaginationModel" ></param>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<ApiResponse<CouponCodeResponseModel>> GetCouponListByAdmin(CommonPaginationModel info)
        {
            ApiResponse<CouponCodeResponseModel> response = new ApiResponse<CouponCodeResponseModel>() { Data = new List<CouponCodeResponseModel>() };
            var result = await _couponService.GetCouponListByAdmin(info);
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
        /// Get CouponCode Details By ID 
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        [HttpGet("getcoupon/{Id}")]
        public async Task<ApiPostResponse<CouponCodeResponseModel>> GetCouponCodeById(long Id)
        {
            ApiPostResponse<CouponCodeResponseModel> response = new ApiPostResponse<CouponCodeResponseModel>() { Data = new CouponCodeResponseModel() };

            var result = await _couponService.GetCouponCodeById(Id);
            if (result != null)
            {
                response.Data = result;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Get CouponCode Details By ID For Customer List  
        /// </summary>
        /// <param model="CouponCodeReqDetailModel"></param>
        /// <returns></returns>
        [HttpPost("get-couponcode-detail")]
        public async Task<ApiResponse<CouponCodeResponseModel>> GetCouponCodeDetailById(CouponCodeReqDetailModel couponCodeDetailInfo)
        {
            ApiResponse<CouponCodeResponseModel> response = new ApiResponse<CouponCodeResponseModel>() { Data = new List<CouponCodeResponseModel>() };
             var Path = Constants.https + HttpContext.Request.Host.Value;
            var result = await _couponService.GetCouponCodeDetailById(couponCodeDetailInfo);

            if (result != null)
            {
                for (var i = 0; i < result.Count; i++)
                {
                    result[i].CustomerImage = Path + _config["Path:CustomerProfileImagePath"] + result[i].CustomerEmail + '/' + result[i].CustomerImage;
                }
                response.Data = result;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Delete Coupon 
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        [HttpPost("delete/{Id}")]
        public async Task<BaseApiResponse> DeleteCouponCode(long Id)
        {
            BaseApiResponse response = new BaseApiResponse();
            var result = await _couponService.DeleteCouponCode(Id);
            if (result == Status.Success)
            {
                response.Message = ErrorMessages.DeleteCouponSuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }
            return response;
        }

     
    }
}
