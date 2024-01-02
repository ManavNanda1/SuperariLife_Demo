using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.Coupon;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.CouponCode;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.Admin.Controllers;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class CouponControllerTests
    {
        private Mock<ICouponService> _couponServiceMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IConfiguration> _configurationMock;
        private CouponController _couponController;

        [SetUp]
        public void Setup()
        {
            _couponServiceMock = new Mock<ICouponService>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _configurationMock = new Mock<IConfiguration>();
            _couponController = new CouponController(
                _couponServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object,
                _configurationMock.Object
            );
        }

        [Test]
        public async Task InsertUpdateCouponCode_ReturnsSuccess()
        {
            // Arrange
            var couponCodeReqModel = new CouponCodeReqModel();
            var expectedResult = new BaseApiResponse { Success = true };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns("Bearer YourTokenHere");

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
                .Returns(new TokenModel { Id = 1 });

            _couponServiceMock.Setup(x => x.InsertUpdateCouponCode(It.IsAny<CouponCodeReqModel>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _couponController.InsertUpdateCouponCode(couponCodeReqModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task GetCouponListByAdmin_ReturnsCouponList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<CouponCodeResponseModel> { Success = true, Data = new List<CouponCodeResponseModel>() };

            _couponServiceMock.Setup(x => x.GetCouponListByAdmin(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<CouponCodeResponseModel>());

            // Act
            var result = await _couponController.GetCouponListByAdmin(commonPaginationModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetCouponCodeById_ReturnsCouponCodeInfo()
        {
            // Arrange
            const long couponId = 1;
            var expectedResult = new ApiPostResponse<CouponCodeResponseModel> { Success = true, Data = new CouponCodeResponseModel() };

            _couponServiceMock.Setup(x => x.GetCouponCodeById(couponId))
                .ReturnsAsync(new CouponCodeResponseModel());

            // Act
            var result = await _couponController.GetCouponCodeById(couponId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetCouponCodeDetailById_ReturnsCouponCodeDetails()
        {
            // Arrange
            var couponCodeReqDetailModel = new CouponCodeReqDetailModel();
            var expectedResult = new ApiResponse<CouponCodeResponseModel> { Success = true, Data = new List<CouponCodeResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _couponController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _couponServiceMock.Setup(x => x.GetCouponCodeDetailById(It.IsAny<CouponCodeReqDetailModel>()))
                .ReturnsAsync(new List<CouponCodeResponseModel>());

            // Act
            var result = await _couponController.GetCouponCodeDetailById(couponCodeReqDetailModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task DeleteCouponCode_ReturnsSuccess()
        {
            // Arrange
            const long couponId = 1;
            var expectedResult = new BaseApiResponse { Success = true };

            _couponServiceMock.Setup(x => x.DeleteCouponCode(couponId))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _couponController.DeleteCouponCode(couponId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }
    }
}
