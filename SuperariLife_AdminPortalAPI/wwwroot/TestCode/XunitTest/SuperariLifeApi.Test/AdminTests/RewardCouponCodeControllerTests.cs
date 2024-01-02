using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.EmailNotification;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.CouponCode;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Coupon.RewardCouponCode;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.Admin.Controllers;
using Xunit;

namespace SuperariLife.Tests.Controllers.Admin
{
    public class RewardCouponCodeControllerTests
    {
        private readonly Mock<IRewardCouponCodeService> _rewardCouponCodeServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly Mock<IOptions<SMTPSettings>> _smtpSettingsMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly RewardCouponCodeController _rewardCouponCodeController;

        public RewardCouponCodeControllerTests()
        {
            _rewardCouponCodeServiceMock = new Mock<IRewardCouponCodeService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _smtpSettingsMock = new Mock<IOptions<SMTPSettings>>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _configurationMock = new Mock<IConfiguration>();

            _rewardCouponCodeController = new RewardCouponCodeController(
                _rewardCouponCodeServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object,
                _smtpSettingsMock.Object,
                _appSettingsMock.Object,
                _configurationMock.Object
            );
        }

        [Fact]
        public async Task InsertUpdateRewardCouponCode_ReturnsSuccess()
        {
            // Arrange
            var rewardCouponCodeReqModel = new RewardCouponCodeReqModel
            {
            };
            var expectedResult = new BaseApiResponse { Success = true };
            var jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _rewardCouponCodeServiceMock.Setup(x => x.InsertUpdateRewardCouponCode(It.IsAny<RewardCouponCodeReqModel>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _rewardCouponCodeController.InsertUpdateRewardCouponCode(rewardCouponCodeReqModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }

        [Fact]
        public async Task GetRewardCouponCodeListByAdmin_ReturnsRewardCouponCodeList()
        {
            // Arrange
            var expectedResult = new ApiResponse<RewardCouponCodeResponseModel> { Success = true, Data = new List<RewardCouponCodeResponseModel>() };
            _rewardCouponCodeServiceMock.Setup(x => x.GetRewardCouponCodeListByAdmin(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<RewardCouponCodeResponseModel>());

            // Act
            var result = await _rewardCouponCodeController.GetRewardCouponCodeListByAdmin(new CommonPaginationModel());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetCustomerListForSendingCouponCodeReward_ReturnsCustomerList()
        {
            // Arrange
            var expectedResult = new ApiResponse<CustomerListForSendingCouponCodeRewardResponseModel>
            {
                Success = true,
                Data = new List<CustomerListForSendingCouponCodeRewardResponseModel>()
            };
            _rewardCouponCodeServiceMock.Setup(x => x.GetCustomerListForSendingCouponCodeReward(It.IsAny<string>()))
                .ReturnsAsync(new List<CustomerListForSendingCouponCodeRewardResponseModel>());

            // Act
            var result = await _rewardCouponCodeController.GetCustomerListForSendingCouponCodeReward(null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }
    }
}
