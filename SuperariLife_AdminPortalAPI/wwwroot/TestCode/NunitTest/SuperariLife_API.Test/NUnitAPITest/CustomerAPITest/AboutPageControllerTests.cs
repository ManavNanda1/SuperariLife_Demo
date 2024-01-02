using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Model.SettingPages;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.AboutPage;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;

public class AboutPageControllerTests
{
    private AboutPageController _aboutPageController;
    private Mock<IAboutPageService> _aboutPageServiceMock;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
    private Mock<IHostingEnvironment> _hostingEnvironmentMock;
    private Mock<IConfiguration> _configMock;

    [SetUp]
    public void Setup()
    {
        _aboutPageServiceMock = new Mock<IAboutPageService>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
        _hostingEnvironmentMock = new Mock<IHostingEnvironment>();
        _configMock = new Mock<IConfiguration>();

        _aboutPageController = new AboutPageController(
            _aboutPageServiceMock.Object,
            _httpContextAccessorMock.Object,
            _jwtAuthenticationServiceMock.Object,
            _hostingEnvironmentMock.Object,
            _configMock.Object
        );
    }

    [Test]
    public async Task GetAboutPageSectionByCustomer_ValidData_ReturnsSuccessResponseWithData()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost:7061");

        httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
        _aboutPageController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        var aboutPageSectionResponseModelList = new List<AboutPageSectionResponseModel>
        {
              new AboutPageSectionResponseModel
    {
        AboutPageSectionId = 1,
        AboutPageSectionLayoutTypeId = 1,
        AboutPageSectionLayoutTypeOptionName = "Option 1",
        AboutPageSectionIsSetupFreeButtonEnable = true,
        AboutPageSectionContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
        AboutPageSectionContentTitle = "Section 1",
        AboutPageSectionImage = "image1.jpg",
        IsActive = true,
        RowNumber = 1,
        TotalRecords = 2
    },
    new AboutPageSectionResponseModel
    {
        AboutPageSectionId = 2,
        AboutPageSectionLayoutTypeId = 2,
        AboutPageSectionLayoutTypeOptionName = "Option 2",
        AboutPageSectionIsSetupFreeButtonEnable = false,
        AboutPageSectionContent = "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
        AboutPageSectionContentTitle = "Section 2",
        AboutPageSectionImage = "image2.jpg",
        IsActive = true,
        RowNumber = 2,
        TotalRecords = 2
    }
        };

        _aboutPageServiceMock.Setup(x => x.GetAboutPageSectionByCustomer()).ReturnsAsync(aboutPageSectionResponseModelList);

        // Act
        var result = await _aboutPageController.GetAboutPageSectionByCustomer();

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(aboutPageSectionResponseModelList, result.Data);
    }

    [Test]
    public async Task GetAboutPageImageList_ValidData_ReturnsSuccessResponseWithData()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost:7061");

        httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
        _aboutPageController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        var aboutImageResponseModelList = new List<AboutImageResponseModel>
        {
            new AboutImageResponseModel
            {
             AboutPageImageId =1,
             AboutPageImages ="nil.jpg",
             RowNumber =1,
             TotalRecord =2,
            },
            new AboutImageResponseModel
            {
             AboutPageImageId =2,
             AboutPageImages ="nil.jpg",
             RowNumber =2,
             TotalRecord =2,
            }
        };

        _aboutPageServiceMock.Setup(x => x.GetAboutPageImageList()).ReturnsAsync(aboutImageResponseModelList);

        // Act
        var result = await _aboutPageController.GetAboutPageImageList();

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(aboutImageResponseModelList, result.Data);
    }
}
