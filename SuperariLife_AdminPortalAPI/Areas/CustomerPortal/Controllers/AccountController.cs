using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using static SuperariLife.Common.EmailNotification.EmailNotification;
using SuperariLife.Common.CommonMethod;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Login;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Account;
using SuperariLife.Service.JWTAuthentication;

namespace SuperariLifeAPI.Areas.CustomerPortal.Controllers
{
    [Route("api/customer/account")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        #region Field
        private readonly IAccountService _accountService;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SMTPSettings _smtpSettings;
        private readonly IConfiguration _config;

        #endregion

        #region Constructor
        public AccountController(
            IAccountService accountService,
            IJWTAuthenticationService jwtAuthenticationService,
            IOptions<AppSettings> appSettings,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration config,
            IOptions<SMTPSettings> smtpSettings
            )
        {
            _accountService = accountService;
            _jwtAuthenticationService = jwtAuthenticationService;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _smtpSettings = smtpSettings.Value;
            _config = config;
        }
        #endregion

        /// <summary>
        /// Login with Email to customer
        /// </summary>
        /// <param name="LoginRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<ApiPostResponse<CustomerLoginResponseModel>> LoginCustomer([FromBody] LoginRequestModel model)

        {
            ApiPostResponse<CustomerLoginResponseModel> response = new ApiPostResponse<CustomerLoginResponseModel>() { Data = new CustomerLoginResponseModel() };
            model.Password = EncryptionDecryption.GetEncrypt(model.Password);

            SaltResponseModel res = await _accountService.GetCustomerSaltByEmail(model.Email);
            if (res.Status == LoginStatus.CustomerDeactive)
            {
                response.Success = false;
                response.Message = ErrorMessages.CustomerIsDeActivatedByAdmin;
                return response;
            }
            else if (res.Status == LoginStatus.CustomerDeleted)
            {
                response.Success = false;
                response.Message = ErrorMessages.CustomerIsDeletedByAdmin;
                return response;
            }
            else if (res.Status == LoginStatus.EmailNotExist)
            {
                response.Success = false;
                response.Message = ErrorMessages.EmailNotExist;
                return response;

            }
            else if (res == null)
            {
                response.Success = false;
                response.Message = ErrorMessages.InvalidEmailId;
                return response;
            }
            else
            {
                string Hash = EncryptionDecryption.GetDecrypt(res.CustomerPassword);
                string Salt = EncryptionDecryption.GetDecrypt(res.PasswordSalt);

                bool isPasswordMatched = EncryptionDecryption.Verify(model.Password, Hash, Salt);
                var Path = Constants.https + HttpContext.Request.Host.Value;
                if (isPasswordMatched)
                {
                    model.Password = res.CustomerPassword;
                    CustomerLoginResponseModel result = await _accountService.CustomerLogin(model);
                    if (result != null && result.CustomerId > 0)
                    {
                        TokenModel objTokenData = new TokenModel();
                        objTokenData.EmailId = model.Email;
                        objTokenData.Id = result.CustomerId != null ? result.CustomerId : 0;
                        objTokenData.FullName = result.Customerfirstname + result.Customerlastname;
                        objTokenData.FirstName = result.Customerfirstname;
                        objTokenData.LastName = result.Customerlastname;
                        objTokenData.IsSuperAdmin = false;

                        AccessTokenModel objAccessTokenData = _jwtAuthenticationService.GenerateToken(objTokenData, _appSettings.JWT_Secret, _appSettings.JWT_Validity_Mins);
                        result.Token = objAccessTokenData.Token;

                        await _accountService.UpdateLoginTokenForCustomer(objAccessTokenData.Token, objAccessTokenData.Id);

                        response.Message = ErrorMessages.LoginSuccess;
                        response.Success = true;
                        response.Data.Token = result.Token.ToString();
                        response.Data.CustomerId = result.CustomerId;
                        response.Data.Customerfirstname = result.Customerfirstname;
                        response.Data.Customerlastname = result.Customerlastname;
                        response.Data.CustomerEmail = result.CustomerEmail;
                        response.Data.CustomerImage = Path + _config["Path:CustomerProfileImagePath"] + "/" + result.CustomerImage;
                        return response;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = ErrorMessages.InvalidCredential;
                        return response;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = ErrorMessages.InvalidCredential;
                    return response;
                }
            }

        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<BaseApiResponse> Logout()
        {
            BaseApiResponse response = new BaseApiResponse();

            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
            var data = await _accountService.LogoutCustomer(TokenData.Id);
            if (data > 0)
            {

                response.Message = ErrorMessages.CustomerLogoutSuccess;
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
        /// Forget Passowrd  with Email to Customer with link
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("forget-password")]
        public async Task<BaseApiResponse> ForgetPasswordWithURL([FromBody] ForgetPasswordRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            string emailBody;
            string EncryptedCustomerId;
            var result = await _accountService.ForgetPasswordForCustomer(model.EmailId);
            if (result == null)
            {
                response.Message = ErrorMessages.NotRegisterEmailId;
                response.Success = false;
            }
            else if (result.CustomerId > 0)
            {
                EncryptedCustomerId = EncryptionDecryption.GetEncrypt(result.CustomerId.ToString());
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

                string BasePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.EmailTemplate);

                if (!Directory.Exists(BasePath))
                {
                    Directory.CreateDirectory(BasePath);
                }
                bool isSuccess = false;

                using (StreamReader reader = new StreamReader(Path.Combine(BasePath, Constants.ForgetPasswordEmailtem)))
                {
                    emailBody = reader.ReadToEnd();
                }
                var path = HttpContext.Request.Host.Value;
                emailBody = emailBody.Replace("##MailOf##", " Customer ");
                emailBody = emailBody.Replace("##userName##", " Customer ");
                emailBody = emailBody.Replace("##LogoURL##", Constants.https + path + _appSettings.EmailLogo);
                emailBody = emailBody.Replace("##envelopicon##", Constants.https + path + _appSettings.EnvelopIcon);
                emailBody = emailBody.Replace("##facebookicon##", Constants.https + path + _appSettings.FacebookIcon);
                emailBody = emailBody.Replace("##instagramicon##", Constants.https + path + _appSettings.InstagramIcon);
                emailBody = emailBody.Replace("##linkedinicon##", Constants.https + path + _appSettings.LinkedIn);
                emailBody = emailBody.Replace("##recruitmentbannerimg##", Constants.https + path + _appSettings.RecurimentBanner);
                emailBody = emailBody.Replace("##Password##", (model.ForgetPasswordUrl + '/' + EncryptedCustomerId).ToString());
                isSuccess = await Task.Run(() => SendMailMessage(model.EmailId, null, null, "Reset password link", emailBody, setting, null));
                if (isSuccess)
                {
                    response.Message = ErrorMessages.ForgetPasswordSuccessEmail;
                    response.Success = true;
                }
                else
                {
                    response.Message = ErrorMessages.EmailVerifyOrWait;
                    response.Success = true;
                }
            }
            else
            {
                response.Message = ErrorMessages.CutomerError;
                response.Success = false;
                Response.StatusCode = StatusCodes.Status404NotFound;

            }
            return response;
        }

        /// <summary>
        /// Password Change With URL Link
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("success-forget-password")]
        public async Task<BaseApiResponse> SuccessForgetPasswordChangeURL([FromBody] PasswordChangeRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            string decryptedId = EncryptionDecryption.GetDecrypt(model.EncryptedCustomerId);
            model.CustomerId = Convert.ToInt64(decryptedId);
            string[] encryptedPassword = CommonMethods.GenerateEncryptedPasswordAndPasswordSalt(model.Password);
            model.Password = encryptedPassword[0];
            model.PasswordSalt = encryptedPassword[1];
            var result = await _accountService.SuccessForgetPasswordChangeURLForCustomer(model);
            if (result == Status.URLExpired)
            {
                response.Success = false;
                response.Message = ErrorMessages.UrlForPasswordChangeExpired;
            }
            else if (result == Status.URLUsed)
            {
                response.Success = false;
                response.Message = ErrorMessages.URLAlreadyUsed;
            }
            else if (result == Status.Success)
            {
                response.Success = true;
                response.Message = ErrorMessages.ResetPasswordSuccess;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }
            return response;

        }
       

        /// <summary>
        /// Change  password to customer through portal
        /// </summary>
        /// <param name="ChangePasswordRequestModel"></param>
        /// <returns></returns>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<BaseApiResponse> UpdatePasswordByCustomer([FromBody] ChangePasswordRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.OldPassword = EncryptionDecryption.GetEncrypt(model.OldPassword.Trim());
            model.CreatePassword = EncryptionDecryption.GetEncrypt(model.CreatePassword.Trim());
            model.ConfirmPassword = EncryptionDecryption.GetEncrypt(model.ConfirmPassword.Trim());
            var res = await _accountService.GetCustomerSaltByEmail(tokenModel.EmailId);
            bool isPasswordSame = true;
            bool isPasswordMatched = true;
            if (tokenModel.Id == 0 || string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.CreatePassword) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                response.Message = ErrorMessages.PasswordFieldValidation;
                response.Success = false;
                return response;
            }
            if (res != null)
            {
                string Hash = EncryptionDecryption.GetDecrypt(res.CustomerPassword);
                string Salt = EncryptionDecryption.GetDecrypt(res.PasswordSalt);

                isPasswordMatched = EncryptionDecryption.Verify(model.OldPassword, Hash, Salt);
                isPasswordSame = EncryptionDecryption.Verify(model.CreatePassword, Hash, Salt);
            }
            #region Validation 
            if (!isPasswordMatched)
            {
                response.Message = ErrorMessages.PasswordCheck;
                response.Success = false;
                return response;
            }
            if (isPasswordSame)
            {
                response.Message = ErrorMessages.PasswordMatch;
                response.Success = false;
                return response;
            }
            if (string.IsNullOrEmpty(model.CreatePassword) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                response.Message = ErrorMessages.PasswordValidation;
                response.Success = false;
                return response;
            }
            if (model.CreatePassword != model.ConfirmPassword)
            {
                response.Message = ErrorMessages.ConfirmPassword;
                response.Success = false;
                return response;
            }
            if (!CommonMethods.IsPasswordStrong(EncryptionDecryption.GetDecrypt(model.CreatePassword)))
            {
                response.Message = ErrorMessages.StrongPassword;
                response.Success = false;
                return response;
            }
            #endregion
            string hashed = EncryptionDecryption.Hash(model.CreatePassword);
            string[] segments = hashed.Split(":");
            string EncryptedHash = EncryptionDecryption.GetEncrypt(segments[0]);
            string EncryptedSalt = EncryptionDecryption.GetEncrypt(segments[1]);
            var result = await _accountService.ChangePasswordForCustomer(tokenModel.Id, EncryptedHash, EncryptedSalt);
            if (string.IsNullOrEmpty(result))
            {
                response.Message = ErrorMessages.ResetPasswordSuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
                Response.StatusCode = StatusCodes.Status403Forbidden;
            }
            return response;
        }

    }
}
