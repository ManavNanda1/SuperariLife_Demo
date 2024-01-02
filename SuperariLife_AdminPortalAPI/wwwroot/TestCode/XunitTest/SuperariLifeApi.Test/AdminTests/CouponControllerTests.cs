using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLifeAPI.Areas.Admin.Controllers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.CouponCode;
using SuperariLife.Model.Token;
using SuperariLife.Service.Coupon;
using SuperariLife.Service.JWTAuthentication;
using Xunit;
using SuperariLife.Data.DBRepository.Coupon;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;
using System.Net.Http;

public class CouponControllerTests
{
    private readonly Mock<ICouponService> _couponServiceMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;

    private CouponController _couponController;

    public CouponControllerTests()
    {
        _couponServiceMock = new Mock<ICouponService>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();

        _couponController = new CouponController(
            _couponServiceMock.Object,
            _httpContextAccessorMock.Object,
            _jwtAuthenticationServiceMock.Object,
            Mock.Of<Microsoft.Extensions.Configuration.IConfiguration>()
        );
    }

    [Fact]
    public async Task InsertUpdateCouponCode_ValidModel_ReturnsSuccessResponse()
    {
        // Arrange
        var model = new CouponCodeReqModel {  };
        var expectedResult = new BaseApiResponse { Success = true };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
        _httpContextAccessorMock.Object.HttpContext.Request.Scheme = "https";
        _httpContextAccessorMock.Object.HttpContext.Request.Host = new HostString("example.com");
        _httpContextAccessorMock.Object.HttpContext.Request.Headers[HeaderNames.Authorization] = "Bearer VALID_JWT_TOKEN"; // Replace with a valid JWT token

        _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
            .Returns(new TokenModel { Id = 1 }); 
        _couponServiceMock.Setup(x => x.InsertUpdateCouponCode(It.IsAny<CouponCodeReqModel>()))
            .ReturnsAsync(StatusResult.Updated);

        // Act
        var result = await _couponController.InsertUpdateCouponCode(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Success, result.Success);
    }

    [Fact]
    public async Task GetCouponListByAdmin_ValidModel_ReturnsValidResponse()
    {
        // Arrange
        var model = new CommonPaginationModel();
        var expectedResult = new ApiResponse<CouponCodeResponseModel> { Success = true };
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost:7061");

        httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
        _couponController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        // Act
        var result = await _couponController.GetCouponListByAdmin(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Success, result.Success);
    }

    [Fact]
    public async Task GetCouponCodeById_ValidId_ReturnsValidResponse()
    {
        // Arrange
        var couponId = 1;
        var expectedResult = new ApiPostResponse<CouponCodeResponseModel> { Success = true };
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost:7061");

        httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
        _couponController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        // Act
        var result = await _couponController.GetCouponCodeById(couponId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Success, result.Success);
    }

    [Fact]
    public async Task GetCouponCodeDetailById_ValidModel_ReturnsValidResponse()
    {
        // Arrange
        var model = new CouponCodeReqDetailModel { /* initialize model properties */ };
        var expectedResult = new ApiResponse<CouponCodeResponseModel> { Success = true };
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost:7061");

        httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
        _couponController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        // Act
        var result = await _couponController.GetCouponCodeDetailById(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Success, result.Success);
    }

    [Fact]
    public async Task DeleteCouponCode_ValidId_ReturnsSuccessResponse()
    {
        // Arrange
        var couponId = 1;
        var expectedResult = new BaseApiResponse { Success = true };
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost:7061");

        httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
        _couponController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        // Act
        var result = await _couponController.DeleteCouponCode(couponId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Success, result.Success);
    }

}
