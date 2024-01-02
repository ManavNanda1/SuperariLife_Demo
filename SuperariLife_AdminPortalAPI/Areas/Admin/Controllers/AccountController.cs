using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.CommonMethod;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Login;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Account;
using SuperariLife.Service.JWTAuthentication;
using static SuperariLife.Common.EmailNotification.EmailNotification;

namespace SuperariLifeAPI.Controllers
{
    [Route("api/admin/account")]
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
        /// Login with Email to user
        /// </summary>
        /// <param name="LoginRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<ApiPostResponse<LoginResponseModel>> LoginUser([FromBody] LoginRequestModel model)
        
        {
            ApiPostResponse<LoginResponseModel> response = new ApiPostResponse<LoginResponseModel>() { Data = new LoginResponseModel() };
            model.Password = EncryptionDecryption.GetEncrypt(model.Password);

            SaltResponseModel res = await _accountService.GetUserSalt(model.Email);
            if (res.Status == LoginStatus.UserDeactive)
            {
                response.Success = false;
                response.Message = ErrorMessages.UserIsDeActivatedByAdmin;
                return response;
            }
            else if(res.Status == LoginStatus.UserDeleted)
            {
                response.Success = false;
                response.Message=ErrorMessages.UserIsDeletedByAdmin;
                return response;
            }
            else if (res.Status == LoginStatus.EmailNotExist)
            {
                response.Success = false;
                response.Message = ErrorMessages.EmailNotExist;
                return response;

            }
            else if(res == null)
            {
                response.Success = false;
                response.Message=ErrorMessages.InvalidEmailId; 
                return response;
            }
            else
            {
                string Hash = EncryptionDecryption.GetDecrypt(res.Password);
                string Salt = EncryptionDecryption.GetDecrypt(res.PasswordSalt);

                bool isPasswordMatched = EncryptionDecryption.Verify(model.Password, Hash, Salt);
                var Path = Constants.https + HttpContext.Request.Host.Value;
                if (isPasswordMatched)
                {
                    model.Password = res.Password;
                    LoginResponseModel result = await _accountService.LoginUser(model);
                    if (result != null && result.UserId > 0)
                    {
                        TokenModel objTokenData = new TokenModel();
                        objTokenData.EmailId = model.Email;
                        objTokenData.Id = result.UserId != null ? result.UserId : 0;
                        objTokenData.FullName = result.Firstname + result.Lastname;
                        objTokenData.RoleId = result.RoleManagementId;
                        objTokenData.FirstName = result.Firstname;
                        objTokenData.LastName = result.Lastname;
                        objTokenData.IsSuperAdmin = true;

                        AccessTokenModel objAccessTokenData = _jwtAuthenticationService.GenerateToken(objTokenData, _appSettings.JWT_Secret, _appSettings.JWT_Validity_Mins);
                        result.Token = objAccessTokenData.Token;

                        await _accountService.UpdateLoginToken(objAccessTokenData.Token, objAccessTokenData.Id);

                        response.Message = ErrorMessages.LoginSuccess;
                        response.Success = true;
                        response.Data.Token = result.Token.ToString();
                        response.Data.UserId = result.UserId;
                        response.Data.Firstname = result.Firstname;
                        response.Data.Lastname = result.Lastname;
                        response.Data.Email = result.Email;
                        response.Data.UserImage = Path + _config["Path:UserProfileImagePath"] + "/"  + result.UserImage;
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
            TokenModel userTokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
            var data = await _accountService.LogoutUser(userTokenData.Id);
            if (data > 0)
            {
               
                response.Message = ErrorMessages.UserLogoutSuccess;
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
        /// Forget Passowrd  with Email to user with link
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("forget-password")]
        public async Task<BaseApiResponse> ForgetPasswordWithURL([FromBody] ForgetPasswordRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            string emailBody;
            string EncryptedUserId;
            var result = await _accountService.ForgetPassword(model.EmailId);
            if (result == null)
            {
                response.Message = ErrorMessages.NotRegisterEmailId;
                response.Success = false;
            }
           else if (result.UserId > 0)
            {
                 EncryptedUserId = EncryptionDecryption.GetEncrypt(result.UserId.ToString());
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
                emailBody = emailBody.Replace("##MailOf##", " User ");
                emailBody = emailBody.Replace("##userName##", " User ");
                emailBody = emailBody.Replace("##LogoURL##", Constants.https + path + _appSettings.EmailLogo);
                emailBody = emailBody.Replace("##envelopicon##", Constants.https + path + _appSettings.EnvelopIcon);
                emailBody = emailBody.Replace("##facebookicon##", Constants.https + path + _appSettings.FacebookIcon);
                emailBody = emailBody.Replace("##instagramicon##", Constants.https + path + _appSettings.InstagramIcon);
                emailBody = emailBody.Replace("##linkedinicon##", Constants.https + path + _appSettings.LinkedIn);
                emailBody = emailBody.Replace("##recruitmentbannerimg##", Constants.https + path + _appSettings.RecurimentBanner);
                emailBody = emailBody.Replace("##Password##",(model.ForgetPasswordUrl + '/' + EncryptedUserId).ToString());
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
                response.Message = ErrorMessages.UserError;
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
            string decryptedId = EncryptionDecryption.GetDecrypt(model.EncryptedUserId);
            model.UserId = Convert.ToInt64(decryptedId);
            string[] encryptedPassword = CommonMethods.GenerateEncryptedPasswordAndPasswordSalt(model.Password);
            model.Password = encryptedPassword[0];
            model.PasswordSalt = encryptedPassword[1];
            var result = await _accountService.SuccessForgetPasswordChangeURL(model);
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
            else if (result==Status.Success)
            {
               response.Success=true;
               response.Message=ErrorMessages.ResetPasswordSuccess;    
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong; 
                response.Success = false;
            }
            return response;

        }

        /// <summary>
        /// Validate whether the Reset Passowrd OTP is Correct or not
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("verification-code")]
        public async Task<BaseApiResponse> verificationCode([FromBody] VerificationOTPRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            if (ModelState.IsValid)
            {

                int PasswrodValid = _appSettings.PasswordLinkValidityMins;
                long UserId = await _accountService.GetUserIDByEmail(model.EmailId);
                string result = await _accountService.VerificationCode(UserId, model.OTP, PasswrodValid);
                if (string.IsNullOrEmpty(result))
                {
                    response.Message = ErrorMessages.VerifyOTP;
                    response.Success = true;
                }
                else
                {
                    response.Message = result;
                    response.Success = false;
                }
            }
            return response;
        }

        /// <summary>
        /// Reset Password of User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        [Authorize]
        public async Task<BaseApiResponse> ResetPassword([FromBody] ResetPasswordRequestModel model)
        {
            model.ConfirmPassword = model.ConfirmPassword.Trim();
            model.NewPassword = model.NewPassword.Trim();

            BaseApiResponse response = new BaseApiResponse();
            #region Validation 
            if (string.IsNullOrEmpty(model.EmailId))
            {
                response.Message = ErrorMessages.EmailIsRequired;
                response.Success = false;
                return response;
            }
            if (!CommonMethods.IsValidEmail(model.EmailId))
            {
                response.Message = ErrorMessages.EnterValidEmail;
                response.Success = false;
                return response;
            }
            if (string.IsNullOrEmpty(model.NewPassword))
            {
                response.Message = ErrorMessages.PasswordValidation;
                response.Success = false;
                return response;
            }
            if (string.IsNullOrEmpty(model.ConfirmPassword))
            {
                response.Message = ErrorMessages.PasswordValidationConfirm;
                response.Success = false;
                return response;
            }
            if (model.NewPassword != model.ConfirmPassword)
            {
                response.Message = ErrorMessages.ConfirmPassword;
                response.Success = false;
                return response;
            }
            if (!CommonMethods.IsPasswordStrong(model.NewPassword))
            {
                response.Message = ErrorMessages.StrongPassword;
                response.Success = false;
                return response;
            }
            #endregion
            string hashed = EncryptionDecryption.Hash(EncryptionDecryption.GetEncrypt(model.NewPassword));
            string[] segments = hashed.Split(":");
            string EncryptedHash = EncryptionDecryption.GetEncrypt(segments[0]);
            string EncryptedSalt = EncryptionDecryption.GetEncrypt(segments[1]);
            long UserId = await _accountService.GetUserIDByEmail(model.EmailId);
            var result = await _accountService.ResetPassword(UserId, model.EmailId, EncryptedHash, EncryptedSalt);
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

        /// <summary>
        /// Change  password to user through portal
        /// </summary>
        /// <param name="ChangePasswordRequestModel"></param>
        /// <returns></returns>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<BaseApiResponse> UpdatePasswordByUser([FromBody] ChangePasswordRequestModel model)
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
            var res = await _accountService.GetUserSalt(tokenModel.EmailId);
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
                string Hash = EncryptionDecryption.GetDecrypt(res.Password);
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
            var result = await _accountService.ChangePassword(tokenModel.Id, EncryptedHash, EncryptedSalt);
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
