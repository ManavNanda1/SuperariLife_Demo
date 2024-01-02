using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Moq;
using NUnit.Framework;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.ContactUs;
using SuperariLife.Model.Settings;
using SuperariLife.Service.Account;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;
using System.IO;
using System.Threading.Tasks;

namespace SuperariLifeAPI.Tests.CustomerPortal.Controllers
{
    [TestFixture]
    public class ContactUsControllerTests
    {
        private ContactUsController _contactUsController;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private Mock<IAccountService> _accountServiceMock;
        private readonly IConfiguration _config;

        [SetUp]
        public void Setup()
        {
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _accountServiceMock = new Mock<IAccountService>();

            var appSettingsOptions = Options.Create(new AppSettings
            {
                EmailLogo = "/Logo/EmailLogo.png",
                EnvelopIcon = "/Logo/envelop-icon.png",
                FacebookIcon = "/Logo/facebook-icon.png",
                InstagramIcon = "/Logo/instagram-icon.png",
                LinkedIn = "/Logo/linkedin-icon.png",
                RecurimentBanner = "/Logo/superari-banner-img.png",
                ContactUsMail = "contactus@example.com"
            });

            var smtpSettingsOptions = Options.Create(new SMTPSettings
            {
                EmailEnableSsl = "true",
                EmailHostName = "smtp.example.com",
                EmailPassword = "password",
                EmailAppPassword = "appPassword",
                EmailPort = "587",
                FromEmail = "from@example.com",
                FromName = "Superari Life",
                EmailUsername = "username"
            });

            _contactUsController = new ContactUsController(
                _accountServiceMock.Object,
                _jwtAuthenticationServiceMock.Object,
                appSettingsOptions,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IConfiguration>(),
                smtpSettingsOptions
            );
        }

        //[Test]
        //public async Task ContactUs_ValidModel_ReturnsSuccessResponse()
        //{
        //    // Arrange
        //    var model = new ContactUsMailModel
        //    {
        //        FirstName = "John",
        //        LastName = "Doe",
        //        PhoneNumber = "1234567890",
        //        Message = "Test message",
        //        Email = "test@gmail.com"
        //    };

        //    var expectedResult = new BaseApiResponse { Success = true };

        //    var httpContext = new DefaultHttpContext();
        //    httpContext.Request.Scheme = "https";
        //    httpContext.Request.Host = new HostString("localhost:7061");

        //    httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
        //    _contactUsController.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = httpContext
        //    };

        //    // Act
        //    var result = await _contactUsController.ContactUs(model);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(expectedResult.Success, result.Success);
        //}

        [Test]
        public async Task ContactUs_NullModel_ReturnsErrorResponse()
        {
            // Arrange
            ContactUsMailModel model = null;
            var expectedResult = new BaseApiResponse { Success = false, Message = ErrorMessages.ContactUsEmailError };

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _contactUsController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _contactUsController.ContactUs(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.AreEqual(expectedResult.Message, result.Message);
            Assert.AreEqual(StatusCodes.Status404NotFound, _contactUsController.Response.StatusCode);
        }
    }
}
