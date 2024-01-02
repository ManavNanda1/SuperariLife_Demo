using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.SettingPages;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.TestimonialReviewPage;
using SuperariLifeAPI.Areas.Admin.Controllers;
using Xunit;

namespace SuperariLife.Tests.Controllers.Admin
{
    public class TestimonialReviewPageControllerTests
    {
        private readonly Mock<ITestimonialReviewPageService> _testimonialReviewPageServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly TestimonialReviewPageController _testimonialReviewPageController;

        public TestimonialReviewPageControllerTests()
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

        [Fact]
        public async Task InsertUpdateTestimonialPagesReview_ReturnsSuccess()
        {
            // Arrange
            var testimonialPagesReviewReqModel = new TestimonialPagesReviewReqModel
            {
            };
            var expectedResult = new BaseApiResponse { Success = true };
            var jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _testimonialReviewPageServiceMock.Setup(x => x.InsertUpdateTestimonialPagesReview(It.IsAny<TestimonialPagesReviewReqModel>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _testimonialReviewPageController.InsertUpdateTestimonialPagesReview(testimonialPagesReviewReqModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }

        [Fact]
        public async Task GetTestimonialPagesReviewList_ReturnsTestimonialPagesReviewList()
        {
            // Arrange
            var expectedResult = new ApiResponse<TestimonialPagesReviewResponseModel> { Success = true, Data = new List<TestimonialPagesReviewResponseModel>() };
            _testimonialReviewPageServiceMock.Setup(x => x.GetTestimonialPagesReviewList(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<TestimonialPagesReviewResponseModel>());

            // Act
            var result = await _testimonialReviewPageController.GetTestimonialPagesReviewList(new CommonPaginationModel());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task DeletePaymentType_ReturnsSuccess()
        {
            // Arrange
            var expectedResult = new BaseApiResponse { Success = true };
            _testimonialReviewPageServiceMock.Setup(x => x.DeleteTestimonialPagesReview(It.IsAny<int>()))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _testimonialReviewPageController.DeletePaymentType(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }
    }
}
