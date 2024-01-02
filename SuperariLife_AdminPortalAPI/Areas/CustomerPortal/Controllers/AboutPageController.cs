using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.AboutPage;

namespace SuperariLifeAPI.Areas.CustomerPortal.Controllers
{
    [Route("api/customer/about-us")]
    [ApiController]

    public class AboutPageController : ControllerBase
    {
        #region Fields
        private readonly IAboutPageService _aboutPageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        #endregion

        #region Constructor
        public AboutPageController(IAboutPageService aboutPageService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
           IConfiguration config
         )
        {
            _aboutPageService = aboutPageService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
            _hostingEnvironment = hostingEnvironment;
            _config = config;
        }
        #endregion



        /// <summary>
        /// Get about page section list 
        /// </summary>
        /// <param ></param>
        /// <returns></returns>

        [HttpGet("list")]
        public async Task<ApiResponse<AboutPageSectionResponseModel>> GetAboutPageSectionByCustomer( )
        {
            ApiResponse<AboutPageSectionResponseModel> response = new ApiResponse<AboutPageSectionResponseModel>() { Data = new List<AboutPageSectionResponseModel>() };
            var Path = Constants.https + HttpContext.Request.Host.Value;
            var result = await _aboutPageService.GetAboutPageSectionByCustomer();

            if (result.Count != 0)
            {
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].AboutPageSectionImage != null)
                    {
                        result[i].AboutPageSectionImage = Path + _config["Path:AboutPageSectionImagePath"] + '/' + result[i].AboutPageSectionImage;
                    }
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

        /// <summary>
        /// Get about page image list
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        [HttpGet("about-us-image-list")]
        public async Task<ApiResponse<AboutImageResponseModel>> GetAboutPageImageList()
        {
            var Path = Constants.https + HttpContext.Request.Host.Value;
            ApiResponse<AboutImageResponseModel> response = new ApiResponse<AboutImageResponseModel>() { Data = new List<AboutImageResponseModel>() };
            var result = await _aboutPageService.GetAboutPageImageList();
            if (result.Count != 0)
            {
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].AboutPageImages != null)
                    {
                        result[i].AboutPageImages = Path + _config["Path:AboutPageImagePath"] + '/' + result[i].AboutPageImages;
                    }
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
