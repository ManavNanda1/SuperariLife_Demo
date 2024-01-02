using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.PrivacyPolicyPage;
using SuperariLifeAPI.Areas.Admin.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class PrivacyPolicyPageControllerTests
    {
        private Mock<IPrivacyPolicyPageService> _privacyPolicyPageServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private PrivacyPolicyPageController _privacyPolicyPageController;

        [SetUp]
        public void Setup()
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

        [Test]
        public async Task InsertUpdatePrivacyPage_ReturnsSuccess()
        {
            // Arrange
            var privacyPageReqModel = new PrivacyPageReqModel();
            var expectedResult = new BaseApiResponse { Success = true };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns("Bearer YourTokenHere");

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
                .Returns(new TokenModel { Id = 1 });

            _privacyPolicyPageServiceMock.Setup(x => x.InsertUpdatePrivacyPage(It.IsAny<PrivacyPageReqModel>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _privacyPolicyPageController.InsertUpdatePrivacyPage(privacyPageReqModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task GetPrivacyPage_ReturnsPrivacyPageData()
        {
            // Arrange
            var expectedResult = new ApiResponse<PrivacyPageResponseModel> { Success = true, Data = new List<PrivacyPageResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _privacyPolicyPageController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _privacyPolicyPageServiceMock.Setup(x => x.GetPrivacyPage())
                .ReturnsAsync(new List<PrivacyPageResponseModel>());

            // Act
            var result = await _privacyPolicyPageController.GetPrivacyPage();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }
    }
}
