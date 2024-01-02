using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.TermsAndConditionPage;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;
using Xunit;

namespace SuperariLife_API.Test.xUnitAPITest.CustomerPortalAPITest
{
    public class TermsAndConditionPageControllerTests
    {
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<ITermsAndConditionPageService> _termsAndConditionPageServiceMock;
        private readonly TermsAndConditionPageController _termsAndConditionPageController;

        public TermsAndConditionPageControllerTests()
        {
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _termsAndConditionPageServiceMock = new Mock<ITermsAndConditionPageService>();
            _termsAndConditionPageController = new TermsAndConditionPageController(
                _termsAndConditionPageServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object
            );
        }

        [Fact]
        public async Task GetTermsAndConditionPage_ReturnsTermsAndConditionPage()
        {
            // Arrange
            var expectedResult = new ApiResponse<TermsAndConditionPageResponseModel> { Success = true, Data = new List<TermsAndConditionPageResponseModel>() };
            _termsAndConditionPageServiceMock.Setup(x => x.GetTermsAndConditionPage())
                .ReturnsAsync(new List<TermsAndConditionPageResponseModel>());

            // Act
            var result = await _termsAndConditionPageController.GetTermsAndConditionPage();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }
    }
}
