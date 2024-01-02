using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.CommonMethod;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.Customer;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Customer;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using static SuperariLife.Common.EmailNotification.EmailNotification;

namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/customer")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        #region Fields
        private readonly ICustomerService _customerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        private readonly SMTPSettings _smtpSettings;
        private readonly AppSettings _appSettings;
        #endregion

        #region Constructor
        public CustomerController(ICustomerService customerService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService,
           Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
           IConfiguration config,
           IOptions<SMTPSettings> smtpSettings,
           IOptions<AppSettings> appSettings
         )
        {
            _customerService = customerService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
            _hostingEnvironment = hostingEnvironment;
            _config = config;
            _smtpSettings = smtpSettings.Value;
            _appSettings = appSettings.Value;
        }
        #endregion

        /// <summary>
        /// Add Customer By Admin
        /// </summary>
        /// <param name="CustomerReqModelForAdmin"></param>
        /// <returns></returns>
       
        [HttpPost("customeradd-admin")]
        public async Task<ApiPostResponse<CustomerInsertUpdateResponseModel>> InsertUpdateCustomerByAdmin([FromForm] CustomerReqModelForAdmin model)
        {
            ApiPostResponse<CustomerInsertUpdateResponseModel> response = new ApiPostResponse<CustomerInsertUpdateResponseModel>() { Data = new CustomerInsertUpdateResponseModel() };
            string customerImageFolder = _hostingEnvironment.WebRootPath + _config["Path:CustomerProfileImagePath"];

            if (!CommonMethods.IsValidEmail(model.CustomerEmail))
            {
                response.Message = ErrorMessages.InvalidEmailId;
                response.Success = false;
                return response;
            }
            TokenModel tokenModel = new TokenModel();
            string AutoGeneratepassword = "";
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
                if (model.CustomerId == 0)
                {
                    AutoGeneratepassword = Utility.GeneratePassword();
                    string[] encryptedPassword = CommonMethods.GenerateEncryptedPasswordAndPasswordSalt(AutoGeneratepassword);
                    model.CustomerPassword = encryptedPassword[0];
                    model.CustomerPasswordSalt = encryptedPassword[1];
                }
                else
                {
                    model.CustomerPassword = "";
                    model.CustomerPasswordSalt = "";
                }
                if (model.CustomerImage != null)
                {
                    var path = _hostingEnvironment.WebRootPath + _config["Path:CustomerProfileImagePath"] + "/";
                    model.ImageName = await CommonMethods.UploadImage(model.CustomerImage, path, model.CustomerEmail);  
                }
            }
            
            model.UserId = tokenModel.Id;
 
            var result = await _customerService.InsertUpdateCustomerByAdmin(model);
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
                emailBody = emailBody.Replace("##MailOf##", " Customer ");
                emailBody = emailBody.Replace("##userName##", (model.Customerfirstname + " "+ model.Customerlastname).ToString());
                emailBody = emailBody.Replace("##LogoURL##", Constants.https + path + _appSettings.EmailLogo);
                emailBody = emailBody.Replace("##envelopicon##", Constants.https + path + _appSettings.EnvelopIcon);
                emailBody = emailBody.Replace("##facebookicon##", Constants.https + path + _appSettings.FacebookIcon);
                emailBody = emailBody.Replace("##instagramicon##", Constants.https + path + _appSettings.InstagramIcon);
                emailBody = emailBody.Replace("##linkedinicon##", Constants.https + path + _appSettings.LinkedIn);
                emailBody = emailBody.Replace("##recruitmentbannerimg##", Constants.https + path + _appSettings.RecurimentBanner);
                emailBody = emailBody.Replace("##Password##", AutoGeneratepassword);
                await Task.Run(() => SendMailMessage(model.CustomerEmail, null, null, "User Password", emailBody, setting, null));
                response.Message = ErrorMessages.SaveCustomerSuccess;
                response.Success = true;
            }
            else if (result.StatusOfInsertUpdate == StatusResult.Updated)
            {
                string customerImageName = result.CustomerImageName;
                CommonMethods.DeleteFileByName(customerImageFolder, customerImageName);
                response.Message = ErrorMessages.UpdateCustomerSuccess;
                response.Success = true;
            }
            else if (result.StatusOfInsertUpdate == StatusResult.AlreadyExists)
            {
                string customerImageName = model.ImageName;
                CommonMethods.DeleteFileByName(customerImageFolder, customerImageName);
                response.Message = ErrorMessages.EmailIdExists;
                response.Success = false;
            }
            else
            {
                string customerImageName = model.ImageName;
                CommonMethods.DeleteFileByName(customerImageFolder, customerImageName);
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }

            return response;
        }

        /// <summary>
        /// Get customer List with pagination wth a option of get all user or only active user by passing value ALLUser = false for active and AllUser =ture for Active Inactive
        /// </summary>
        /// <param name="CommonPaginationModel"></param>
        /// <returns></returns>
       
        [HttpPost("list")]
        public async Task<ApiResponse<CustomerResponseModel>> GetCustomerListByAdmin([FromBody] CommonPaginationModel model)
        {
            ApiResponse<CustomerResponseModel> response = new ApiResponse<CustomerResponseModel>() { Data = new List<CustomerResponseModel>() };
            var Path = Constants.https + HttpContext.Request.Host.Value;
            var result = await _customerService.GetCustomerListByAdmin(model);

            if (result.Count != 0)
            {
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].CustomerImage != null)
                    {
                        result[i].CustomerImage = Path + _config["Path:CustomerProfileImagePath"] + '/' + result[i].CustomerImage;
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
        /// Get Customer Details By ID User
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
   
        [HttpGet("get-customer-admin/{Id}")]
        public async Task<ApiPostResponse<CustomerResponseModel>> GetCustomerById(long Id)
        {
            ApiPostResponse<CustomerResponseModel> response = new ApiPostResponse<CustomerResponseModel>() { Data = new CustomerResponseModel() };
            var Path = Constants.https + HttpContext.Request.Host.Value;

            var result = await _customerService.GetCustomerByIdByAdmin(Id);
            if (result != null)
            {
                if (result.CustomerImage != null)
                {
                    result.CustomerImage = Path + _config["Path:CustomerProfileImagePath"] + '/' + result.CustomerImage;
                }
                response.Data = result;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Delete Customer By Admin
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
       
        [HttpPost("delete/{Id}")]
        public async Task<ApiPostResponse<CustomerDeleteResponseModel>> DeleteCustomerByAdmin(long Id)
        {
          
            ApiPostResponse<CustomerDeleteResponseModel> response = new ApiPostResponse<CustomerDeleteResponseModel>() { Data = new CustomerDeleteResponseModel() };
            var result = await _customerService.DeleteCustomerByAdmin(Id);
            if (result.StatusOfDelete == Status.Success)
            {
                
                string customerImageFolder = _hostingEnvironment.WebRootPath + _config["Path:CustomerProfileImagePath"];
                string customerImageName = result.CustomerImageName;
                CommonMethods.DeleteFileByName(customerImageFolder, customerImageName); 
                response.Message = ErrorMessages.DeleteCustomerSuccess;
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
        ///Active / InActive Customer By Admin 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
       
        [HttpPost("active-inactive/{Id}")]
        public async Task<ApiPostResponse<CustomerResponseModel>> ActiveDeactiveUserByAdmin(long Id)
        {
            ApiPostResponse<CustomerResponseModel> response = new ApiPostResponse<CustomerResponseModel>() { Data = new CustomerResponseModel() };
            var result = await _customerService.ActiveDeactiveCustomerByAdmin(Id);
            if (result.IsActive == ActiveStatus.Inactive)
            {
                response.Message = ErrorMessages.CustomerDeactived;
                response.Success = true;
            }
            else if (result.IsActive == ActiveStatus.Active)
            {
                response.Message = ErrorMessages.CustomerActived;
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
