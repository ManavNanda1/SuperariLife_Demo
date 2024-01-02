using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;
using Xunit;

namespace SuperariLife_API.Test.xUnitAPITest.CustomerPortalAPITest
{
    public class AppointmentCustomerControllerTests
    {
        private readonly Mock<IAppointmentService> _appointmentServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment> _hostingEnvironmentMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IOptions<SMTPSettings>> _smtpSettingsMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
        private readonly AppointmentController _appointmentController;

        public AppointmentCustomerControllerTests()
        {
            _appointmentServiceMock = new Mock<IAppointmentService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _hostingEnvironmentMock = new Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
            _configurationMock = new Mock<IConfiguration>();
            var smtpSettingsOptions = Options.Create(new SMTPSettings
            {
                // SMTP settings initialization
            });

            var appSettingsOptions = Options.Create(new AppSettings
            {
                // App settings initialization
            });

            _appointmentController = new AppointmentController(
                _appointmentServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object,
                _hostingEnvironmentMock.Object,
                _configurationMock.Object,
                smtpSettingsOptions,
                appSettingsOptions
            );
        }

        [Fact]
        public async Task InsertUpdateAppointmentByCustomer_ReturnsSuccess()
        {
            // Arrange
            var appointmentReqModelByCustomer = new AppointmentReqModelByCustomer();
            var expectedResult = new BaseApiResponse { Success = true };
            var jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _appointmentServiceMock.Setup(x => x.InsertUpdateAppointmentByCustomer(It.IsAny<AppointmentReqModelByCustomer>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _appointmentController.InsertUpdateAppointmentByCustomer(appointmentReqModelByCustomer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }

        [Fact]
        public async Task GetAppointmentTypeForDropDown_ReturnsAppointmentTypeList()
        {
            // Arrange
            var expectedResult = new ApiResponse<AppointmentResponseDropDownModel> { Success = true, Data = new List<AppointmentResponseDropDownModel>() };

            _appointmentServiceMock.Setup(x => x.GetAppointmentTypeForDropDown())
                .ReturnsAsync(new List<AppointmentResponseDropDownModel>());

            // Act
            var result = await _appointmentController.GetAppointmentTypeForDropDown();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        // Other test methods...

        [Fact]
        public async Task DeleteAppointmentByCustomer_ReturnsSuccess()
        {
            // Arrange
            const long appointmentId = 1;
            var expectedResult = new ApiPostResponse<long> { Success = true };

            _appointmentServiceMock.Setup(x => x.DeleteAppointmentByCustomer(appointmentId))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _appointmentController.DeleteAppointmentByCustomer(appointmentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }
    }
}
