using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.State;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.State;
using SuperariLifeAPI.Areas.Admin.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class StateControllerTests
    {
        private Mock<IStateService> _stateServiceMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private StateController _stateController;

        [SetUp]
        public void Setup()
        {
            _stateServiceMock = new Mock<IStateService>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _stateController = new StateController(
                _jwtAuthenticationServiceMock.Object,
                _httpContextAccessorMock.Object,
                _stateServiceMock.Object
            );
        }

        [Test]
        public async Task AddUpdateState_ReturnsSuccess()
        {
            // Arrange
            var stateRequestModel = new StateRequestModel();
            var expectedResult = new ApiPostResponse<int> { Success = true, Data = 1 };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues("Bearer YourTokenHere"));

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
                .Returns(new TokenModel { Id = 1 });

            _stateServiceMock.Setup(x => x.AddUpdateState(It.IsAny<StateRequestModel>()))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _stateController.AddUpdateState(stateRequestModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task DeleteState_ReturnsSuccess()
        {
            // Arrange
            var stateId = 1;
            var expectedResult = new ApiPostResponse<int> { Success = true, Data = 1 };

            _stateServiceMock.Setup(x => x.DeleteState(It.IsAny<int>()))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _stateController.DeleteState(stateId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetStateListByCountryId_ReturnsStateList()
        {
            // Arrange
            var countryId = 1;
            var expectedResult = new ApiResponse<StateModel> { Success = true, Data = new List<StateModel>() };

            _stateServiceMock.Setup(x => x.GetStateList(It.IsAny<int>()))
                .ReturnsAsync(new List<StateModel>());

            // Act
            var result = await _stateController.GetStateListByCountryId(countryId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetStateById_ReturnsState()
        {
            // Arrange
            var stateId = 1;
            var expectedResult = new ApiPostResponse<StateModel> { Success = true, Data = new StateModel() };

            _stateServiceMock.Setup(x => x.GetStateListById(It.IsAny<int>()))
                .ReturnsAsync(new StateModel());

            // Act
            var result = await _stateController.GetStateById(stateId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }
    }
}
