using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.TermsAndConditionPage;
using SuperariLifeAPI.Areas.Admin.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class TermsAndConditionPageControllerTests
    {
        private Mock<ITermsAndConditionPageService> _termsAndConditionPageServiceMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private TermsAndConditionPageController _termsAndConditionPageController;

        [SetUp]
        public void Setup()
        {
            _termsAndConditionPageServiceMock = new Mock<ITermsAndConditionPageService>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _termsAndConditionPageController = new TermsAndConditionPageController(
                _termsAndConditionPageServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object
            );
        }

        [Test]
        public async Task InsertUpdateTermsAndConditionPage_ReturnsSuccess()
        {
            // Arrange
            var model = new TermsAndConditionPageReqModel();
            var expectedResult = new BaseApiResponse { Success = true, Message = ErrorMessages.UpdateTermsAndConditionPageSuccess };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues("Bearer YourTokenHere"));

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
                .Returns(new TokenModel { Id = 1 });

            _termsAndConditionPageServiceMock.Setup(x => x.InsertUpdateTermsAndConditionPage(It.IsAny<TermsAndConditionPageReqModel>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _termsAndConditionPageController.InsertUpdateTermsAndConditionPage(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.AreEqual(expectedResult.Message, result.Message);
        }

        [Test]
        public async Task GetTermsAndConditionPage_ReturnsPageData()
        {
            // Arrange
            var expectedResult = new ApiResponse<TermsAndConditionPageResponseModel>
            {
                Success = true,
                Data = new List<TermsAndConditionPageResponseModel>()
            };

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
