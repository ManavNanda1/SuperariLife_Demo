
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.CommonMethod;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.Customer;
using SuperariLife.Model.Customer;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;

namespace SuperariLifeAPI.Areas.CustomerPortal.Controllers
{
    [Route("api/customer/customer")]
    [ApiController]
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
        /// Add  Customer
        /// </summary>
        /// <param name="CustomerReqModelForAdmin"></param>
        /// <returns></returns>

        [HttpPost("save")]
        public async Task<ApiPostResponse<CustomerInsertUpdateResponseModel>> Register([FromBody] CustomerReqModelForAdmin model)
        {
            ApiPostResponse<CustomerInsertUpdateResponseModel> response = new ApiPostResponse<CustomerInsertUpdateResponseModel>() { Data = new CustomerInsertUpdateResponseModel() };
            string customerImageFolder = _hostingEnvironment.WebRootPath + _config["Path:CustomerProfileImagePath"];
        

            if (!CommonMethods.IsValidEmail(model.CustomerEmail))
            {
                response.Message = ErrorMessages.InvalidEmailId;
                response.Success = false;
                return response;
            }

            if (model.CustomerImage != null)
            {
                var path = _hostingEnvironment.WebRootPath + _config["Path:CustomerProfileImagePath"] + "/";
                model.ImageName = await CommonMethods.UploadImage(model.CustomerImage, path, model.CustomerEmail);
            }
            string[] encryptedPassword = CommonMethods.GenerateEncryptedPasswordAndPasswordSalt(model.CustomerPassword);
            model.CustomerPassword = encryptedPassword[0];
            model.CustomerPasswordSalt = encryptedPassword[1];

            var result = await _customerService.InsertUpdateCustomer(model);
            if (result.StatusOfInsertUpdate > StatusResult.Updated)
            {
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
        /// Add  Customer
        /// </summary>
        /// <param name="CustomerReqModelForAdmin"></param>
        /// <returns></returns>

        [HttpPost("update")]
        [Authorize]
        public async Task<ApiPostResponse<CustomerInsertUpdateResponseModel>> UpdateCustomer([FromForm] CustomerReqModelForAdmin model)
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
         
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
               
                if (model.CustomerImage != null)
                {
                    var path = _hostingEnvironment.WebRootPath + _config["Path:CustomerProfileImagePath"] + "/";
                    model.ImageName = await CommonMethods.UploadImage(model.CustomerImage, path, model.CustomerEmail);
                }
            }

            model.CustomerId = tokenModel.Id;


            var result = await _customerService.InsertUpdateCustomer(model);
            if (result.StatusOfInsertUpdate > StatusResult.Updated)
            {
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
        /// Get Customer Details By Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>

        [HttpGet("get-customer-detail/{Id}")]
        [Authorize]
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
    }
}
