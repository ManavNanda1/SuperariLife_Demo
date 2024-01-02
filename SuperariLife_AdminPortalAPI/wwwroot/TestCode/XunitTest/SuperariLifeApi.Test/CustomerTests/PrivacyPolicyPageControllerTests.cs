using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.PrivacyPolicyPage;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;
using Xunit;

namespace SuperariLife_API.Test.xUnitAPITest.CustomerPortalAPITest
{
    public class PrivacyPolicyPageControllerTests
    {
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IPrivacyPolicyPageService> _privacyPolicyPageServiceMock;
        private readonly PrivacyPolicyPageController _privacyPolicyPageController;

        public PrivacyPolicyPageControllerTests()
        {
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _privacyPolicyPageServiceMock = new Mock<IPrivacyPolicyPageService>();
            _privacyPolicyPageController = new PrivacyPolicyPageController(
                _privacyPolicyPageServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object
            );
        }

        [Fact]
        public async Task GetPrivacyPage_ReturnsPrivacyPage()
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
