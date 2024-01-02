using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.User;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Model.User;
using SuperariLife.Service.JWTAuthentication;
using UserController = SuperariLifeAPI.Areas.Admin.Controllers.UserController;

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment> _hostingEnvironmentMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IOptions<SMTPSettings>> _smtpSettingsMock;
        private Mock<IOptions<AppSettings>> _appSettingsMock;
        private UserController _userController;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _hostingEnvironmentMock = new Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
            _configurationMock = new Mock<IConfiguration>();
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
            _userController = new UserController(
                _userServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object,
                _hostingEnvironmentMock.Object,
                smtpSettingsOptions,
                _configurationMock.Object,
                appSettingsOptions
            );
        }

        [Test]
        public async Task InsertUpdateUserByAdmin_ReturnsSuccess()
        {
            // Arrange
            var userReqModelForAdmin = new UserReqModelForAdmin
            {
                Email = "test@example.com"
            };
            var expectedResult = new ApiPostResponse<UserInsertUpdateResponseModel> { Success = true };
            var jwtToken = "your_jwt_token_here";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _userServiceMock.Setup(x => x.InsertUpdateUserByAdmin(It.IsAny<UserReqModelForAdmin>()))
                .ReturnsAsync(new UserInsertUpdateResponseModel { StatusOfInsertUpdate = StatusResult.Updated });

            // Act
            var result = await _userController.InsertUpdateUserByAdmin(userReqModelForAdmin);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task GetUserListByAdmin_ReturnsUserList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<ResponseUserModel> { Success = true, Data = new List<ResponseUserModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var jwtToken = "your_jwt_token_here";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _userServiceMock.Setup(x => x.GetUserListByAdmin(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<ResponseUserModel>());

            // Act
            var result = await _userController.GetUserListByAdmin(commonPaginationModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetUserById_ReturnsUserInfo()
        {
            // Arrange
            const int userId = 1;
            var expectedResult = new ApiPostResponse<ResponseUserModel> { Success = true, Data = new ResponseUserModel() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var jwtToken = "your_jwt_token_here";

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _userServiceMock.Setup(x => x.GetUserById(userId))
                .ReturnsAsync(new ResponseUserModel());

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task DeleteUserByAdmin_ReturnsSuccess()
        {
            // Arrange
            const int userId = 1;
            var expectedResult = new ApiPostResponse<UserDeleteResponseModel> { Success = true, Data = new UserDeleteResponseModel() };

            _userServiceMock.Setup(x => x.DeleteUserByAdmin(userId))
                .ReturnsAsync(new UserDeleteResponseModel { StatusOfDelete = Status.Success });

            // Act
            var result = await _userController.DeleteUserByAdmin(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task ActiveDeactiveUserByAdmin_ReturnsSuccess()
        {
            // Arrange
            const int userId = 1;
            var expectedResult = new ApiPostResponse<ResponseUserModel> { Success = true, Data = new ResponseUserModel() };

            _userServiceMock.Setup(x => x.ActiveDeactiveUserByAdmin(userId))
                .ReturnsAsync(new ResponseUserModel { IsActive = ActiveStatus.Inactive });

            // Act
            var result = await _userController.ActiveDeactiveUserByAdmin(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }
    }
}
