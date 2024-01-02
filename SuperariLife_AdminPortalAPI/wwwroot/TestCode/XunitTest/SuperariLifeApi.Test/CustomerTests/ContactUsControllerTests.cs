using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using SuperariLife.Common.EmailNotification;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.ContactUs;
using SuperariLife.Model.Settings;
using SuperariLife.Service.Account;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;
using Xunit;

namespace SuperariLife_API.Test.xUnitAPITest.CustomerPortalAPITest
{
    public class ContactUsControllerTests
    {
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IOptions<SMTPSettings>> _smtpSettingsMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
        private readonly ContactUsController _contactUsController;

        public ContactUsControllerTests()
        {
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _accountServiceMock = new Mock<IAccountService>();
            _configurationMock = new Mock<IConfiguration>();
            _smtpSettingsMock = new Mock<IOptions<SMTPSettings>>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _contactUsController = new ContactUsController(
                _accountServiceMock.Object,
                _jwtAuthenticationServiceMock.Object,
                _appSettingsMock.Object,
                _httpContextAccessorMock.Object,
                _configurationMock.Object,
                _smtpSettingsMock.Object
            );
        }

        [Fact]
        public async Task ContactUs_ReturnsSuccess()
        {
            // Arrange
            var contactUsMailModel = new ContactUsMailModel();
            var expectedResult = new BaseApiResponse { Success = true };
            var smtpSettingsOptions = Options.Create(new SMTPSettings
            {
                EmailAppPassword = "pwdrcfpmcoykksgh",
                EmailEnableSsl = "true",
                EmailHostName = "smtp.gmail.com",
                EmailPort = "587",
                EmailUsername = "project.shaligraminfotech@gmail.com",
                FromEmail = "project.shaligraminfotech@gmail.com",
                FromName = "Superari Life"
            });

            var appSettingsOptions = Options.Create(new AppSettings
            {
                JWT_Secret = "4226452948404D635166546A576E5A7234743777217A25432A462D4A614E645267556B58703273357638792F413F4428472B4B6250655368566D597133743677",
                JWT_Validity_Mins = 4340,
                ErrorSendToEmail = "nilesh.y@shaligraminfotech.com",
                ForgotPasswordAttemptValidityHours = 1,
                PasswordLinkValidityMins = 2,
                EmailLogo = "/Logo/EmailLogo.png",
                EnvelopIcon = "/Logo/envelop-icon.png",
                FacebookIcon = "/Logo/facebook-icon.png",
                InstagramIcon = "/Logo/instagram-icon.png",
                LinkedIn = "/Logo/linkedin-icon.png",
                RecurimentBanner = "/Logo/superari-banner-img.png",
                EnvelopURL = "superarilife@gmail.com.com",
                FacebookURL = "https://www.facebook.com/Rakhee.Vithlani",
                InstagramURL = "https://www.instagram.com/yogi_rakhee/",
                LinkedInURL = "https://uk.linkedin.com/company/superari-ltd?trk=public_profile_topcard-current-company",
                ContactUsMail = "nilesh.y@shaligraminfotech.com"
            });


            //_smtpSettingsMock.Setup(x => x.Value).Returns(smtpSettings);
            //_appSettingsMock.Setup(x => x.Value).Returns(appSettings);

            //_accountServiceMock.Setup(x => x.SendMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<EmailNotification.EmailNotification.EmailSetting>(), It.IsAny<string>()))
            //    .ReturnsAsync(true);

            // Act
            var result = await _contactUsController.ContactUs(contactUsMailModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }
    }
}
