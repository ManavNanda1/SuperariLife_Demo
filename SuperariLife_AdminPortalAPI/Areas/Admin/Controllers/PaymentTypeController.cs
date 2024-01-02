using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.PaymentType;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.PaymentType;

namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/paymentType")]
    [ApiController]
    [Authorize]
    public class PaymentTypeController : ControllerBase
    {
        #region Fields
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        #endregion

        #region constructor
        public PaymentTypeController(IPaymentTypeService paymentTypeService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService
         )
        {
            _paymentTypeService = paymentTypeService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
        }
        #endregion

        /// <summary>
        /// Add Update PaymentType
        /// </summary>
        /// <param name="PaymentTypeReqModel"></param>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<BaseApiResponse> InsertUpdatePaymentType([FromBody] PaymentTypeReqModel model)
        {
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.UserId = tokenModel.Id;
            BaseApiResponse response = new BaseApiResponse();
            var result = await _paymentTypeService.InsertUpdatePaymentType(model);
            if (result > StatusResult.Updated)
            {
                response.Message = ErrorMessages.SavePaymentTypeSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.Updated)
            {
                response.Message = ErrorMessages.UpdatePaymentTypeSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.AlreadyExists)
            {
                response.Message = ErrorMessages.PaymentTypeExist;
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
        /// Get User List Of PaymentType
        /// </summary>
        /// <param model="CommonPaginationModel" ></param>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<ApiResponse<PaymentTypeResponseModel>> GetPaymentTypeListByAdmin(CommonPaginationModel info)
        {
            ApiResponse<PaymentTypeResponseModel> response = new ApiResponse<PaymentTypeResponseModel>() { Data = new List<PaymentTypeResponseModel>() };
            var result = await _paymentTypeService.GetPaymentTypeList(info);
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
        /// Get PaymentType Details By ID 
        /// </summary>
        /// <param name="paymentTypeId"></param>
        /// <returns></returns>
        [HttpGet("getpayment-type/{Id}")]
        public async Task<ApiPostResponse<PaymentTypeResponseModel>> GetPaymentTypeById(long Id)
        {
            ApiPostResponse<PaymentTypeResponseModel> response = new ApiPostResponse<PaymentTypeResponseModel>() { Data = new PaymentTypeResponseModel() };

            var result = await _paymentTypeService.GetPaymentTypeById(Id);
            if (result != null)
            {
                response.Data = result;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Delete PaymentType 
        /// </summary>
        /// <param name="paymentTypeId"></param>
        /// <returns></returns>
        /// 
        [HttpPost("delete/{Id}")]
        public async Task<BaseApiResponse> DeletePaymentType(long Id)
        {
            BaseApiResponse response = new BaseApiResponse();
            var result = await _paymentTypeService.DeletePaymentType(Id);
            if (result == Status.Success)
            {
                response.Message = ErrorMessages.DeletePaymentTypeSuccess;
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
