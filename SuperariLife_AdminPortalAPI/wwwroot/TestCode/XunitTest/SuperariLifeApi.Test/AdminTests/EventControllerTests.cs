using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Event;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Event;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.Admin.Controllers;
using Xunit;

namespace SuperariLife.Tests.Controllers.Admin
{
    public class EventControllerTests
    {
        private readonly Mock<IEventService> _eventServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment> _hostingEnvironmentMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly EventController _eventController;

        public EventControllerTests()
        {
            _eventServiceMock = new Mock<IEventService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _hostingEnvironmentMock = new Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
            _configurationMock = new Mock<IConfiguration>();
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

            _eventController = new EventController(
                _eventServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object,
                _hostingEnvironmentMock.Object,
                _configurationMock.Object
            );
        }

        [Fact]
        public async Task InsertUpdateEvent_ReturnsSuccess()
        {
            // Arrange
            var eventReqModel = new EventReqModel
            {
            };
            var expectedResult = new BaseApiResponse { Success = true };
            var jwtToken = "your_jwt_token_here";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _eventServiceMock.Setup(x => x.InsertUpdateEvent(It.IsAny<EventReqModel>(), It.IsAny<List<EventGalleryImages>>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _eventController.InsertUpdateEvent(eventReqModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }

        [Fact]
        public async Task GetEventListByAdmin_ReturnsEventList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<EventResponseModel> { Success = true, Data = new List<EventResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg";
            _eventController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _eventServiceMock.Setup(x => x.GetEventListByAdmin(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<EventResponseModel>());

            // Act
            var result = await _eventController.GetEventListByAdmin(commonPaginationModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetEventDetailByIdForAdmin_ReturnsEventDetail()
        {
            // Arrange
            const long eventId = 1;
            var expectedResult = new ApiPostResponse<EventResponseModel> { Success = true, Data = new EventResponseModel() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg";
            _eventController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _eventServiceMock.Setup(x => x.GetEventByIdForAdmin(eventId))
                .ReturnsAsync(new EventResponseModel());

            // Act
            var result = await _eventController.GetEventDetailByIdForAdmin(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetEventDetailOfQuestion_ReturnsQuestionDetails()
        {
            // Arrange
            const string questionId = "1,2,3";
            var expectedResult = new ApiResponse<QuestionEventResponseModel> { Success = true, Data = new List<QuestionEventResponseModel>() };
            _eventServiceMock.Setup(x => x.GetEventDetailOfQuestion(questionId))
                .ReturnsAsync(new List<QuestionEventResponseModel>());

            // Act
            var result = await _eventController.GetEventDetailOfQuestion(questionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

    }
}
