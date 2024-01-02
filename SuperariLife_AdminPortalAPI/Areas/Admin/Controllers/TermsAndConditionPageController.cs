using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.TermsAndConditionPage;

namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/terms-condition")]
    [ApiController]
    [Authorize]
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
        /// Update Terms And Condition Page
        /// </summary>
        /// <param name="TermsAndConditionPageReqModel"></param>
        /// <returns></returns>
        [HttpPost("save")]

        public async Task<BaseApiResponse> InsertUpdateTermsAndConditionPage([FromBody] TermsAndConditionPageReqModel model)
        {
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.UserId = tokenModel.Id;
            BaseApiResponse response = new BaseApiResponse();
            var result = await _termsAndConditionPageService.InsertUpdateTermsAndConditionPage(model);
            if (result > StatusResult.Updated)
            {
                response.Message = ErrorMessages.AddTermsAndConditionPageSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.Updated)
            {
                response.Message = ErrorMessages.UpdateTermsAndConditionPageSuccess;
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
