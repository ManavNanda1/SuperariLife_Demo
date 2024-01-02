using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using NUnit.Framework;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.Customer;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Customer;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.CustomerAPITest
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
                // Set SMTPSettings properties
            });

            var appSettingsOptions = Options.Create(new AppSettings
            {
                // Set AppSettings properties
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
        public async Task Register_ValidModel_ReturnsSuccessResponse()
        {
            // Arrange
            var model = new CustomerReqModelForAdmin
            {
                CustomerEmail="test@gamil.com",
                CustomerPassword="password"
            };

            var expectedResult = new ApiPostResponse<CustomerInsertUpdateResponseModel>
            {
                Data = new CustomerInsertUpdateResponseModel(),
                Success = true,
                Message = ErrorMessages.UpdateCustomerSuccess
            };

            _customerServiceMock.Setup(x => x.InsertUpdateCustomer(It.IsAny<CustomerReqModelForAdmin>()))
                .ReturnsAsync(new CustomerInsertUpdateResponseModel { StatusOfInsertUpdate = StatusResult.Updated });

            // Act
            var result = await _customerController.Register(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.AreEqual(expectedResult.Message, result.Message);
        }
    

        [Test]
        public async Task UpdateCustomer_ValidModel_ReturnsSuccessResponse()
        {
            // Arrange
            var model = new CustomerReqModelForAdmin
            {
                CustomerEmail = "test@gamil.com"
            };

            var tokenModel = new TokenModel { Id = 1 /* Set other properties if needed */ };
            var jwtToken = "valid_jwt_token";
            var expectedResult = new ApiPostResponse<CustomerInsertUpdateResponseModel>
            {
                Data = new CustomerInsertUpdateResponseModel(),
                Success = true,
                Message = ErrorMessages.UpdateCustomerSuccess
            };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization]).Returns(jwtToken);
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken)).Returns(tokenModel);
            _customerServiceMock.Setup(x => x.InsertUpdateCustomer(It.IsAny<CustomerReqModelForAdmin>()))
                .ReturnsAsync(new CustomerInsertUpdateResponseModel { StatusOfInsertUpdate = StatusResult.Updated });

            // Act
            var result = await _customerController.UpdateCustomer(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.AreEqual(expectedResult.Message, result.Message);
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
            var jwtToken = "valid_jwt_token";
            httpContext.Request.Headers[HeaderNames.Authorization] = $"Bearer {jwtToken}";
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

       
    }
}
