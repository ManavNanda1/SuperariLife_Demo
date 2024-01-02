using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.Country;
using SuperariLife.Model.Country;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;


namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/country")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        #region Fields
        private ICountryService _countryService;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public CountryController(ICountryService countryService,
            IJWTAuthenticationService jwtAuthenticationService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _countryService = countryService;
            _jwtAuthenticationService = jwtAuthenticationService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods

        /// <summary>
        ///  Add Update countryInfo 
        /// </summary>
        /// <param model="CountryRequestModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("add-update")]
        public async Task<ApiPostResponse<int>> AddUpdateCountry([FromBody] CountryRequestModel country)
        {
            ApiPostResponse<int> response = new ApiPostResponse<int>();
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            country.UpdatedBy = tokenModel.Id;
            var result = await _countryService.InsertUpdateCountry(country);

            if (result == Status.Success)
            {
                response.Message = ErrorMessages.UpdateCountrySuccess;
                response.Success = true;
            }
            else if (result > Status.Success)
            {
                response.Message = ErrorMessages.AddCountrySuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.ErrorCountry;
                response.Success = false;
            }
            return response;
        }


        /// <summary>
        /// Delete countryInfo 
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("delete/{countryId}")]
        public async Task<ApiPostResponse<int>> DeleteCountry(int countryId)
        {
            ApiPostResponse<int> response = new ApiPostResponse<int>();
            var result = await _countryService.DeleteCountry(countryId);

            if (result == Status.Success)
            {
                response.Message = ErrorMessages.DeleteCountrySuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.ErrorCountry;
                response.Success = false;
            }
            return response;

        }

        /// <summary>
        ///  Get country list  
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ApiResponse<CountryModel>> GetCountry()
        {
            ApiResponse<CountryModel> response = new ApiResponse<CountryModel>() { Data = new List<CountryModel>() };
            var result = await _countryService.GetCountryList();
            if (result != null)
            {
                response.Data = result;
            }
            response.Success = true;
            return response;
        }


        /// <summary>
        ///  Get countryInfo by countryId  
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpPost("countryById/{countryId}")]
        public async Task<ApiPostResponse<CountryModel>> GetCountryById(int countryId)
        {
            ApiPostResponse<CountryModel> response = new ApiPostResponse<CountryModel>();
            var result = await _countryService.GetCountryById(countryId);
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
