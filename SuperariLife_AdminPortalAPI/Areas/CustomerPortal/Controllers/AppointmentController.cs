using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Appointment;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Appointment;
using SuperariLife.Service.JWTAuthentication;


namespace SuperariLifeAPI.Areas.CustomerPortal.Controllers
{
    [Route("api/customer/appointment")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        #region Fields
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        private readonly SMTPSettings _smtpSettings;
        private readonly AppSettings _appSettings;
        #endregion

        #region Constructor
        public AppointmentController(IAppointmentService appointmentService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService,
           Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
           IConfiguration config,
           IOptions<SMTPSettings> smtpSettings,
           IOptions<AppSettings> appSettings
         )
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
            _hostingEnvironment = hostingEnvironment;
            _config = config;
            _smtpSettings = smtpSettings.Value;
            _appSettings = appSettings.Value;
        }
        #endregion

        /// <summary>
        /// Add update Appointment
        /// </summary>
        /// <param name="AppointmentReqModelByCustomer"></param>
        /// <returns></returns>
        [HttpPost("save")]

        public async Task<BaseApiResponse> InsertUpdateAppointmentByCustomer([FromBody] AppointmentReqModelByCustomer model)
        {
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.CustomerId = tokenModel.Id;
            BaseApiResponse response = new BaseApiResponse();
            var result = await _appointmentService.InsertUpdateAppointmentByCustomer(model);
            if (result > StatusResult.Updated)
            {
                response.Message = ErrorMessages.SaveAppointmentSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.Updated)
            {
                response.Message = ErrorMessages.UpdateAppointmentSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.AlreadyExists)
            {
                response.Message = ErrorMessages.AppointmentExist;
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
        /// Get appointment type List For Drop Down
        /// </summary>
        /// <param ></param>
        /// <returns></returns>

        [HttpGet("appointment-type-drop-down")]
        public async Task<ApiResponse<AppointmentResponseDropDownModel>> GetAppointmentTypeForDropDown()
        {
            ApiResponse<AppointmentResponseDropDownModel> response = new ApiResponse<AppointmentResponseDropDownModel>() { Data = new List<AppointmentResponseDropDownModel>() };
            var result = await _appointmentService.GetAppointmentTypeForDropDown();
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
        /// Get Appointment List For Admin
        /// </summary>
        /// <param model="CommonPaginationModel" ></param>
        /// <returns></returns>

        [HttpPost("appointment-list")]
        public async Task<ApiResponse<AppointmentResponseModelForCustomer>> GetAppointmentListByCustomer(CommonPaginationModel info)
        {
            ApiResponse<AppointmentResponseModelForCustomer> response = new ApiResponse<AppointmentResponseModelForCustomer>() { Data = new List<AppointmentResponseModelForCustomer>() };
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            info.CustomerId = tokenModel.Id;
            var result = await _appointmentService.GetAppointmentListByCustomer(info);
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
        /// Get appointment details by appointment Id 
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>

        [HttpGet("get-appointment-detail/{Id}")]
        public async Task<ApiPostResponse<AppointmentResponseModelForCustomer>> GetAppointmentDetailByIdForCustomer(long Id)
        {
            ApiPostResponse<AppointmentResponseModelForCustomer> response = new ApiPostResponse<AppointmentResponseModelForCustomer>() { Data = new AppointmentResponseModelForCustomer() };
            var result = await _appointmentService.GetAppointmentDetailByIdForCustomer(Id);
            if (result != null)
            {
                
                response.Data = result;
            }
            response.Success = true;
            return response;
        }


        /// <summary>
        /// Delete appointment 
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        [HttpPost("delete/{Id}")]
        public async Task<ApiPostResponse<long>> DeleteAppointmentByCustomer(long Id)
        {
            ApiPostResponse<long> response = new ApiPostResponse<long>();

            var result = await _appointmentService.DeleteAppointmentByCustomer(Id);
           
            if (result == Status.Success)
            {
                response.Message = ErrorMessages.DeleteAppointmentSuccess;
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
