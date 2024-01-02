using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Event;
using SuperariLife.Service.Event;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.CustomerPortalAPITest
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
        public async Task GetEventListForCustomer_ReturnsEventList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<EventResponseModel> { Success = true, Data = new List<EventResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");
            _eventController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _eventServiceMock.Setup(x => x.GetEventListByAdmin(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<EventResponseModel>());

            // Act
            var result = await _eventController.GetEventListForCustomer(commonPaginationModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetEventDetailByIdForCustomer_ReturnsEventDetail()
        {
            // Arrange
            const long eventId = 1;
            var expectedResult = new ApiPostResponse<EventResponseModel> { Success = true, Data = new EventResponseModel() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");
            _eventController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _eventServiceMock.Setup(x => x.GetEventByIdForAdmin(eventId))
                .ReturnsAsync(new EventResponseModel());

            // Act
            var result = await _eventController.GetEventDetailByIdForCustomer(eventId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }
    }
}
