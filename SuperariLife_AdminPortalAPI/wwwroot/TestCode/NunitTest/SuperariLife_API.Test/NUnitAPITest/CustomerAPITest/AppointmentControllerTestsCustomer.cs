using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using NUnit.Framework;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Appointment;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Appointment;
using SuperariLife.Service.JWTAuthentication;
using AppointmentController = SuperariLifeAPI.Areas.CustomerPortal.Controllers.AppointmentController;

[TestFixture]
public class AppointmentControllerTestsCustomer
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
    public async Task InsertUpdateAppointmentByCustomer_ValidData_ReturnsSuccessResponse()
    {
        // Arrange
        var model = new AppointmentReqModelByCustomer();
        var expectedResult = new BaseApiResponse { Success = true };
        var jwtToken = "sampleJwtToken";
        _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
            .Returns(new StringValues($"Bearer {jwtToken}"));
        _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
            .Returns(new TokenModel { Id = 1 });
        _appointmentServiceMock.Setup(x => x.InsertUpdateAppointmentByCustomer(model))
            .ReturnsAsync(StatusResult.Updated);

        // Act
        var result = await _appointmentController.InsertUpdateAppointmentByCustomer(model);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResult.Success, result.Success);
    }

    [Test]
    public async Task GetAppointmentTypeForDropDown_ReturnsSuccessResponse()
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
    public async Task GetAppointmentListByCustomer_ReturnsSuccessResponse()
    {
        // Arrange
        var model = new CommonPaginationModel();
        var expectedResult = new ApiResponse<AppointmentResponseModelForCustomer> { Success = true };
        var jwtToken = "sampleJwtToken";
        _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
            .Returns(new StringValues($"Bearer {jwtToken}"));
        _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
            .Returns(new TokenModel { Id = 1 });
        _appointmentServiceMock.Setup(x => x.GetAppointmentListByCustomer(model))
            .ReturnsAsync(new List<AppointmentResponseModelForCustomer>());

        // Act
        var result = await _appointmentController.GetAppointmentListByCustomer(model);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResult.Success, result.Success);
    }

    [Test]
    public async Task GetAppointmentDetailByIdForCustomer_ReturnsSuccessResponse()
    {
        // Arrange
        var appointmentId = 1;
        var expectedResult = new ApiPostResponse<AppointmentResponseModelForCustomer> { Success = true };
        var jwtToken = "sampleJwtToken";
        _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
            .Returns(new StringValues($"Bearer {jwtToken}"));
        _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
            .Returns(new TokenModel { Id = 1 });
        _appointmentServiceMock.Setup(x => x.GetAppointmentDetailByIdForCustomer(appointmentId))
            .ReturnsAsync(new AppointmentResponseModelForCustomer());

        // Act
        var result = await _appointmentController.GetAppointmentDetailByIdForCustomer(appointmentId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResult.Success, result.Success);
    }

    [Test]
    public async Task DeleteAppointmentByCustomer_ReturnsSuccessResponse()
    {
        // Arrange
        var appointmentId = 1;
        var expectedResult = new ApiPostResponse<long> { Success = true };
        var jwtToken = "sampleJwtToken";
        _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
            .Returns(new StringValues($"Bearer {jwtToken}"));
        _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
            .Returns(new TokenModel { Id = 1 });
        _appointmentServiceMock.Setup(x => x.DeleteAppointmentByCustomer(appointmentId))
            .ReturnsAsync(Status.Success);

        // Act
        var result = await _appointmentController.DeleteAppointmentByCustomer(appointmentId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResult.Success, result.Success);
    }
}
