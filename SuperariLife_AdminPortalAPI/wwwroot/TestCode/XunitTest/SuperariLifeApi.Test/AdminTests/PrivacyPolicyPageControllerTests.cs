using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.PrivacyPolicyPage;
using SuperariLifeAPI.Areas.Admin.Controllers;
using Xunit;

namespace SuperariLife.Tests.Controllers.Admin
{
    public class PrivacyPolicyPageControllerTests
    {
        private readonly Mock<IPrivacyPolicyPageService> _privacyPolicyPageServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly PrivacyPolicyPageController _privacyPolicyPageController;

        public PrivacyPolicyPageControllerTests()
        {
            _privacyPolicyPageServiceMock = new Mock<IPrivacyPolicyPageService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();

            _privacyPolicyPageController = new PrivacyPolicyPageController(
                _privacyPolicyPageServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object
            );
        }

        [Fact]
        public async Task InsertUpdatePrivacyPage_ReturnsSuccess()
        {
            // Arrange
            var privacyPageReqModel = new PrivacyPageReqModel
            {
            };
            var expectedResult = new BaseApiResponse { Success = true };
            var jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _privacyPolicyPageServiceMock.Setup(x => x.InsertUpdatePrivacyPage(It.IsAny<PrivacyPageReqModel>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _privacyPolicyPageController.InsertUpdatePrivacyPage(privacyPageReqModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }

        [Fact]
        public async Task GetPrivacyPage_ReturnsPrivacyPageList()
        {
            // Arrange
            var expectedResult = new ApiResponse<PrivacyPageResponseModel> { Success = true, Data = new List<PrivacyPageResponseModel>() };
            _privacyPolicyPageServiceMock.Setup(x => x.GetPrivacyPage())
                .ReturnsAsync(new List<PrivacyPageResponseModel>());

            // Act
            var result = await _privacyPolicyPageController.GetPrivacyPage();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }
    }
}
