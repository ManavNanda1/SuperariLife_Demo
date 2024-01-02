using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Event;
using SuperariLife.Model.Token;
using SuperariLife.Service.Event;
using SuperariLife.Service.JWTAuthentication;
using EventController = SuperariLifeAPI.Areas.Admin.Controllers.EventController;

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class EventControllerTests
    {
        private Mock<IEventService> _eventServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment> _hostingEnvironmentMock;
        private Mock<IConfiguration> _configurationMock;
        private EventController _eventController;

        [SetUp]
        public void Setup()
        {
            _eventServiceMock = new Mock<IEventService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _hostingEnvironmentMock = new Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
            _configurationMock = new Mock<IConfiguration>();
            _eventController = new EventController(
                _eventServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object,
                _hostingEnvironmentMock.Object,
                _configurationMock.Object
            );
        }

        [Test]
        public async Task InsertUpdateEvent_ReturnsSuccess()
        {
            // Arrange
            var eventReqModel = new EventReqModel();
            var expectedResult = new BaseApiResponse { Success = true };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns("Bearer YourTokenHere");

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
                .Returns(new TokenModel { Id = 1 });

            _eventServiceMock.Setup(x => x.InsertUpdateEvent(It.IsAny<EventReqModel>(), It.IsAny<List<EventGalleryImages>>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _eventController.InsertUpdateEvent(eventReqModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task GetEventListByAdmin_ReturnsEventList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<EventResponseModel> { Success = true, Data = new List<EventResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _eventController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _eventServiceMock.Setup(x => x.GetEventListByAdmin(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<EventResponseModel>());

            // Act
            var result = await _eventController.GetEventListByAdmin(commonPaginationModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetEventDetailByIdForAdmin_ReturnsEventDetails()
        {
            // Arrange
            const long eventId = 1;
            var expectedResult = new ApiPostResponse<EventResponseModel> { Success = true, Data = new EventResponseModel() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _eventController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _eventServiceMock.Setup(x => x.GetEventByIdForAdmin(eventId))
                .ReturnsAsync(new EventResponseModel());

            // Act
            var result = await _eventController.GetEventDetailByIdForAdmin(eventId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetEventDetailOfQuestion_ReturnsQuestionDetails()
        {
            // Arrange
            const string questionId = "1,2,3";
            var expectedResult = new ApiResponse<QuestionEventResponseModel> { Success = true, Data = new List<QuestionEventResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _eventController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _eventServiceMock.Setup(x => x.GetEventDetailOfQuestion(questionId))
                .ReturnsAsync(new List<QuestionEventResponseModel>());

            // Act
            var result = await _eventController.GetEventDetailOfQuestion(questionId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetEventDetailOfCustomerParticipant_ReturnsCustomerDetails()
        {
            // Arrange
            var eventCustomerReqModel = new EventCustomerReqModel();
            var expectedResult = new ApiResponse<EventCustomerResponseModel> { Success = true, Data = new List<EventCustomerResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _eventController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _eventServiceMock.Setup(x => x.GetEventDetailOfCustomerParticipant(It.IsAny<EventCustomerReqModel>()))
                .ReturnsAsync(new List<EventCustomerResponseModel>());

            // Act
            var result = await _eventController.GetEventDetailOfCustomerParticipant(eventCustomerReqModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task DeleteEventQuestion_ReturnsSuccess()
        {
            // Arrange
            const long eventQuestionId = 1;
            var expectedResult = new BaseApiResponse { Success = true };

            _eventServiceMock.Setup(x => x.DeleteEventQuestion(eventQuestionId))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _eventController.DeleteEventQuestion(eventQuestionId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task DeleteEventGalleryImage_ReturnsSuccess()
        {
            // Arrange
            const string galleryImageName = "image.jpg";
            var expectedResult = new BaseApiResponse { Success = true };

            _eventServiceMock.Setup(x => x.DeleteEventGalleryImage(galleryImageName))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _eventController.DeleteEventGalleryImage(galleryImageName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task DeleteEvent_ReturnsSuccess()
        {
            // Arrange
            var eventDeleteInfo = new CommonDeleteModel { Name = "EventName" };
            var expectedResult = new BaseApiResponse { Success = true };

            _eventServiceMock.Setup(x => x.DeleteEvent(eventDeleteInfo))
                .ReturnsAsync(Status.Success);

            _hostingEnvironmentMock.Setup(x => x.WebRootPath)
                .Returns("wwwroot");

            // Act
            var result = await _eventController.DeleteEvent(eventDeleteInfo);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }
    }
}
