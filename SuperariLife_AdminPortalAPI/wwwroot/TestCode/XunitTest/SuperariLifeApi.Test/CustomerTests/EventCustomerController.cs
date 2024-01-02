using System.Collections.Generic;
using System.Threading.Tasks;
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
using Xunit;

namespace SuperariLife_API.Test.xUnitAPITest.CustomerPortalAPITest
{
    public class EventCustomerControllerTests
    {
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IEventService> _eventServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly EventController _eventController;

        public EventCustomerControllerTests()
        {
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _eventServiceMock = new Mock<IEventService>();
            _configurationMock = new Mock<IConfiguration>();
            _eventController = new EventController(
                _eventServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object,
                null, // Pass appropriate mock for Microsoft.AspNetCore.Hosting.IHostingEnvironment if needed
                _configurationMock.Object
            );
        }

        [Fact]
        public async Task GetEventListForCustomer_ReturnsEventList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<EventResponseModel> { Success = true, Data = new List<EventResponseModel>() };
            var path = "https://localhost";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Host)
                .Returns(new HostString("localhost"));
            _eventServiceMock.Setup(x => x.GetEventListByAdmin(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<EventResponseModel>());

            // Act
            var result = await _eventController.GetEventListForCustomer(commonPaginationModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetEventDetailByIdForCustomer_ReturnsEventDetail()
        {
            // Arrange
            const long eventId = 1;
            var expectedResult = new ApiPostResponse<EventResponseModel> { Success = true, Data = new EventResponseModel() };
            var path = "https://localhost";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Host)
                .Returns(new HostString("localhost"));
            _eventServiceMock.Setup(x => x.GetEventByIdForAdmin(eventId))
                .ReturnsAsync(new EventResponseModel());

            // Act
            var result = await _eventController.GetEventDetailByIdForCustomer(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }
    }
}
