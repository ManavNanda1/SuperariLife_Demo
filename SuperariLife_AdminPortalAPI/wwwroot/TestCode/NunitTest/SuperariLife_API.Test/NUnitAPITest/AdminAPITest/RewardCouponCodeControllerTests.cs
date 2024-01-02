
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.CouponCode;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Coupon.RewardCouponCode;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.Admin.Controllers;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class RewardCouponCodeControllerTests
    {
        private Mock<IRewardCouponCodeService> _rewardCouponServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IOptions<SMTPSettings>> _smtpSettingsMock;
        private Mock<IOptions<AppSettings>> _appSettingsMock;
        private RewardCouponCodeController _rewardCouponCodeController;

        [SetUp]
        public void Setup()
        {
            _rewardCouponServiceMock = new Mock<IRewardCouponCodeService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _configurationMock = new Mock<IConfiguration>();
            _smtpSettingsMock = new Mock<IOptions<SMTPSettings>>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
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
            _rewardCouponCodeController = new RewardCouponCodeController(
                _rewardCouponServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object,
                smtpSettingsOptions,
                appSettingsOptions,
                _configurationMock.Object
            );
        }

        [Test]
        public async Task InsertUpdateRewardCouponCode_ReturnsSuccess()
        {
            // Arrange
            var rewardCouponCodeReqModel = new RewardCouponCodeReqModel();
            var expectedResult = new BaseApiResponse { Success = true };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns("Bearer YourTokenHere");

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
                .Returns(new TokenModel { Id = 1 });

            _rewardCouponServiceMock.Setup(x => x.InsertUpdateRewardCouponCode(It.IsAny<RewardCouponCodeReqModel>()))
                .ReturnsAsync(StatusResult.Updated);

            _configurationMock.Setup(x => x["Path:CustomerProfileImagePath"]).Returns("/path/to/customer/images/");

            // Act
            var result = await _rewardCouponCodeController.InsertUpdateRewardCouponCode(rewardCouponCodeReqModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task GetRewardCouponCodeListByAdmin_ReturnsRewardCouponCodeList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<RewardCouponCodeResponseModel> { Success = true, Data = new List<RewardCouponCodeResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _rewardCouponCodeController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _rewardCouponServiceMock.Setup(x => x.GetRewardCouponCodeListByAdmin(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<RewardCouponCodeResponseModel>());

            // Act
            var result = await _rewardCouponCodeController.GetRewardCouponCodeListByAdmin(commonPaginationModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetCustomerListForSendingCouponCodeReward_ReturnsCustomerList()
        {
            // Arrange
            var searchStr = "test";
            var expectedResult = new ApiResponse<CustomerListForSendingCouponCodeRewardResponseModel> { Success = true, Data = new List<CustomerListForSendingCouponCodeRewardResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _rewardCouponCodeController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _rewardCouponServiceMock.Setup(x => x.GetCustomerListForSendingCouponCodeReward(It.IsAny<string>()))
                .ReturnsAsync(new List<CustomerListForSendingCouponCodeRewardResponseModel>());

            // Act
            var result = await _rewardCouponCodeController.GetCustomerListForSendingCouponCodeReward(searchStr);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }
    }
}
