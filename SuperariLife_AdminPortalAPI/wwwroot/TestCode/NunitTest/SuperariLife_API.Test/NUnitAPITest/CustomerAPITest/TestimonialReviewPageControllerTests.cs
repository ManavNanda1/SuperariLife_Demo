using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.TestimonialReviewPage;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.CustomerPortalAPITest
{
    [TestFixture]
    public class TestimonialReviewPageControllerTests
    {
        private Mock<ITestimonialReviewPageService> _testimonialReviewPageServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private TestimonialReviewPageController _testimonialReviewPageController;

        [SetUp]
        public void Setup()
        {
            _testimonialReviewPageServiceMock = new Mock<ITestimonialReviewPageService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();

            _testimonialReviewPageController = new TestimonialReviewPageController(
                _testimonialReviewPageServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object
            );
        }

        [Test]
        public async Task GetTestimonialPagesReviewForCustomer_ReturnsTestimonialPagesReview()
        {
            // Arrange
            var expectedResult = new ApiResponse<TestimonialPagesReviewResponseModel> { Success = true, Data = new List<TestimonialPagesReviewResponseModel>() };

            _testimonialReviewPageServiceMock.Setup(x => x.GetTestimonialPagesReviewForCustomer())
                .ReturnsAsync(new List<TestimonialPagesReviewResponseModel>());

            // Act
            var result = await _testimonialReviewPageController.GetTestimonialPagesReviewForCustomer();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }
    }
}
