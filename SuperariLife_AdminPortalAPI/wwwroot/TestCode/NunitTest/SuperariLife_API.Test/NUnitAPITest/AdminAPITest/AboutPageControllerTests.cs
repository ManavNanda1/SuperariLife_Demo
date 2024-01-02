using Microsoft.AspNetCore.Http;
using Moq;
using SuperariLifeAPI.Areas.Admin.Controllers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Service.SettingPage.AboutPage;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Service.JWTAuthentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Model.Token;

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class AboutPageControllerTests
    {
        private Mock<IAboutPageService> _aboutPageServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;

        private AboutPageController _aboutPageController;

        [SetUp]
        public void Setup()
        {
            _aboutPageServiceMock = new Mock<IAboutPageService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();

            _aboutPageController = new AboutPageController(
                _aboutPageServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object,
                Mock.Of<Microsoft.AspNetCore.Hosting.IHostingEnvironment>(),
                Mock.Of<Microsoft.Extensions.Configuration.IConfiguration>()
            );
        }

        [Test]
        public async Task GetAboutPageImageById_ReturnsSuccess()
        {

            // Arrange
            var aboutPageImageId = 1;
            var expectedResult = new ApiPostResponse<AboutImageResponseModel> { Success = true };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _aboutPageController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _aboutPageServiceMock.Setup(x => x.GetAboutPageImageById(aboutPageImageId))
                .ReturnsAsync(new AboutImageResponseModel());

            // Act
            var result = await _aboutPageController.GetAboutPageImageById(aboutPageImageId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        // Add similar tests for other actions...

        [Test]
        public async Task DeleteAboutPageImage_ReturnsSuccess()
        {
            // Arrange
            var aboutPageImageId = 1;
            var expectedResult = new ApiPostResponse<CommonAboutPageDeleteModel> { Success = true };

            _aboutPageServiceMock.Setup(x => x.DeleteAboutPageImage(aboutPageImageId))
                .ReturnsAsync(new CommonAboutPageDeleteModel { StatusOfDelete = Status.Success });

            // Act
            var result = await _aboutPageController.DeleteAboutPageImage(aboutPageImageId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }


        [Test]
        public async Task InsertUpdateAboutPageSection_ReturnsSuccess()
        {
            // Arrange
            var model = new AboutPageSectionReqModel();
            var expectedResult = new ApiPostResponse<AboutInsertUpdateResponseModel> { Success = true };

            // Mocking HttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

            _aboutPageController = new AboutPageController(
                _aboutPageServiceMock.Object,
                httpContextAccessorMock.Object,  // Use the mocked HttpContextAccessor
                _jwtAuthenticationServiceMock.Object,
                Mock.Of<Microsoft.AspNetCore.Hosting.IHostingEnvironment>(),
                Mock.Of<Microsoft.Extensions.Configuration.IConfiguration>()
            );

            httpContextAccessorMock.Object.HttpContext.Request.Scheme = "https";
            httpContextAccessorMock.Object.HttpContext.Request.Host = new HostString("example.com");
            httpContextAccessorMock.Object.HttpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
                .Returns(new TokenModel { Id = 1 }); // Adjust this according to your token setup

            _aboutPageServiceMock.Setup(x => x.InsertUpdateAboutPageSection(It.IsAny<AboutPageSectionReqModel>()))
                .ReturnsAsync(new AboutInsertUpdateResponseModel { StatusOfInsertUpdate = StatusResult.Updated });

            // Act
            var result = await _aboutPageController.InsertUpdateAboutPageSection(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }




    }
}
