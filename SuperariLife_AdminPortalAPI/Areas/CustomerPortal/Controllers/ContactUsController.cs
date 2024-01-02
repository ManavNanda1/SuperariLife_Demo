using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.ContactUs;
using SuperariLife.Model.Settings;
using SuperariLife.Service.Account;
using SuperariLife.Service.JWTAuthentication;
using static SuperariLife.Common.EmailNotification.EmailNotification;

namespace SuperariLifeAPI.Areas.CustomerPortal.Controllers
{
    [Route("api/customer/contact-us")]
    [ApiController]

    public class ContactUsController : ControllerBase
    {
        #region Field
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SMTPSettings _smtpSettings;
        private readonly IConfiguration _config;

        #endregion

        #region Constructor
        public ContactUsController(
            IAccountService accountService,
            IJWTAuthenticationService jwtAuthenticationService,
            IOptions<AppSettings> appSettings,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration config,
            IOptions<SMTPSettings> smtpSettings
            )
        {
            _jwtAuthenticationService = jwtAuthenticationService;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _smtpSettings = smtpSettings.Value;
            _config = config;
        }
        #endregion


        /// <summary>
        /// Contact us mail send
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("send")]
        public async Task<BaseApiResponse> ContactUs([FromBody] ContactUsMailModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            string emailBody;
            if (model != null)
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

                string BasePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.EmailTemplate);

                if (!Directory.Exists(BasePath))
                {
                    Directory.CreateDirectory(BasePath);
                }
                bool isSuccess = false;

                using (StreamReader reader = new StreamReader(Path.Combine(BasePath, Constants.ContactUs)))
                {
                    emailBody = reader.ReadToEnd();
                }
                var path = HttpContext.Request.Host.Value;
                emailBody = emailBody.Replace("##MailOf##", " Customer Contact Us Request ");
                emailBody = emailBody.Replace("##CustomerName##", (model.FirstName + ' '+ model.LastName));
                emailBody = emailBody.Replace("##CustomerNumber##", model.PhoneNumber);
                emailBody = emailBody.Replace("##CustomerMessage##", model.Message);
                emailBody = emailBody.Replace("##LogoURL##", Constants.https + path + _appSettings.EmailLogo);
                emailBody = emailBody.Replace("##envelopicon##", Constants.https + path + _appSettings.EnvelopIcon);
                emailBody = emailBody.Replace("##facebookicon##", Constants.https + path + _appSettings.FacebookIcon);
                emailBody = emailBody.Replace("##instagramicon##", Constants.https + path + _appSettings.InstagramIcon);
                emailBody = emailBody.Replace("##linkedinicon##", Constants.https + path + _appSettings.LinkedIn);
                emailBody = emailBody.Replace("##recruitmentbannerimg##", Constants.https + path + _appSettings.RecurimentBanner);
                isSuccess = await Task.Run(() => SendMailMessage(_appSettings.ContactUsMail, null, null, "Contact Us Request By Customer", emailBody, setting, null));
                if (isSuccess)
                {
                    response.Message = ErrorMessages.ContactUsPasswordSuccessEmail;
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
                response.Message = ErrorMessages.ContactUsEmailError;
                response.Success = false;
                Response.StatusCode = StatusCodes.Status404NotFound;
            }
            return response;
        }

    }
}
