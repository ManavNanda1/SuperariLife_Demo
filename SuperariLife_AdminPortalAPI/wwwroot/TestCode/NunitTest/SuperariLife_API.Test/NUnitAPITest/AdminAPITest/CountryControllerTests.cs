using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.Country;
using SuperariLife.Model.Country;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.Admin.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class CountryControllerTests
    {
        private Mock<ICountryService> _countryServiceMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private CountryController _countryController;

        [SetUp]
        public void Setup()
        {
            _countryServiceMock = new Mock<ICountryService>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _countryController = new CountryController(
                _countryServiceMock.Object,
                _jwtAuthenticationServiceMock.Object,
                _httpContextAccessorMock.Object
            );
        }

        [Test]
        public async Task AddUpdateCountry_ReturnsSuccess()
        {
            // Arrange
            var countryRequestModel = new CountryRequestModel();
            var expectedResult = new ApiPostResponse<int> { Success = true };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns("Bearer YourTokenHere");

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
                .Returns(new TokenModel { Id = 1 });

            _countryServiceMock.Setup(x => x.InsertUpdateCountry(It.IsAny<CountryRequestModel>()))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _countryController.AddUpdateCountry(countryRequestModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task DeleteCountry_ReturnsSuccess()
        {
            // Arrange
            const int countryId = 1;
            var expectedResult = new ApiPostResponse<int> { Success = true };

            _countryServiceMock.Setup(x => x.DeleteCountry(countryId))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _countryController.DeleteCountry(countryId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task GetCountry_ReturnsCountryList()
        {
            // Arrange
            var expectedResult = new ApiResponse<CountryModel> { Success = true, Data = new List<CountryModel>() };

            _countryServiceMock.Setup(x => x.GetCountryList())
                .ReturnsAsync(new List<CountryModel>());

            // Act
            var result = await _countryController.GetCountry();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetCountryById_ReturnsCountryInfo()
        {
            // Arrange
            const int countryId = 1;
            var expectedResult = new ApiPostResponse<CountryModel> { Success = true, Data = new CountryModel() };

            _countryServiceMock.Setup(x => x.GetCountryById(countryId))
                .ReturnsAsync(new CountryModel());

            // Act
            var result = await _countryController.GetCountryById(countryId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }
    }
}
