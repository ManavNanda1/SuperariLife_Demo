using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.PrivacyPolicyPage;

namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/privacy-policy")]
    [ApiController]
    [Authorize]
    public class PrivacyPolicyPageController : ControllerBase
    {
        #region Fields
        private readonly IPrivacyPolicyPageService _privacyPolicPageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        #endregion

        #region constructor
        public PrivacyPolicyPageController(IPrivacyPolicyPageService privacyPolicPageService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService
         )
        {
            _privacyPolicPageService = privacyPolicPageService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
        }
        #endregion

        /// <summary>
        /// Update Privacy Page
        /// </summary>
        /// <param name="PrivacyPageReqModel"></param>
        /// <returns></returns>
        [HttpPost("save")]

        public async Task<BaseApiResponse> InsertUpdatePrivacyPage([FromBody] PrivacyPageReqModel model)
        {
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.UserId = tokenModel.Id;
            BaseApiResponse response = new BaseApiResponse();
            var result = await _privacyPolicPageService.InsertUpdatePrivacyPage(model);
            if (result > StatusResult.Updated)
            {
                response.Message = ErrorMessages.AddPrivacyPolicyPageSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.Updated)
            {
                response.Message = ErrorMessages.UpdatePrivacyPolicyPageSuccess;
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
        /// Get Privacy Page 
        /// </summary>
        /// <param  ></param>
        /// <returns></returns>

        [HttpGet("page")]
        public async Task<ApiResponse<PrivacyPageResponseModel>> GetPrivacyPage( )
        {
            ApiResponse<PrivacyPageResponseModel> response = new ApiResponse<PrivacyPageResponseModel>() { Data = new List<PrivacyPageResponseModel>() };
            var result = await _privacyPolicPageService.GetPrivacyPage();
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

    }
}
