using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.CommonMethod;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.SettingPages;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.AboutPage;


namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/about-us")]
    [ApiController]
    [Authorize]
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

        #region About Page Image
        /// <summary>
        /// Add Update about image  
        /// </summary>
        /// <param name="AboutImageReqModel"></param>
        /// <returns></returns>

        [HttpPost("save")]
        public async Task<BaseApiResponse> InsertUpdateAboutPageImage([FromForm] AboutImageReqModel model)
        {
            List<string> AboutPageImageName = new List<string>();
            BaseApiResponse response = new BaseApiResponse();
            TokenModel tokenModel = new TokenModel();
            string aboutPageImageFolder = _hostingEnvironment.WebRootPath + _config["Path:AboutPageImagePath"];
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
                if (model.AboutPageImages != null)
                {
                    if (model.AboutPageImages.Count > 0)
                    {

                        string aboutImagePath = _hostingEnvironment.WebRootPath + _config["Path:AboutPageImagePath"]  +  "/" ;
                        for (int i = 0; i < model.AboutPageImages.Count; i++)
                        {

                            var aboutImageName = await CommonMethods.UploadImage(model.AboutPageImages[i], aboutImagePath, "");
                            AboutPageImageName.Add(aboutImageName);
                        }
                    }
                }

            }
            model.UserId = tokenModel.Id;

            var result = await _aboutPageService.InsertUpdateAboutPageImage(model, AboutPageImageName);
            if (result > StatusResult.Updated)
            {
                response.Message = ErrorMessages.SaveAboutPageImageSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.Updated)
            {
                
                response.Message = ErrorMessages.UpdateAboutPageImageSuccess;
                response.Success = true;
            }
            else
            {
                for(int i=0;i < AboutPageImageName.Count; i++ )
                {
                    CommonMethods.DeleteFileByName(aboutPageImageFolder, AboutPageImageName[i]);
                }
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }

            return response;
        }

        /// <summary>
        /// Get About Page Image By ID   
        /// </summary>
        /// <param name="aboutPageImageId"></param>
        /// <returns></returns>
        [HttpGet("about-us-image/{aboutPageImageId}")]
        public async Task<ApiPostResponse<AboutImageResponseModel>> GetAboutPageImageById(int aboutPageImageId)
        {
            var Path = Constants.https + HttpContext.Request.Host.Value;


            ApiPostResponse<AboutImageResponseModel> response = new ApiPostResponse<AboutImageResponseModel>() { Data = new AboutImageResponseModel() };

            var result = await _aboutPageService.GetAboutPageImageById(aboutPageImageId);
            if (result != null)
            {
                if (result.AboutPageImages != null)
                {
                    result.AboutPageImages = Path + _config["Path:AboutPageImagePath"] +  '/' + result.AboutPageImages ;
                }
                response.Data = result;
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

        /// <summary>
        /// Delete about Page Image By Admin
        /// </summary>
        /// <param name="aboutPageImageId"></param>
        /// <returns></returns>

        [HttpPost("delete/{aboutPageImageId}")]
        public async Task<ApiPostResponse<CommonAboutPageDeleteModel>> DeleteAboutPageImage(int aboutPageImageId)
        {
            ApiPostResponse<CommonAboutPageDeleteModel> response = new ApiPostResponse<CommonAboutPageDeleteModel>() { Data = new CommonAboutPageDeleteModel() };
            var result = await _aboutPageService.DeleteAboutPageImage(aboutPageImageId);
            if (result.StatusOfDelete == Status.Success)
            {
                string aboutpageImageFolder = _hostingEnvironment.WebRootPath + _config["Path:AboutPageImagePath"];
                string aboutpageImageName = result.ImageName;
                CommonMethods.DeleteFileByName(aboutpageImageFolder, aboutpageImageName);
                response.Message = ErrorMessages.DeleteAboutPageImageSuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }
            return response;
        }


        #endregion


        #region  About Page Section

        /// <summary>
        /// Add Update about page section By Admin
        /// </summary>
        /// <param name="AboutPageSectionReqModel"></param>
        /// <returns></returns>

        [HttpPost("save-about-us-section")]
        public async Task<ApiPostResponse<AboutInsertUpdateResponseModel>> InsertUpdateAboutPageSection([FromForm] AboutPageSectionReqModel model)
        {
            ApiPostResponse<AboutInsertUpdateResponseModel> response = new ApiPostResponse<AboutInsertUpdateResponseModel>() { Data = new AboutInsertUpdateResponseModel() };

            string aboutPageSectionImageFolder = _hostingEnvironment.WebRootPath + _config["Path:AboutPageSectionImagePath"];

            TokenModel tokenModel;
            string AutoGeneratepassword = "";
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
               
                if (model.AboutPageSectionImage != null)
                {
                    var path = _hostingEnvironment.WebRootPath + _config["Path:AboutPageSectionImagePath"] + "/";
                    model.AboutPageSectionImageName = await CommonMethods.UploadImage(model.AboutPageSectionImage, path,"");     
                }
                model.UserId = tokenModel.Id;
            }

            var result = await _aboutPageService.InsertUpdateAboutPageSection(model);
            if (result.StatusOfInsertUpdate > StatusResult.Updated)
            {            
                response.Message = ErrorMessages.SaveAboutPageSectionSuccess;
                response.Success = true;
            }
            else if (result.StatusOfInsertUpdate == StatusResult.Updated)
            {
                string aboutPageSectionImageName = result.AboutPageSectionImageName;
                CommonMethods.DeleteFileByName(aboutPageSectionImageFolder, aboutPageSectionImageName);
                response.Message = ErrorMessages.UpdateAboutPageImageSuccess;
                response.Success = true;
            }
            else
            {
                string aboutPageSectionImageName = model.AboutPageSectionImageName;
                CommonMethods.DeleteFileByName(aboutPageSectionImageFolder, aboutPageSectionImageName);
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }

            return response;
        }



        /// <summary>
        /// Get about page section list 
        /// </summary>
        /// <param name="CommonPaginationModel"></param>
        /// <returns></returns>

        [HttpPost("list")]
        public async Task<ApiResponse<AboutPageSectionResponseModel>> GetAboutPageSectionList([FromBody] CommonPaginationModel model)
        {
            ApiResponse<AboutPageSectionResponseModel> response = new ApiResponse<AboutPageSectionResponseModel>() { Data = new List<AboutPageSectionResponseModel>() };
            var Path = Constants.https + HttpContext.Request.Host.Value;
            var result = await _aboutPageService.GetAboutPageSectionList(model);

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
        #endregion


        /// <summary>
        /// Get About Page Section Layout Type For DropDown
        /// </summary>
        /// <param></param>
        /// <returns></returns>

        [HttpGet("page-section-layout-type-dropdown-list")]
        public async Task<ApiResponse<PageSectionLayoutTypeModel>> GetAboutPageSectionLayoutTypeForDropDown()
        {
            ApiResponse<PageSectionLayoutTypeModel> response = new ApiResponse<PageSectionLayoutTypeModel>() { Data = new List<PageSectionLayoutTypeModel>() };
            var result = await _aboutPageService.GetAboutPageSectionLayoutTypeForDropDown();
            if (result.Count != null)
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
        /// Get about page section by id
        /// </summary>
        /// <param name="aboutPageSectionId"></param>
        /// <returns></returns>

        [HttpGet("get-about-us-section/{aboutPageSectionId}")]
        public async Task<ApiPostResponse<AboutPageSectionResponseModel>> GetAboutPageSectionById(int aboutPageSectionId)
        {
            ApiPostResponse<AboutPageSectionResponseModel> response = new ApiPostResponse<AboutPageSectionResponseModel>() { Data = new AboutPageSectionResponseModel() };
            var Path = Constants.https + HttpContext.Request.Host.Value;


            var result = await _aboutPageService.GetAboutPageSectionById(aboutPageSectionId);
            if (result != null)
            {
                if (result.AboutPageSectionImage != null)
                {
                    result.AboutPageSectionImage = Path + _config["Path:AboutPageSectionImagePath"] + '/' + result.AboutPageSectionImage;
                }
                response.Data = result;
            }
            response.Success = true;
            return response;
        }


        /// <summary>
        /// Delete about Page section By Admin
        /// </summary>
        /// <param name="aboutPageSectionId"></param>
        /// <returns></returns>

        [HttpPost("delete-about-us-section/{aboutPageSectionId}")]
        public async Task<ApiPostResponse<CommonAboutPageDeleteModel>> DeleteAboutPageSection(int aboutPageSectionId)
        {
            ApiPostResponse<CommonAboutPageDeleteModel> response = new ApiPostResponse<CommonAboutPageDeleteModel>() { Data = new CommonAboutPageDeleteModel() };
            var result = await _aboutPageService.DeleteAboutPageSection(aboutPageSectionId);
            if (result.StatusOfDelete == Status.Success)
            {
                string aboutpageImageFolder = _hostingEnvironment.WebRootPath + _config["Path:AboutPageSectionImagePath"];
                string aboutpageImageName = result.ImageName;
                CommonMethods.DeleteFileByName(aboutpageImageFolder, aboutpageImageName);
                response.Message = ErrorMessages.DeleteAboutPageSectionSuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }
            return response;
        }

        [HttpGet("delete-about-us-section-image/{aboutPageSectionId}")]
        public async Task<BaseApiResponse> DeleteAboutPageSectionImage(long aboutPageSectionId)
        {
            BaseApiResponse response = new BaseApiResponse() ;
            var result = await _aboutPageService.DeleteAboutSectionImage(aboutPageSectionId);
            if (result == Status.Success)
            {
                response.Message = "Image Deleted Succesfully";
                response.Success = true;  
            }
            else if( result == Status.Failed)
            {
                response.Message = "Image Not Deleted";
                response.Success = false;
            }
            return response;
        }




    }
}
