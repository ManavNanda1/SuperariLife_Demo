using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Customer.CustomerAddress;
using SuperariLife.Model.Token;
using SuperariLife.Service.Customer.CustomerAddress;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;


namespace SuperariLife_API.Test.NUnitAPITest.CustomerAPITest
{
    [TestFixture]
    public class CustomerAddressControllerTests
    {
        private CustomerAddressController _customerAddressController;
        private Mock<ICustomerAddressService> _customerAddressServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment> _hostingEnvironmentMock;

        [SetUp]
        public void Setup()
        {
            _customerAddressServiceMock = new Mock<ICustomerAddressService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _hostingEnvironmentMock = new Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();

            _customerAddressController = new CustomerAddressController(
                _customerAddressServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object,
                _hostingEnvironmentMock.Object
            );
        }

        [Test]
        public async Task InsertUpdateCustomerAddressByCustomer_ValidModel_ReturnsSuccessResponse()
        {
            // Arrange
            var model = new CustomerAddressReqModelForAdmin
            {
                // Set properties of the model for the test
            };

            var tokenModel = new TokenModel { Id = 1 /* Set other properties if needed */ };
            var jwtToken = "valid_jwt_token";
            var expectedResult = new ApiPostResponse<CustomerAddressInsertUpdateResponseModel>
            {
                Data = new CustomerAddressInsertUpdateResponseModel(),
                Success = true,
                Message = ErrorMessages.UpdateCustomerAddressSuccess
            };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization]).Returns(jwtToken);
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken)).Returns(tokenModel);
            _customerAddressServiceMock.Setup(x => x.InsertUpdateCustomerAddressByCustomer(It.IsAny<CustomerAddressReqModelForAdmin>()))
                .ReturnsAsync(new CustomerAddressInsertUpdateResponseModel { StatusOfInsertUpdate = StatusResult.Updated });

            // Act
            var result = await _customerAddressController.InsertUpdateCustomerAddressByCustomer(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.AreEqual(expectedResult.Message, result.Message);
        }

        [Test]
        public async Task GetCustomerAddressList_ReturnsCustomerAddressList()
        {
            // Arrange
            var model = new CommonPaginationModel();
            var expectedResult = new ApiResponse<CustomerAddressResponseModel>
            {
                Data = new List<CustomerAddressResponseModel> { new CustomerAddressResponseModel() },
                Success = true
            };

            _customerAddressServiceMock.Setup(x => x.GetCustomerAddressList(model))
                .ReturnsAsync(new List<CustomerAddressResponseModel> { new CustomerAddressResponseModel() });

            // Act
            var result = await _customerAddressController.GetCustomerAddressList(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            CollectionAssert.IsNotEmpty(result.Data);
        }

        [Test]
        public async Task GetCustomerAddressById_ReturnsCustomerAddress()
        {
            // Arrange
            var customerId = 1;
            var expectedResult = new ApiPostResponse<CustomerAddressResponseModel>
            {
                Data = new CustomerAddressResponseModel(),
                Success = true
            };

            _customerAddressServiceMock.Setup(x => x.GetCustomerAddressById(customerId))
                .ReturnsAsync(new CustomerAddressResponseModel());

            // Act
            var result = await _customerAddressController.GetCustomerAddressById(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task DeletePaymentType_ValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var customerId = 1;
            var expectedResult = new BaseApiResponse
            {
                Success = true,
                Message = ErrorMessages.DeleteTestimonialReviewSuccess
            };

            _customerAddressServiceMock.Setup(x => x.DeleteCustomerAddress(customerId))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _customerAddressController.DeletePaymentType(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.AreEqual(expectedResult.Message, result.Message);
        }
    }
}