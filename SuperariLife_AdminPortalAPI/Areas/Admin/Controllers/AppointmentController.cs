
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

namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/appointment")]
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
        /// update Appointment
        /// </summary>
        /// <param name="AppointmentReqModelByAdmin"></param>
        /// <returns></returns>
        [HttpPost("save")]

        public async Task<BaseApiResponse> InsertUpdateAppointmentByAdmin([FromBody] AppointmentReqModelByAdmin model)
        {
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.UserId = tokenModel.Id;
            BaseApiResponse response = new BaseApiResponse();
            var result = await _appointmentService.InsertUpdateAppointmentByAdmin(model);
            if (result > StatusResult.Updated)
            {
                response.Message = ErrorMessages.SaveAppointmentSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.Updated)
            {
                if(model.IsAppointmentAccepted== AppointmentStatus.Accepted)
                {
                    response.Message= ErrorMessages.AppointmentAccepted;
                }
                else if(model.IsAppointmentAccepted== AppointmentStatus.Rejected)
                {
                    response.Message = ErrorMessages.AppointmentRejected;
                }
                else
                {
                    response.Message = ErrorMessages.AppointmentPending;
                }
            
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
        public async Task<ApiResponse<AppointmentResponseModelForAdmin>> GetAppointmentListByAdmin(CommonPaginationModel info)
        {
            ApiResponse<AppointmentResponseModelForAdmin> response = new ApiResponse<AppointmentResponseModelForAdmin>() { Data = new List<AppointmentResponseModelForAdmin>() };
            var Path = Constants.https + HttpContext.Request.Host.Value;
            var result = await _appointmentService.GetAppointmentListByAdmin(info);
            if (result.Count != 0)
            {
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].CustomerImage != null)
                    {
                        result[i].CustomerImage = Path + _config["Path:CustomerProfileImagePath"] + '/' + result[i].CustomerImage;
                    }

                }
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
        /// Get appointment details by appointment Id for approval dissapproval 
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>

        [HttpGet("get-appointment-detail/{Id}")]
        public async Task<ApiPostResponse<AppointmentResponseModelForAdmin>> GetAppointmentDetailByIdForAdmin(long Id)
        {
            ApiPostResponse<AppointmentResponseModelForAdmin> response = new ApiPostResponse<AppointmentResponseModelForAdmin>() { Data = new AppointmentResponseModelForAdmin() };
            var Path = Constants.https + HttpContext.Request.Host.Value;

            var result = await _appointmentService.GetAppointmentDetailByIdForAdmin(Id);
            if (result != null)
            {
                if (result.CustomerImage != null)
                {
                    result.CustomerImage = Path + _config["Path:CustomerProfileImagePath"] + '/' + result.CustomerImage;
                }
                response.Data = result;
            }
            response.Success = true;
            return response;
        }
    }
}
