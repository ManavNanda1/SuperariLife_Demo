using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.City;
using SuperariLife.Model.City;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;


namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/city")]
    [ApiController]
    public class CityController : ControllerBase
    {
        #region Fields
        private ICityService _cityService;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public CityController(
            IJWTAuthenticationService jwtAuthenticationService,
            IHttpContextAccessor httpContextAccessor,
            ICityService cityService
            )
        {
            _cityService = cityService;
            _jwtAuthenticationService = jwtAuthenticationService;
            _httpContextAccessor = httpContextAccessor; 
        }
        #endregion

        #region Methods

        /// <summary>
        ///  Insert Update City
        /// </summary>
        /// <param name="CityRequestModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("addUpdate")]
        public async Task<ApiPostResponse<int>> AddUpdateCity([FromBody] CityRequestModel city)
        {
            ApiPostResponse<int> response = new ApiPostResponse<int>();
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            city.UpdatedBy = tokenModel.Id;
            var result = await _cityService.AddUpdateCity(city);

            if (result == Status.Success)
            {
                response.Message = ErrorMessages.UpdateCitySuccess;
                response.Success = true;
            }
            else if (result > Status.Success)
            {
                response.Message = ErrorMessages.AddCitySuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.ErrorCity;
                response.Success = false;
            }
            return response;
        }

        /// <summary>
        ///  Delete City
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpPost("delete/{cityId}")]
        public async Task<ApiPostResponse<int>> DeleteCity(int cityId)
        {
            ApiPostResponse<int> response = new ApiPostResponse<int>();
            var result = await _cityService.DeleteCity(cityId);

            if (result == Status.Success)
            {
                response.Message = ErrorMessages.DeleteCitySuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.ErrorCity;
                response.Success = false;
            }
            return response;

        }

        /// <summary>
        ///  Get city list by stateId
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns>CityList</returns>
        [HttpPost("list/{stateId}")]
        public async Task<ApiResponse<CityModel>> GetCityListByStateId(long stateId)
        {
            ApiResponse<CityModel> response = new ApiResponse<CityModel>() { Data = new List<CityModel>() };
            var result = await _cityService.GetCityList(stateId);
            if (result != null)
            {
                response.Data = result;
            }
            response.Success = true;
            return response;

        }

        /// <summary>
        ///  Get city info by cityId
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>cityInfo</returns>
        [HttpPost("city-list-id/{cityId}")]
        public async Task<ApiPostResponse<CityModel>> GetCityById(long cityId)
        {
            ApiPostResponse<CityModel> response = new ApiPostResponse<CityModel>();
            var result = await _cityService.GetCityById(cityId);
            if (result != null)
            {
                response.Data = result;
            }
            response.Success = true;
            return response;

        }   
        #endregion

    }
}
