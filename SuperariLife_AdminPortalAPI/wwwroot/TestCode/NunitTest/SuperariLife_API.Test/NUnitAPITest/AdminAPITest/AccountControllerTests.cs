using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SuperariLife.Common.EmailNotification;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Login;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Account;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Controllers;

namespace SuperariLifeAPI.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<IAccountService> _accountServiceMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private Mock<IOptions<AppSettings>> _appSettingsMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IOptions<SMTPSettings>> _smtpSettingsMock;
        private AccountController _accountController;

        [SetUp]
        public void Setup()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _configurationMock = new Mock<IConfiguration>();
            _smtpSettingsMock = new Mock<IOptions<SMTPSettings>>();

            _accountController = new AccountController(
                _accountServiceMock.Object,
                _jwtAuthenticationServiceMock.Object,
                _appSettingsMock.Object,
                _httpContextAccessorMock.Object,
                _configurationMock.Object,
                _smtpSettingsMock.Object
            );
        }

        //[Test]
        //public async Task LoginUser_ValidCredentials_ReturnsSuccess()
        //{
        //    // Arrange
        //    var loginRequestModel = new LoginRequestModel
        //    {
        //        Email = "test@example.com",
        //        Password = "password123"
        //    };

        //    _accountServiceMock.Setup(x => x.GetUserSalt(loginRequestModel.Email))
        //        .ReturnsAsync(new SaltResponseModel { Status = null, Password = "hashedPassword", PasswordSalt = "salt" });

        //    EncryptionDecryption.Verify(loginRequestModel.Password, "hashedPassword", "salt");
              

        //    _accountServiceMock.Setup(x => x.LoginUser(It.IsAny<LoginRequestModel>()))
        //        .ReturnsAsync(new LoginResponseModel { UserId = 1, Token = "fakeToken" });

        //    // Act
        //    var result = await _accountController.LoginUser(loginRequestModel);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual("fakeToken", result.Data.Token);
        //}

        //[Test]
        //public async Task LoginUser_InvalidCredentials_ReturnsFailure()
        //{
        //    // Arrange
        //    var loginRequestModel = new LoginRequestModel
        //    {
        //        Email = "test@example.com",
        //        Password = "wrongPassword"
        //    };

        //    _accountServiceMock.Setup(x => x.GetUserSalt(loginRequestModel.Email))
        //        .ReturnsAsync(new SaltResponseModel { Status = LoginStatus.EmailNotExist, Password = "hashedPassword", PasswordSalt = "salt" });

        //    EncryptionDecryption.Verify(loginRequestModel.Password, "hashedPassword", "salt");
                

        //    // Act
        //    var result = await _accountController.LoginUser(loginRequestModel);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.IsFalse(result.Success);
        //    // Add more assertions based on the expected response for invalid credentials
        //}

        [Test]
        public async Task LoginUser_UserDeactivated_ReturnsFailure()
        {
            // Arrange
            var loginRequestModel = new LoginRequestModel
            {
                Email = "deactivated@example.com",
                Password = "password123"
            };

            _accountServiceMock.Setup(x => x.GetUserSalt(loginRequestModel.Email))
                .ReturnsAsync(new SaltResponseModel { Status = LoginStatus.UserDeactive });

            // Act
            var result = await _accountController.LoginUser(loginRequestModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            // Add more assertions based on the expected response for a deactivated user
        }

        // Add more test cases for different scenarios (user deleted, email not exist, etc.)

        [Test]
        public async Task Logout_ValidToken_ReturnsSuccess()
        {
            // Arrange
            var expectedResult = new BaseApiResponse { Success = true };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers["Authorization"])
                .Returns("Bearer validToken");

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData("validToken"))
                .Returns(new TokenModel { Id = 1 });

            _accountServiceMock.Setup(x => x.LogoutUser(1))
                .ReturnsAsync(1);

            // Act
            var result = await _accountController.Logout();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            // Add more assertions based on the expected response
        }

    }
}
