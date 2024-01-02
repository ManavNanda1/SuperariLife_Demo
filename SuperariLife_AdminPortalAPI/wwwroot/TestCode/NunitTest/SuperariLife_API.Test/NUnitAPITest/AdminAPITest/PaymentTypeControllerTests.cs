using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class PaymentTypeControllerTests
    {
        private Mock<IPaymentTypeService> _paymentTypeServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private PaymentTypeController _paymentTypeController;

        [SetUp]
        public void Setup()
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

        [Test]
        public async Task InsertUpdatePaymentType_ReturnsSuccess()
        {
            // Arrange
            var paymentTypeReqModel = new PaymentTypeReqModel();
            var expectedResult = new BaseApiResponse { Success = true };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns("Bearer YourTokenHere");

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
                .Returns(new TokenModel { Id = 1 });

            _paymentTypeServiceMock.Setup(x => x.InsertUpdatePaymentType(It.IsAny<PaymentTypeReqModel>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _paymentTypeController.InsertUpdatePaymentType(paymentTypeReqModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task GetPaymentTypeListByAdmin_ReturnsPaymentTypeList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<PaymentTypeResponseModel> { Success = true, Data = new List<PaymentTypeResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _paymentTypeController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };


            _paymentTypeServiceMock.Setup(x => x.GetPaymentTypeList(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<PaymentTypeResponseModel>());

            // Act
            var result = await _paymentTypeController.GetPaymentTypeListByAdmin(commonPaginationModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetPaymentTypeById_ReturnsPaymentTypeDetails()
        {
            // Arrange
            const long paymentTypeId = 1;
            var expectedResult = new ApiPostResponse<PaymentTypeResponseModel> { Success = true, Data = new PaymentTypeResponseModel() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _paymentTypeController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _paymentTypeServiceMock.Setup(x => x.GetPaymentTypeById(paymentTypeId))
                .ReturnsAsync(new PaymentTypeResponseModel());

            // Act
            var result = await _paymentTypeController.GetPaymentTypeById(paymentTypeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
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
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }
    }
}
