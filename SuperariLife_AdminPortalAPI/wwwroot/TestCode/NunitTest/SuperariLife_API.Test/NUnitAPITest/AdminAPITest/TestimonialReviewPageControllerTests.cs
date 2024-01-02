using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using NUnit.Framework;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.SettingPages;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.TestimonialReviewPage;
using SuperariLifeAPI.Areas.Admin.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class TestimonialReviewPageControllerTests
    {
        private Mock<ITestimonialReviewPageService> _testimonialReviewPageServiceMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private TestimonialReviewPageController _testimonialReviewPageController;

        [SetUp]
        public void Setup()
        {
            _testimonialReviewPageServiceMock = new Mock<ITestimonialReviewPageService>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _testimonialReviewPageController = new TestimonialReviewPageController(
                _testimonialReviewPageServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object
            );
        }

        [Test]
        public async Task InsertUpdateTestimonialPagesReview_ReturnsSuccess()
        {
            // Arrange
            var model = new TestimonialPagesReviewReqModel();
            var expectedResult = new BaseApiResponse { Success = true, Message = ErrorMessages.UpdateTestimonialReviewSuccess };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues("Bearer YourTokenHere"));

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
                .Returns(new TokenModel { Id = 1 });

            _testimonialReviewPageServiceMock.Setup(x => x.InsertUpdateTestimonialPagesReview(It.IsAny<TestimonialPagesReviewReqModel>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _testimonialReviewPageController.InsertUpdateTestimonialPagesReview(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.AreEqual(expectedResult.Message, result.Message);
        }

        [Test]
        public async Task GetTestimonialPagesReviewList_ReturnsPageData()
        {
            // Arrange
            var expectedResult = new ApiResponse<TestimonialPagesReviewResponseModel>
            {
                Success = true,
                Data = new List<TestimonialPagesReviewResponseModel>()
            };

            _testimonialReviewPageServiceMock.Setup(x => x.GetTestimonialPagesReviewList(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<TestimonialPagesReviewResponseModel>());

            // Act
            var result = await _testimonialReviewPageController.GetTestimonialPagesReviewList(new CommonPaginationModel());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetTestimonialPagesReviewById_ReturnsTestimonialReviewData()
        {
            // Arrange
            var expectedResult = new ApiPostResponse<TestimonialPagesReviewResponseModel>
            {
                Success = true,
                Data = new TestimonialPagesReviewResponseModel()
            };

            _testimonialReviewPageServiceMock.Setup(x => x.GetTestimonialPagesReviewById(It.IsAny<int>()))
                .ReturnsAsync(new TestimonialPagesReviewResponseModel());

            // Act
            var result = await _testimonialReviewPageController.GetTestimonialPagesReviewById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task DeleteTestimonialPagesReview_ReturnsSuccess()
        {
            // Arrange
            var expectedResult = new BaseApiResponse { Success = true, Message = ErrorMessages.DeleteTestimonialReviewSuccess };

            _testimonialReviewPageServiceMock.Setup(x => x.DeleteTestimonialPagesReview(It.IsAny<int>()))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _testimonialReviewPageController.DeletePaymentType(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.AreEqual(expectedResult.Message, result.Message);
        }
    }
}
