using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Appointment;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Appointment;
using SuperariLife.Service.JWTAuthentication;
using AppointmentController = SuperariLifeAPI.Areas.Admin.Controllers.AppointmentController;

[TestFixture]
public class AppointmentControllerTests
{
    private AppointmentController _appointmentController;
    private Mock<IAppointmentService> _appointmentServiceMock;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;

    [SetUp]
    public void Setup()
    {
        _appointmentServiceMock = new Mock<IAppointmentService>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();

        var smtpSettingsOptions = Options.Create(new SMTPSettings
        {
            EmailAppPassword = "pwdrcfpmcoykksgh",
            EmailEnableSsl = "true",
            EmailHostName = "smtp.gmail.com",
            EmailPort = "587",
            EmailUsername = "project.shaligraminfotech@gmail.com",
            FromEmail = "project.shaligraminfotech@gmail.com",
            FromName = "Superari Life"
        });

        var appSettingsOptions = Options.Create(new AppSettings
        {
            JWT_Secret = "4226452948404D635166546A576E5A7234743777217A25432A462D4A614E645267556B58703273357638792F413F4428472B4B6250655368566D597133743677",
            JWT_Validity_Mins = 4340,
            ErrorSendToEmail = "nilesh.y@shaligraminfotech.com",
            ForgotPasswordAttemptValidityHours = 1,
            PasswordLinkValidityMins = 2,
            EmailLogo = "/Logo/EmailLogo.png",
            EnvelopIcon = "/Logo/envelop-icon.png",
            FacebookIcon = "/Logo/facebook-icon.png",
            InstagramIcon = "/Logo/instagram-icon.png",
            LinkedIn = "/Logo/linkedin-icon.png",
            RecurimentBanner = "/Logo/superari-banner-img.png",
            EnvelopURL = "superarilife@gmail.com.com",
            FacebookURL = "https://www.facebook.com/Rakhee.Vithlani",
            InstagramURL = "https://www.instagram.com/yogi_rakhee/",
            LinkedInURL = "https://uk.linkedin.com/company/superari-ltd?trk=public_profile_topcard-current-company",
            ContactUsMail = "nilesh.y@shaligraminfotech.com"
        });

        _appointmentController = new AppointmentController(
            _appointmentServiceMock.Object,
            _httpContextAccessorMock.Object,
            _jwtAuthenticationServiceMock.Object,
            Mock.Of<Microsoft.AspNetCore.Hosting.IHostingEnvironment>(),
            Mock.Of<Microsoft.Extensions.Configuration.IConfiguration>(),
            smtpSettingsOptions,
            appSettingsOptions
        );
    }

    [Test]
    public async Task InsertUpdateAppointmentByAdmin_ReturnsSuccess()
    {
        // Arrange
        var model = new AppointmentReqModelByAdmin();
        var expectedResult = new BaseApiResponse { Success = true };
        var jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
        _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
            .Returns(new StringValues($"Bearer {jwtToken}"));
        _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
            .Returns(new TokenModel { Id = 1 });

        _appointmentServiceMock.Setup(x => x.InsertUpdateAppointmentByAdmin(It.IsAny<AppointmentReqModelByAdmin>()))
            .ReturnsAsync(StatusResult.Updated);

        // Act
        var result = await _appointmentController.InsertUpdateAppointmentByAdmin(model);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResult.Success, result.Success);
    }

    [Test]
    public async Task GetAppointmentTypeForDropDown_ReturnsSuccess()
    {
        // Arrange
        var expectedResult = new ApiResponse<AppointmentResponseDropDownModel> { Success = true };

        _appointmentServiceMock.Setup(x => x.GetAppointmentTypeForDropDown())
            .ReturnsAsync(new List<AppointmentResponseDropDownModel>());

        // Act
        var result = await _appointmentController.GetAppointmentTypeForDropDown();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResult.Success, result.Success);
    }

    [Test]
    public async Task GetAppointmentListByAdmin_ReturnsSuccess()
    {
        // Arrange
        var model = new CommonPaginationModel();
    
        var expectedResult = new ApiResponse<AppointmentResponseModelForAdmin> { Success = true };
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost:7061");

        httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
        _appointmentController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
      

        _appointmentServiceMock.Setup(x => x.GetAppointmentListByAdmin(model))
            .ReturnsAsync(new List<AppointmentResponseModelForAdmin>());

        // Act
        var result = await _appointmentController.GetAppointmentListByAdmin(model);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResult.Success, result.Success);
    }

    [Test]
    public async Task GetAppointmentDetailByIdForAdmin_ReturnsSuccess()
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

        _appointmentServiceMock.Setup(x => x.GetAppointmentDetailByIdForAdmin(appointmentId))
            .ReturnsAsync(new AppointmentResponseModelForAdmin());

        // Act
        var result = await _appointmentController.GetAppointmentDetailByIdForAdmin(appointmentId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResult.Success, result.Success);
    }
}
