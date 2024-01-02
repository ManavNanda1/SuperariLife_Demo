using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Customer.CustomerAddress;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Customer.CustomerAddress;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;
using Xunit;

namespace SuperariLife_API.Test.xUnitAPITest.CustomerPortalAPITest
{
    public class CustomerAddressControllerTests
    {
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<ICustomerAddressService> _customerAddressServiceMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
        private readonly CustomerAddressController _customerAddressController;

        public CustomerAddressControllerTests()
        {
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _customerAddressServiceMock = new Mock<ICustomerAddressService>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _customerAddressController = new CustomerAddressController(
                _customerAddressServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object,
                null // Pass appropriate mock for Microsoft.AspNetCore.Hosting.IHostingEnvironment if needed
            );
        }

        [Fact]
        public async Task InsertUpdateCustomerAddressByCustomer_ReturnsSuccess()
        {
            // Arrange
            var customerAddressReqModel = new CustomerAddressReqModelForAdmin();
            var expectedResult = new ApiPostResponse<CustomerAddressInsertUpdateResponseModel> { Success = true };
            var jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _customerAddressServiceMock.Setup(x => x.InsertUpdateCustomerAddressByCustomer(It.IsAny<CustomerAddressReqModelForAdmin>()))
                .ReturnsAsync(new CustomerAddressInsertUpdateResponseModel { StatusOfInsertUpdate = StatusResult.Updated });

            // Act
            var result = await _customerAddressController.InsertUpdateCustomerAddressByCustomer(customerAddressReqModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }

        [Fact]
        public async Task GetCustomerAddressList_ReturnsCustomerAddressList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<CustomerAddressResponseModel> { Success = true, Data = new List<CustomerAddressResponseModel>() };
            var jwtToken = "your_jwt_token_here";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _customerAddressServiceMock.Setup(x => x.GetCustomerAddressList(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<CustomerAddressResponseModel>());

            // Act
            var result = await _customerAddressController.GetCustomerAddressList(commonPaginationModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetCustomerAddressById_ReturnsCustomerAddressInfo()
        {
            // Arrange
            const long customerAddressId = 1;
            var expectedResult = new ApiPostResponse<CustomerAddressResponseModel> { Success = true, Data = new CustomerAddressResponseModel() };
            var jwtToken = "your_jwt_token_here";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _customerAddressServiceMock.Setup(x => x.GetCustomerAddressById(customerAddressId))
                .ReturnsAsync(new CustomerAddressResponseModel());

            // Act
            var result = await _customerAddressController.GetCustomerAddressById(customerAddressId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task DeleteCustomerAddress_ReturnsSuccess()
        {
            // Arrange
            const int customerAddressId = 1;
            var expectedResult = new BaseApiResponse { Success = true };

            _customerAddressServiceMock.Setup(x => x.DeleteCustomerAddress(customerAddressId))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _customerAddressController.DeletePaymentType(customerAddressId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }
    }
}
