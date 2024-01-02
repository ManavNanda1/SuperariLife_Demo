using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.PrivacyPolicyPage;

namespace SuperariLifeAPI.Areas.CustomerPortal.Controllers
{
    [Route("api/customer/privacy-policy")]
    [ApiController]

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
        /// Get Privacy Page 
        /// </summary>
        /// <param  ></param>
        /// <returns></returns>

        [HttpGet("page")]
        public async Task<ApiResponse<PrivacyPageResponseModel>> GetPrivacyPage()
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
