using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.RoleManagement;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.RoleManagement;
using SuperariLife.Model.Token;

using SuperariLife.Service.JWTAuthentication;

namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/role")]
    [ApiController]
    [Authorize]
    public class RoleManagementController : ControllerBase
    {
        #region Fields
        private readonly IRoleManagementService _roleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        #endregion

        #region constructor
        public RoleManagementController(IRoleManagementService roleService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService
         )
        {
            _roleService = roleService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
        }
        #endregion

        /// <summary>
        /// Add Role
        /// </summary>
        /// <param name="RoleManagementReqModel"></param>
        /// <returns></returns>
        [HttpPost("save")]
       
        public async Task<BaseApiResponse> InsertUpdateRoleManagement([FromBody] RoleManagementReqModel model)
        {
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.UserId = tokenModel.Id;
            BaseApiResponse response = new BaseApiResponse();
            var result = await _roleService.InsertUpdateRoleManagement(model);
            if (result > StatusResult.Updated)
            {
                response.Message = ErrorMessages.SaveRoleSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.Updated)
            {
                response.Message = ErrorMessages.UpdateRoleSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.AlreadyExists)
            {
                response.Message = ErrorMessages.RoleExist;
                response.Success = false;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }

            return response;
        }

        /// <summary>
        /// Get Role List For Admin
        /// </summary>
        /// <param model="CommonPaginationModel" ></param>
        /// <returns></returns>
       
        [HttpPost("role-list")]
        public async Task<ApiResponse<RoleManagementResponseModel>> GetRoleListByAdmin(CommonPaginationModel info)
        {
            ApiResponse<RoleManagementResponseModel> response = new ApiResponse<RoleManagementResponseModel>() { Data = new List<RoleManagementResponseModel>() };
            var result = await _roleService.GetRoleManagementList(info);
            if (result.Count != 0)
            {
                response.Data = result;
            }
            else
            {
                response.Message = ErrorMessages.NoSuchRecordFound;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Get Role List For Drop Down
        /// </summary>
        /// <param ></param>
        /// <returns></returns>

        [HttpGet("role-list-drop-down")]
        public async Task<ApiResponse<RoleManagementResponseModel>> GetRoleListForDropDown()
        {
            ApiResponse<RoleManagementResponseModel> response = new ApiResponse<RoleManagementResponseModel>() { Data = new List<RoleManagementResponseModel>() };
            var result = await _roleService.GetRoleListForDropDown();
            if (result.Count != 0)
            {
                response.Data = result;
            }
            else
            {
                response.Message = ErrorMessages.NoSuchRecordFound;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Get Role Details By ID 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("getrole/{Id}")]
        public async Task<ApiPostResponse<RoleManagementResponseModel>> GetRoleManagementById(long Id)
        {
            ApiPostResponse<RoleManagementResponseModel> response = new ApiPostResponse<RoleManagementResponseModel>() { Data = new RoleManagementResponseModel() };

            var result = await _roleService.GetRoleManagementById(Id);
            if (result != null)
            {
                response.Data = result;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Delete Role 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost("delete/{Id}")]
        public async Task<ApiPostResponse<RoleManagementDeleteResponseModel>> DeleteRoleManagement(long Id)
        {
            ApiPostResponse<RoleManagementDeleteResponseModel> response = new ApiPostResponse<RoleManagementDeleteResponseModel>() { Data = new RoleManagementDeleteResponseModel() };

            var result = await _roleService.DeleteRoleManagement(Id);
            if(result.StatusOfRole == Status.InUse)
            {
                response.Message = result.RoleName + " " +ErrorMessages.RoleInUSe;
                response.Success = false;
            }
           else if (result.StatusOfRole == Status.Success)
            {
                response.Message = ErrorMessages.DeleteRoleSuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }
            return response;
        }
    }
}
