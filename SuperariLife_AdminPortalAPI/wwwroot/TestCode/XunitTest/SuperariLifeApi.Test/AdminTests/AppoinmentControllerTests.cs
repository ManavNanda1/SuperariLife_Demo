using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLifeAPI.Areas.Admin.Controllers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Appointment;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Appointment;
using SuperariLife.Service.JWTAuthentication;
using Xunit;
using Microsoft.Extensions.Options;
using System.Net.Http;

public class AppointmentControllerTests
{
    private readonly Mock<IAppointmentService> _appointmentServiceMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;

    private AppointmentController _appointmentController;

    public AppointmentControllerTests()
    {
        _appointmentServiceMock = new Mock<IAppointmentService>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();

        _appointmentController = new AppointmentController(
            _appointmentServiceMock.Object,
            _httpContextAccessorMock.Object,
            _jwtAuthenticationServiceMock.Object,
            Mock.Of<Microsoft.AspNetCore.Hosting.IHostingEnvironment>(),
            Mock.Of<Microsoft.Extensions.Configuration.IConfiguration>(),
            Options.Create(new SMTPSettings()),
            Options.Create(new AppSettings())
        );
    }

    [Fact]
    public async Task InsertUpdateAppointmentByAdmin_ValidModel_ReturnsSuccessResponse()
    {
        // Arrange
        var model = new AppointmentReqModelByAdmin {  };
        var expectedResult = new BaseApiResponse { Success = true };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
        _httpContextAccessorMock.Object.HttpContext.Request.Scheme = "https";
        _httpContextAccessorMock.Object.HttpContext.Request.Host = new HostString("example.com");
        _httpContextAccessorMock.Object.HttpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg"; 

        _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
            .Returns(new TokenModel { Id = 1 }); 

        _appointmentServiceMock.Setup(x => x.InsertUpdateAppointmentByAdmin(It.IsAny<AppointmentReqModelByAdmin>()))
            .ReturnsAsync(StatusResult.Updated);

        // Act
        var result = await _appointmentController.InsertUpdateAppointmentByAdmin(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Success, result.Success);
    }

    // Add similar tests for other actions...

    [Fact]
    public async Task GetAppointmentTypeForDropDown_ReturnsValidResponse()
    {
        // Arrange
        var expectedResult = new ApiResponse<AppointmentResponseDropDownModel> { Success = true };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
        _httpContextAccessorMock.Object.HttpContext.Request.Scheme = "https";
        _httpContextAccessorMock.Object.HttpContext.Request.Host = new HostString("example.com");
        _httpContextAccessorMock.Object.HttpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg"; 

        _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
            .Returns(new TokenModel { Id = 1 });

        _appointmentServiceMock.Setup(x => x.GetAppointmentTypeForDropDown())
            .ReturnsAsync(new List<AppointmentResponseDropDownModel>());

        // Act
        var result = await _appointmentController.GetAppointmentTypeForDropDown();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Success, result.Success);
    }

    [Fact]
    public async Task GetAppointmentListByAdmin_ValidModel_ReturnsValidResponse()
    {
        // Arrange
        var model = new CommonPaginationModel { /* initialize model properties */ };
        var expectedResult = new ApiResponse<AppointmentResponseModelForAdmin> { Success = true };
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost:7061");

        httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
        _appointmentController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
            .Returns(new TokenModel { Id = 1 });

        _appointmentServiceMock.Setup(x => x.GetAppointmentListByAdmin(It.IsAny<CommonPaginationModel>()))
            .ReturnsAsync(new List<AppointmentResponseModelForAdmin>());

        // Act
        var result = await _appointmentController.GetAppointmentListByAdmin(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Success, result.Success);
    }

    [Fact]
    public async Task GetAppointmentDetailByIdForAdmin_ValidId_ReturnsValidResponse()
    {
        // Arrange
        var appointmentId = 1;
        var expectedResult = new ApiPostResponse<AppointmentResponseModelForAdmin> { Success = true };
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost:7061");

        httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
        _appointmentController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
            .Returns(new TokenModel { Id = 1 });

        _appointmentServiceMock.Setup(x => x.GetAppointmentDetailByIdForAdmin(appointmentId))
            .ReturnsAsync(new AppointmentResponseModelForAdmin());

        // Act
        var result = await _appointmentController.GetAppointmentDetailByIdForAdmin(appointmentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Success, result.Success);
    }
}
