using Microsoft.AspNetCore.Mvc;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.TermsAndConditionPage;

namespace SuperariLifeAPI.Areas.CustomerPortal.Controllers
{
    [Route("api/customer/terms-condition")]
    [ApiController]
    public class TermsAndConditionPageController : ControllerBase
    {
        #region Fields
        private readonly ITermsAndConditionPageService _termsAndConditionPageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        #endregion

        #region constructor
        public TermsAndConditionPageController(ITermsAndConditionPageService termsAndConditionPageService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService
         )
        {
            _termsAndConditionPageService = termsAndConditionPageService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
        }
        #endregion

        /// <summary>
        /// Get Terms and Condition Page 
        /// </summary>
        /// <param  ></param>
        /// <returns></returns>

        [HttpGet("page")]
        public async Task<ApiResponse<TermsAndConditionPageResponseModel>> GetTermsAndConditionPage()
        {
            ApiResponse<TermsAndConditionPageResponseModel> response = new ApiResponse<TermsAndConditionPageResponseModel>() { Data = new List<TermsAndConditionPageResponseModel>() };
            var result = await _termsAndConditionPageService.GetTermsAndConditionPage();
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
