using Microsoft.AspNetCore.Http;
using Moq;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.PrivacyPolicyPage;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.CustomerPortalAPITest
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
        public async Task GetPrivacyPage_ReturnsPrivacyPage()
        {
            // Arrange
            var expectedResult = new ApiResponse<PrivacyPageResponseModel> { Success = true, Data = new List<PrivacyPageResponseModel>() };

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
