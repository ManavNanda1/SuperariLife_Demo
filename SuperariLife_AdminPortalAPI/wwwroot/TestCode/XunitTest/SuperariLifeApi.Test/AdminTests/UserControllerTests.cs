using System.Collections.Generic;
using System.Threading.Tasks;
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
using SuperariLifeAPI.Areas.Admin.Controllers;
using Xunit;

namespace SuperariLife_API.Test.xUnitAPITest.AdminAPITest
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment> _hostingEnvironmentMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IOptions<SMTPSettings>> _smtpSettingsMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _hostingEnvironmentMock = new Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
            _configurationMock = new Mock<IConfiguration>();
            var smtpSettingsOptions = Options.Create(new SMTPSettings
            {
                // SMTP settings initialization
            });

            var appSettingsOptions = Options.Create(new AppSettings
            {
                // App settings initialization
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

        [Fact]
        public async Task InsertUpdateUserByAdmin_ReturnsSuccess()
        {
            // Arrange
            var userReqModelForAdmin = new UserReqModelForAdmin
            {
                Email = "test@example.com"
            };
            var expectedResult = new ApiPostResponse<UserInsertUpdateResponseModel> { Success = true };
            var jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _userServiceMock.Setup(x => x.InsertUpdateUserByAdmin(It.IsAny<UserReqModelForAdmin>()))
                .ReturnsAsync(new UserInsertUpdateResponseModel { StatusOfInsertUpdate = StatusResult.Updated });

            // Act
            var result = await _userController.InsertUpdateUserByAdmin(userReqModelForAdmin);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }

        [Fact]
        public async Task GetUserListByAdmin_ReturnsUserList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<ResponseUserModel> { Success = true, Data = new List<ResponseUserModel>() };

            // Additional setup for ControllerContext
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _userServiceMock.Setup(x => x.GetUserListByAdmin(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<ResponseUserModel>());

            // Act
            var result = await _userController.GetUserListByAdmin(commonPaginationModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetUserById_ReturnsUserInfo()
        {
            // Arrange
            const int userId = 1;
            var expectedResult = new ApiPostResponse<ResponseUserModel> { Success = true, Data = new ResponseUserModel() };

            // Additional setup for ControllerContext
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg";

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _userServiceMock.Setup(x => x.GetUserById(userId))
                .ReturnsAsync(new ResponseUserModel());

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
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
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }

        [Fact]
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
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }
    }
}
