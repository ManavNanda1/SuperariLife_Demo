using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.CommonMethod;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.User;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Model.User;
using SuperariLife.Service.JWTAuthentication;
using static SuperariLife.Common.EmailNotification.EmailNotification;

namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        private readonly SMTPSettings _smtpSettings;
        private readonly AppSettings _appSettings;
        #endregion

        #region constructor
        public UserController(IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService,
           Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, 
           IOptions<SMTPSettings> smtpSettings,
           IConfiguration config,
           IOptions<AppSettings> appSettings
         )
        {
            _userService = userService;
            _smtpSettings = smtpSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
            _hostingEnvironment = hostingEnvironment;
            _config = config;
            _appSettings = appSettings.Value;
        }
        #endregion

        /// <summary>
        /// Add User By Admin
        /// </summary>
        /// <param name="UserReqModelForAdmin"></param>
        /// <returns></returns>
     
        [HttpPost("useradd-admin")]
        public async Task<ApiPostResponse<UserInsertUpdateResponseModel>> InsertUpdateUserByAdmin([FromForm] UserReqModelForAdmin model)
        {
            ApiPostResponse<UserInsertUpdateResponseModel> response = new ApiPostResponse<UserInsertUpdateResponseModel>() { Data = new UserInsertUpdateResponseModel() };

            string userImageFolder = _hostingEnvironment.WebRootPath + _config["Path:UserProfileImagePath"];

            if (!CommonMethods.IsValidEmail(model.Email))
            {
                response.Message = ErrorMessages.InvalidEmailId;
                response.Success = false;
                return response;
            }
            TokenModel tokenModel;
            string AutoGeneratepassword="";
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
                if (model.UserId == 0)
                {
                    AutoGeneratepassword = Utility.GeneratePassword();
                    string[] encryptedPassword = CommonMethods.GenerateEncryptedPasswordAndPasswordSalt(AutoGeneratepassword);
                    model.Password = encryptedPassword[0];
                    model.PasswordSalt = encryptedPassword[1];
                }
                else
                {
                    model.Password = "";
                    model.PasswordSalt = "";
                }
                if (model.UserImage != null)
                {     
                    var path = _hostingEnvironment.WebRootPath + _config["Path:UserProfileImagePath"] + "/";
                    model.ImageName = await CommonMethods.UploadImage(model.UserImage, path, model.Email);
                    model.CreatedBy = tokenModel.Id;
                }
            }    
        
            var result = await _userService.InsertUpdateUserByAdmin(model);
            if (result.StatusOfInsertUpdate > StatusResult.Updated)
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

                using (StreamReader reader = new StreamReader(Path.Combine(BasePath, Constants.AutoGeneratedPassword)))
                {
                    emailBody = reader.ReadToEnd();
                }
                var path = HttpContext.Request.Host.Value;
                emailBody = emailBody.Replace("##MailOf##", " User ");
                emailBody = emailBody.Replace("##userName##", (model.Firstname + " " + model.Lastname).ToString());
                emailBody = emailBody.Replace("##LogoURL##", Constants.https + path + _appSettings.EmailLogo);
                emailBody = emailBody.Replace("##envelopicon##", Constants.https + path + _appSettings.EnvelopIcon);
                emailBody = emailBody.Replace("##facebookicon##", Constants.https + path + _appSettings.FacebookIcon);
                emailBody = emailBody.Replace("##instagramicon##", Constants.https + path + _appSettings.InstagramIcon);
                emailBody = emailBody.Replace("##linkedinicon##", Constants.https + path + _appSettings.LinkedIn);
                emailBody = emailBody.Replace("##recruitmentbannerimg##", Constants.https + path + _appSettings.RecurimentBanner);
                emailBody = emailBody.Replace("##Password##", AutoGeneratepassword);
                await Task.Run(() => SendMailMessage(model.Email, null, null, "User Password", emailBody, setting, null));
                response.Message = ErrorMessages.SaveUserSuccess;
                response.Success = true;
            }
            else if (result.StatusOfInsertUpdate == StatusResult.Updated)
            {
                string userImageName = result.UserImageName;
                CommonMethods.DeleteFileByName(userImageFolder, userImageName);
                response.Message = ErrorMessages.UpdateUserSuccess;
                response.Success = true;
            }
            else if(result.StatusOfInsertUpdate== StatusResult.AlreadyExists)
            {
                string userImageName = model.ImageName;
                CommonMethods.DeleteFileByName(userImageFolder, userImageName);
                response.Message = ErrorMessages.EmailIdExists;
                response.Success = false;
            }
            else
            {         
                string userImageName = model.ImageName;
                CommonMethods.DeleteFileByName(userImageFolder, userImageName);
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }

            return response;
        }

        /// <summary>
        /// Add User By User
        /// </summary>
        /// <param name="UserReqModelForUser"></param>
        /// <returns></returns>
        [HttpPost("saveby-user")]
        public async Task<ApiPostResponse<UserInsertUpdateResponseModel>> InsertUserByUser([FromBody] UserReqModelForUser model)
        {
            ApiPostResponse<UserInsertUpdateResponseModel> response = new ApiPostResponse<UserInsertUpdateResponseModel>() { Data = new UserInsertUpdateResponseModel() };
            string[] encryptedPassword = CommonMethods.GenerateEncryptedPasswordAndPasswordSalt(model.Password);
                model.Password = EncryptionDecryption.GetEncrypt(encryptedPassword[0]);
                model.PasswordSalt = EncryptionDecryption.GetEncrypt(encryptedPassword[1]);
                var result = await _userService.InsertUpdateUserByUser(model);
                if (result.StatusOfInsertUpdate > StatusResult.Updated)
                {
                    response.Message = ErrorMessages.SaveUserSuccess;
                    response.Success = true;
                }
                else if(result.StatusOfInsertUpdate == StatusResult.AlreadyExists)
                {
                    response.Message = ErrorMessages.EmailIdExists;
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
        /// Update User Profile By User
        /// </summary>
        /// <param name="UserReqModelForUser"></param>
        /// <returns></returns>
        [HttpPost("updateby-user")]
       
        public async Task<ApiPostResponse<UserInsertUpdateResponseModel>> UpdateUserProfileByUser([FromForm] UserReqModelForUser model)
        {
            TokenModel tokenModel = new TokenModel();
            ApiPostResponse<UserInsertUpdateResponseModel> response = new ApiPostResponse<UserInsertUpdateResponseModel>() { Data = new UserInsertUpdateResponseModel() };
            string userImageFolder = _hostingEnvironment.WebRootPath + _config["Path:UserProfileImagePath"];
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }

            if (model.UserImage != null)
            {
                var path = _hostingEnvironment.WebRootPath + _config["Path:UserProfileImagePath"] + "/" ;
                model.ImageName = await CommonMethods.UploadImage(model.UserImage,path,tokenModel.EmailId);
            }
            var result = await _userService.InsertUpdateUserByUser(model);
            if (result.StatusOfInsertUpdate == StatusResult.Updated)
            {
                string userImageName = result.UserImageName;
                CommonMethods.DeleteFileByName(userImageFolder, userImageName);
                response.Message = ErrorMessages.UpdateUserSuccess;
                response.Success = true;
            }
            else
            {
                string userImageName = model.ImageName;
                CommonMethods.DeleteFileByName(userImageFolder, userImageName);
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }

            return response;
        }

        /// <summary>
        /// Get User List with pagination wth a option of get all user or only active user by passing value ALLUser = false for active and AllUser =ture for Active Inactive
        /// </summary>
        /// <param name="CommonPaginationModel"></param>
        /// <returns></returns>
       
        [HttpPost("list")]
        public async Task<ApiResponse<ResponseUserModel>> GetUserListByAdmin([FromBody] CommonPaginationModel model)
        {
            ApiResponse<ResponseUserModel> response = new ApiResponse<ResponseUserModel>() { Data = new List<ResponseUserModel>() };
             var Path = Constants.https + HttpContext.Request.Host.Value;
            var result = await _userService.GetUserListByAdmin(model);

            if (result.Count != 0)
            {
                for(var i = 0; i < result.Count; i++)
                {
                    if (result[i].UserImage != null)
                    {
                        result[i].UserImage = Path + _config["Path:UserProfileImagePath"] + '/' + result[i].UserImage;
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
        /// Get User List for enum drop down
        /// </summary>
        /// <param></param>
        /// <returns></returns>

        [HttpGet("user-dropdown-list")]
        public async Task<ApiResponse<ResponseUserModel>> GetUserListForDropDownList()
        {
            ApiResponse<ResponseUserModel> response = new ApiResponse<ResponseUserModel>() { Data = new List<ResponseUserModel>() };
            var result = await _userService.GetUserListForDropDownList();
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
        /// Get User Details By ID User
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>

        [HttpGet("get-user-user/{Id}")]
        public async Task<ApiPostResponse<ResponseUserModel>> GetUserById(long Id)
        {
            ApiPostResponse<ResponseUserModel> response = new ApiPostResponse<ResponseUserModel>() { Data = new ResponseUserModel() };
            var Path = Constants.https + HttpContext.Request.Host.Value;
         

            var result = await _userService.GetUserById(Id);
            if (result != null)
            {
                if (result.UserImage != null)
                {           
                    result.UserImage = Path + _config["Path:UserProfileImagePath"] + '/' + result.UserImage;
                }
                response.Data = result;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Delete User By Admin
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
    
        [HttpPost("delete/{Id}")]
        public async Task<ApiPostResponse<UserDeleteResponseModel>> DeleteUserByAdmin(long Id)
        {
            ApiPostResponse<UserDeleteResponseModel> response = new ApiPostResponse<UserDeleteResponseModel>() { Data = new UserDeleteResponseModel() };
            var result = await _userService.DeleteUserByAdmin(Id);
            if (result.StatusOfDelete == Status.Success)
            {
                string userImageFolder  = _hostingEnvironment.WebRootPath + _config["Path:UserProfileImagePath"];
                string userImageName= result.UserImageName;
                CommonMethods.DeleteFileByName(userImageFolder, userImageName);
                response.Message = ErrorMessages.DeleteUserSuccess;
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
        ///Active / InActive User By Admin 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
       
        [HttpPost("active-inactive/{Id}")]
        public async Task<ApiPostResponse<ResponseUserModel>> ActiveDeactiveUserByAdmin(long Id)
        {
            ApiPostResponse<ResponseUserModel> response = new ApiPostResponse<ResponseUserModel>() { Data = new ResponseUserModel() };
            var result = await _userService.ActiveDeactiveUserByAdmin(Id);
            if (result.IsActive == ActiveStatus.Inactive)
            {
               
                response.Message = ErrorMessages.UserDeactived;
                response.Success = true;
            }
            else if (result.IsActive == ActiveStatus.Active)
            {
                response.Message = ErrorMessages.UserActived;
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
