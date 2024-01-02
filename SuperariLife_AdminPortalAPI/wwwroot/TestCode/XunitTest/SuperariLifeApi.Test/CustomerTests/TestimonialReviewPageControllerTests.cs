using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.TestimonialReviewPage;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;
using Xunit;

namespace SuperariLife_API.Test.xUnitAPITest.CustomerPortalAPITest
{
    public class TestimonialReviewPageControllerTests
    {
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<ITestimonialReviewPageService> _testimonialReviewPageServiceMock;
        private readonly TestimonialReviewPageController _testimonialReviewPageController;

        public TestimonialReviewPageControllerTests()
        {
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _testimonialReviewPageServiceMock = new Mock<ITestimonialReviewPageService>();
            _testimonialReviewPageController = new TestimonialReviewPageController(
                _testimonialReviewPageServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object
            );
        }

        [Fact]
        public async Task GetTestimonialPagesReviewForCustomer_ReturnsTestimonialPagesReview()
        {
            // Arrange
            var expectedResult = new ApiResponse<TestimonialPagesReviewResponseModel> { Success = true, Data = new List<TestimonialPagesReviewResponseModel>() };
            _testimonialReviewPageServiceMock.Setup(x => x.GetTestimonialPagesReviewForCustomer())
                .ReturnsAsync(new List<TestimonialPagesReviewResponseModel>());

            // Act
            var result = await _testimonialReviewPageController.GetTestimonialPagesReviewForCustomer();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }
    }
}
