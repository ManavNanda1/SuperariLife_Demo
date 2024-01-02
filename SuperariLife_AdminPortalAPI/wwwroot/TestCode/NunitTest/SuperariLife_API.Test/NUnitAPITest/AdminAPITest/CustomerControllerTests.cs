using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.Customer;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Customer;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using CustomerController = SuperariLifeAPI.Areas.Admin.Controllers.CustomerController;

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class CustomerControllerTests
    {
        private Mock<ICustomerService> _customerServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment> _hostingEnvironmentMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IOptions<SMTPSettings>> _smtpSettingsMock;
        private Mock<IOptions<AppSettings>> _appSettingsMock;
        private CustomerController _customerController;

        [SetUp]
        public void Setup()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _hostingEnvironmentMock = new Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
            _configurationMock = new Mock<IConfiguration>();
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
            _customerController = new CustomerController(
                _customerServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object,
                _hostingEnvironmentMock.Object,
                _configurationMock.Object,
                 smtpSettingsOptions,
                 appSettingsOptions
            );
        }

        [Test]
        public async Task InsertUpdateCustomerByAdmin_ReturnsSuccess()
        {
            // Arrange
            var customerReqModelForAdmin = new CustomerReqModelForAdmin
            {
                CustomerEmail = "nilesh.y12340@gmail.com"
            };
            var expectedResult = new ApiPostResponse<CustomerInsertUpdateResponseModel> { Success = true };
            var jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
                .Returns(new TokenModel { Id = 1 });

            _customerServiceMock.Setup(x => x.InsertUpdateCustomerByAdmin(It.IsAny<CustomerReqModelForAdmin>()))
                .ReturnsAsync(new CustomerInsertUpdateResponseModel { StatusOfInsertUpdate = StatusResult.Updated });

            // Act
            var result = await _customerController.InsertUpdateCustomerByAdmin(customerReqModelForAdmin);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task GetCustomerListByAdmin_ReturnsCustomerList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<CustomerResponseModel> { Success = true, Data = new List<CustomerResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _customerController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _customerServiceMock.Setup(x => x.GetCustomerListByAdmin(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<CustomerResponseModel>());

            // Act
            var result = await _customerController.GetCustomerListByAdmin(commonPaginationModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetCustomerById_ReturnsCustomerInfo()
        {
            // Arrange
            const long customerId = 1;
            var expectedResult = new ApiPostResponse<CustomerResponseModel> { Success = true, Data = new CustomerResponseModel() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _customerController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _customerServiceMock.Setup(x => x.GetCustomerByIdByAdmin(customerId))
                .ReturnsAsync(new CustomerResponseModel());

            // Act
            var result = await _customerController.GetCustomerById(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task DeleteCustomerByAdmin_ReturnsSuccess()
        {
            // Arrange
            const long customerId = 1;
            var expectedResult = new ApiPostResponse<CustomerDeleteResponseModel> { Success = true, Data = new CustomerDeleteResponseModel() };

            _customerServiceMock.Setup(x => x.DeleteCustomerByAdmin(customerId))
                .ReturnsAsync(new CustomerDeleteResponseModel { StatusOfDelete = Status.Success });

            // Act
            var result = await _customerController.DeleteCustomerByAdmin(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task ActiveDeactiveUserByAdmin_ReturnsSuccess()
        {
            // Arrange
            const long customerId = 1;
            var expectedResult = new ApiPostResponse<CustomerResponseModel> { Success = true, Data = new CustomerResponseModel() };

            _customerServiceMock.Setup(x => x.ActiveDeactiveCustomerByAdmin(customerId))
                .ReturnsAsync(new CustomerResponseModel { IsActive = ActiveStatus.Inactive });

            // Act
            var result = await _customerController.ActiveDeactiveUserByAdmin(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }
    }
}
