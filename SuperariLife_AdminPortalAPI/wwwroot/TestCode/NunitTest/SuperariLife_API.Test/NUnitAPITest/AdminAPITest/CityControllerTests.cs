using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.City;
using SuperariLife.Model.City;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.Admin.Controllers;

[TestFixture]
public class CityControllerTests
{
    private CityController _cityController;
    private Mock<ICityService> _cityServiceMock;
    private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;

    [SetUp]
    public void Setup()
    {
        _cityServiceMock = new Mock<ICityService>();
        _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        _cityController = new CityController(
            _jwtAuthenticationServiceMock.Object,
            _httpContextAccessorMock.Object,
            _cityServiceMock.Object
        );
    }

    [Test]
    public async Task AddUpdateCity_ReturnsSuccess()
    {
        // Arrange
        var cityRequestModel = new CityRequestModel();
        var expectedResult = new ApiPostResponse<int> { Success = true };

        var jwtToken = "valid-jwt-token";
        _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
            .Returns(new StringValues($"Bearer {jwtToken}"));

        _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
            .Returns(new TokenModel { Id = 1 });

        _cityServiceMock.Setup(x => x.AddUpdateCity(It.IsAny<CityRequestModel>()))
            .ReturnsAsync(Status.Success);

        // Act
        var result = await _cityController.AddUpdateCity(cityRequestModel);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResult.Success, result.Success);
    }

    [Test]
    public async Task DeleteCity_ReturnsSuccess()
    {
        // Arrange
        var cityId = 1;
        var expectedResult = new ApiPostResponse<int> { Success = true };

        _cityServiceMock.Setup(x => x.DeleteCity(cityId))
            .ReturnsAsync(Status.Success);

        // Act
        var result = await _cityController.DeleteCity(cityId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResult.Success, result.Success);
    }

    [Test]
    public async Task GetCityListByStateId_ReturnsSuccess()
    {
        // Arrange
        var stateId = 1;
        var expectedResult = new ApiResponse<CityModel> { Success = true };

        _cityServiceMock.Setup(x => x.GetCityList(stateId))
            .ReturnsAsync(new List<CityModel>());

        // Act
        var result = await _cityController.GetCityListByStateId(stateId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResult.Success, result.Success);
    }

    [Test]
    public async Task GetCityById_ReturnsSuccess()
    {
        // Arrange
        var cityId = 1;
        var expectedResult = new ApiPostResponse<CityModel> { Success = true };

        _cityServiceMock.Setup(x => x.GetCityById(cityId))
            .ReturnsAsync(new CityModel());

        // Act
        var result = await _cityController.GetCityById(cityId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResult.Success, result.Success);
    }
}
