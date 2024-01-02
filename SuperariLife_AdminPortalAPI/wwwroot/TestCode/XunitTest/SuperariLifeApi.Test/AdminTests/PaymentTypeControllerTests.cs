using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.PaymentType;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.PaymentType;
using SuperariLifeAPI.Areas.Admin.Controllers;
using Xunit;

namespace SuperariLife.Tests.Controllers.Admin
{
    public class PaymentTypeControllerTests
    {
        private readonly Mock<IPaymentTypeService> _paymentTypeServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly PaymentTypeController _paymentTypeController;

        public PaymentTypeControllerTests()
        {
            _paymentTypeServiceMock = new Mock<IPaymentTypeService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();

            _paymentTypeController = new PaymentTypeController(
                _paymentTypeServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object
            );
        }

        [Fact]
        public async Task InsertUpdatePaymentType_ReturnsSuccess()
        {
            // Arrange
            var paymentTypeReqModel = new PaymentTypeReqModel
            {
            };
            var expectedResult = new BaseApiResponse { Success = true };
            var jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _paymentTypeServiceMock.Setup(x => x.InsertUpdatePaymentType(It.IsAny<PaymentTypeReqModel>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _paymentTypeController.InsertUpdatePaymentType(paymentTypeReqModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }

        [Fact]
        public async Task GetPaymentTypeListByAdmin_ReturnsPaymentTypeList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<PaymentTypeResponseModel> { Success = true, Data = new List<PaymentTypeResponseModel>() };
            _paymentTypeServiceMock.Setup(x => x.GetPaymentTypeList(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<PaymentTypeResponseModel>());

            // Act
            var result = await _paymentTypeController.GetPaymentTypeListByAdmin(commonPaginationModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetPaymentTypeById_ReturnsPaymentTypeDetail()
        {
            // Arrange
            const long paymentTypeId = 1;
            var expectedResult = new ApiPostResponse<PaymentTypeResponseModel> { Success = true, Data = new PaymentTypeResponseModel() };
            _paymentTypeServiceMock.Setup(x => x.GetPaymentTypeById(paymentTypeId))
                .ReturnsAsync(new PaymentTypeResponseModel());

            // Act
            var result = await _paymentTypeController.GetPaymentTypeById(paymentTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task DeletePaymentType_ReturnsSuccess()
        {
            // Arrange
            const long paymentTypeId = 1;
            var expectedResult = new BaseApiResponse { Success = true };
            _paymentTypeServiceMock.Setup(x => x.DeletePaymentType(paymentTypeId))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _paymentTypeController.DeletePaymentType(paymentTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }

    }
}
