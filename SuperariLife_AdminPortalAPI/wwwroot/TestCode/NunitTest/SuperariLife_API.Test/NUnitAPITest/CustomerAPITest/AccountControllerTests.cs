//using System;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using Microsoft.Net.Http.Headers;
//using Moq;
//using NUnit.Framework;
//using SuperariLife.Common.Enum;
//using SuperariLife.Common.Helpers;
//using SuperariLife.Model.Login;
//using SuperariLife.Model.Settings;
//using SuperariLife.Model.Token;
//using SuperariLife.Service.Account;
//using SuperariLife.Service.JWTAuthentication;
//using SuperariLifeAPI.Areas.CustomerPortal.Controllers;

//namespace SuperariLife_API.Test.NUnitAPITest.CustomerAPITest
//{
  
//        [TestFixture]
//        public class AccountControllerTests
//        {
//            private Mock<IAccountService> _accountServiceMock;
//            private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
//            private Mock<IOptions<AppSettings>> _appSettingsMock;
//            private Mock<IHttpContextAccessor> _httpContextAccessorMock;
//            private Mock<IConfiguration> _configurationMock;

//            private AccountController _accountController;

//            [SetUp]
//            public void Setup()
//            {
//                _accountServiceMock = new Mock<IAccountService>();
//                _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
//                _appSettingsMock = new Mock<IOptions<AppSettings>>();
//                _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
//                _configurationMock = new Mock<IConfiguration>();

//                _accountController = new AccountController(
//                    _accountServiceMock.Object,
//                    _jwtAuthenticationServiceMock.Object,
//                    _appSettingsMock.Object,
//                    _httpContextAccessorMock.Object,
//                    _configurationMock.Object,
//                    null
//                );
//            }

//            [Test]
//            public async Task LoginCustomer_ValidCredentials_ReturnsToken()
//            {
//                // Arrange
//                var loginRequestModel = new LoginRequestModel
//                {
//                    Email = "test@example.com",
//                    Password = "password"
//                };

//                var customerLoginResponse = new CustomerLoginResponseModel
//                {
//                    // Set properties as needed for the test
//                };

//                _accountServiceMock.Setup(x => x.GetCustomerSaltByEmail(loginRequestModel.Email))
//                    .ReturnsAsync(new SaltResponseModel { /* Set properties as needed for the test */ });

//                _accountServiceMock.Setup(x => x.CustomerLogin(loginRequestModel))
//                    .ReturnsAsync(customerLoginResponse);

//                // Add more setups as needed...

//                // Act
//                var result = await _accountController.LoginCustomer(loginRequestModel);

//                // Assert
//                Assert.IsNotNull(result);
//                Assert.IsTrue(result.Success);
//                Assert.IsNotNull(result.Data);
//                Assert.AreEqual(customerLoginResponse.Token, result.Data.Token);
//                // Add more assertions...

//                // Verify other method calls as needed...
//            }

//            [Test]
//            public async Task Logout_ValidToken_ReturnsSuccess()
//            {
//                // Arrange
//                var tokenModel = new TokenModel
//                {
//                    Id = 123,
//                    EmailId = "test@example.com",
//                    // Set other properties as needed for the test
//                };

//                _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
//                    .Returns($"{JwtBearerDefaults.AuthenticationScheme} test-token");

//                _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData("test-token"))
//                    .Returns(tokenModel);

//                _accountServiceMock.Setup(x => x.LogoutCustomer(tokenModel.Id))
//                    .ReturnsAsync(1); // Assuming 1 is returned for a successful logout

//                // Act
//                var result = await _accountController.Logout();

//                // Assert
//                Assert.IsNotNull(result);
//                Assert.IsTrue(result.Success);
//                Assert.AreEqual(ErrorMessages.CustomerLogoutSuccess, result.Message);

//                // Verify other method calls as needed...
//            }
//        //[Test]
//        //public async Task ForgetPasswordWithURL_ValidEmail_ReturnsSuccess()
//        //{
//        //    // Arrange
//        //    var forgetPasswordRequestModel = new ForgetPasswordRequestModel
//        //    {
//        //        EmailId = "test@example.com"
//        //    };

//        //    _accountServiceMock.Setup(x => x.ForgetPasswordForCustomer(forgetPasswordRequestModel.EmailId))
//        //        .ReturnsAsync(new CustomerInfo { /* Set properties as needed for the test */ });

//        //    // Add more setups as needed...

//        //    // Act
//        //    var result = await _accountController.ForgetPasswordWithURL(forgetPasswordRequestModel);

//        //    // Assert
//        //    Assert.IsNotNull(result);
//        //    Assert.IsTrue(result.Success);
//        //    Assert.AreEqual(ErrorMessages.ForgetPasswordSuccessEmail, result.Message);

//        //    // Verify other method calls as needed...
//        //}

//        [Test]
//        public async Task SuccessForgetPasswordChangeURL_ValidModel_ReturnsSuccess()
//        {
//            // Arrange
//            var passwordChangeRequestModel = new PasswordChangeRequestModel
//            {
//                EncryptedCustomerId = "encrypted-id",
//                Password = "new-password"
//            };

//            _accountServiceMock.Setup(x => x.SuccessForgetPasswordChangeURLForCustomer(passwordChangeRequestModel))
//                .ReturnsAsync(Status.Success);

//            // Add more setups as needed...

//            // Act
//            var result = await _accountController.SuccessForgetPasswordChangeURL(passwordChangeRequestModel);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.IsTrue(result.Success);
//            Assert.AreEqual(ErrorMessages.ResetPasswordSuccess, result.Message);

//            // Verify other method calls as needed...
//        }

//        [Test]
//        public async Task UpdatePasswordByCustomer_ValidModel_ReturnsSuccess()
//        {
//            // Arrange
//            var changePasswordRequestModel = new ChangePasswordRequestModel
//            {
//                OldPassword = "old-password",
//                CreatePassword = "new-password",
//                ConfirmPassword = "new-password"
//            };

//            var tokenModel = new TokenModel
//            {
//                Id = 123,
//                EmailId = "test@example.com",
//                // Set other properties as needed for the test
//            };

//            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
//                .Returns($"{JwtBearerDefaults.AuthenticationScheme} test-token");

//            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData("test-token"))
//                .Returns(tokenModel);

//            _accountServiceMock.Setup(x => x.GetCustomerSaltByEmail(tokenModel.EmailId))
//                .ReturnsAsync(new SaltResponseModel { /* Set properties as needed for the test */ });

//            _accountServiceMock.Setup(x => x.ChangePasswordForCustomer(tokenModel.Id, It.IsAny<string>(), It.IsAny<string>()))
//                .ReturnsAsync(string.Empty);

//            // Add more setups as needed...

//            // Act
//            var result = await _accountController.UpdatePasswordByCustomer(changePasswordRequestModel);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.IsTrue(result.Success);
//            Assert.AreEqual(ErrorMessages.ResetPasswordSuccess, result.Message);

//            // Verify other method calls as needed...
//        }

//    }

//}
