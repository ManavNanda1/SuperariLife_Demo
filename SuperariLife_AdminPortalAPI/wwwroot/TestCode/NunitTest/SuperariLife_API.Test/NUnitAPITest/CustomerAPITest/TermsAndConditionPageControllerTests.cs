using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.TermsAndConditionPage;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.CustomerPortalAPITest
{
    [TestFixture]
    public class TermsAndConditionPageControllerTests
    {
        private Mock<ITermsAndConditionPageService> _termsAndConditionPageServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private TermsAndConditionPageController _termsAndConditionPageController;

        [SetUp]
        public void Setup()
        {
            _termsAndConditionPageServiceMock = new Mock<ITermsAndConditionPageService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();

            _termsAndConditionPageController = new TermsAndConditionPageController(
                _termsAndConditionPageServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object
            );
        }

        [Test]
        public async Task GetTermsAndConditionPage_ReturnsTermsAndConditionPage()
        {
            // Arrange
            var expectedResult = new ApiResponse<TermsAndConditionPageResponseModel> { Success = true, Data = new List<TermsAndConditionPageResponseModel>() };

            _termsAndConditionPageServiceMock.Setup(x => x.GetTermsAndConditionPage())
                .ReturnsAsync(new List<TermsAndConditionPageResponseModel>());

            // Act
            var result = await _termsAndConditionPageController.GetTermsAndConditionPage();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }
    }
}
