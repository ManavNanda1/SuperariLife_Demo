using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.State;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.State;

namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/state")]
    [ApiController]
    public class StateController : ControllerBase
    {
        #region Fields
        private IStateService _stateService;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor

        public StateController(
            IJWTAuthenticationService jwtAuthenticationService,
            IHttpContextAccessor httpContextAccessor,
            IStateService stateService
            )
        {
            _stateService = stateService;
            _jwtAuthenticationService = jwtAuthenticationService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods

        /// <summary>
        ///  Insert Update stateinfo  
        /// </summary>
        /// <param model="StateRequestModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("addUpdate")]
        public async Task<ApiPostResponse<int>> AddUpdateState([FromBody] StateRequestModel state)
        {
            ApiPostResponse<int> response = new ApiPostResponse<int>();
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            state.UpdatedBy = tokenModel.Id;
            var result = await _stateService.AddUpdateState(state);

            if (result == Status.Success)
            {
                response.Message = ErrorMessages.UpdateStateSuccess;
                response.Success = true;
            }
            else if (result > Status.Success)
            {
                response.Message = ErrorMessages.AddStateSuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.ErrorState;
                response.Success = false;
            }
            return response;
        }

        /// <summary>
        ///  Delete state
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpPost("delete/{stateId}")]
        public async Task<ApiPostResponse<int>> DeleteState(int stateId)
        {
            ApiPostResponse<int> response = new ApiPostResponse<int>();
            var result = await _stateService.DeleteState(stateId);

            if (result == Status.Success)
            {
                response.Message = ErrorMessages.DeleteStateSuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.ErrorState;
                response.Success = false;
            }
            return response;

        }


        /// <summary>
        ///  Get state list by countryId
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpPost("list/{countryId}")]
        public async Task<ApiResponse<StateModel>> GetStateListByCountryId(int countryId)
        {
            ApiResponse<StateModel> response = new ApiResponse<StateModel>() { Data = new List<StateModel>() };
            var result = await _stateService.GetStateList(countryId);
            {
                response.Data = result;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        ///  Get stateinfo  by stateId
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpPost("statebyid/{stateId}")]
        public async Task<ApiPostResponse<StateModel>> GetStateById(int stateId)
        {
            ApiPostResponse<StateModel> response = new ApiPostResponse<StateModel>();
            var result = await _stateService.GetStateListById(stateId);
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
