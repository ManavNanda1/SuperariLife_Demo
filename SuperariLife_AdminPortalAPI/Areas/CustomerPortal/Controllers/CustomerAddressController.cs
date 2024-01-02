using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Customer.CustomerAddress;
using SuperariLife.Model.Token;
using SuperariLife.Service.Customer.CustomerAddress;
using SuperariLife.Service.JWTAuthentication;


namespace SuperariLifeAPI.Areas.CustomerPortal.Controllers
{
    [Route("api/customer/customer-address")]
    [ApiController]
    [Authorize]
    public class CustomerAddressController : ControllerBase
    {
        #region Fields
        private readonly ICustomerAddressService _customerAddressService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        #endregion

        #region Constructor
        public CustomerAddressController(ICustomerAddressService customerAddressService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService,
           Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment
         )
        {
            _customerAddressService = customerAddressService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion

        [HttpPost("save")]
        public async Task<ApiPostResponse<CustomerAddressInsertUpdateResponseModel>> InsertUpdateCustomerAddressByCustomer([FromBody] CustomerAddressReqModelForAdmin model)
        {
            ApiPostResponse<CustomerAddressInsertUpdateResponseModel> response = new ApiPostResponse<CustomerAddressInsertUpdateResponseModel>() { Data = new CustomerAddressInsertUpdateResponseModel() };

        
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.CustomerId = tokenModel.Id;
            model.UserId = 0;

            var result = await _customerAddressService.InsertUpdateCustomerAddressByCustomer(model);
            if (result.StatusOfInsertUpdate > StatusResult.Updated)
            {
               
                response.Message = ErrorMessages.SaveCustomerAddressSuccess;
                response.Success = true;
            }
            else if (result.StatusOfInsertUpdate == StatusResult.Updated)
            {

                response.Message = ErrorMessages.UpdateCustomerAddressSuccess;
                response.Success = true;
            }
            else if (result.StatusOfInsertUpdate == StatusResult.AlreadyExists)
            {
 
                response.Message = ErrorMessages.CustomerAddressAlreadyExists;
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
        /// Get customer address list 
        /// </summary>
        /// <param name="CommonPaginationModel"></param>
        /// <returns></returns>

        [HttpPost("list")]
        public async Task<ApiResponse<CustomerAddressResponseModel>> GetCustomerAddressList([FromBody] CommonPaginationModel model)
        {
            ApiResponse<CustomerAddressResponseModel> response = new ApiResponse<CustomerAddressResponseModel>() { Data = new List<CustomerAddressResponseModel>() };
            var result = await _customerAddressService.GetCustomerAddressList(model);

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
        /// Get Customer Address Detail
        /// </summary>
        /// <param name="customerAddressId"></param>
        /// <returns></returns>

        [HttpGet("get-customer-address/{Id}")]
        public async Task<ApiPostResponse<CustomerAddressResponseModel>> GetCustomerAddressById(long Id)
        {
            ApiPostResponse<CustomerAddressResponseModel> response = new ApiPostResponse<CustomerAddressResponseModel>() { Data = new CustomerAddressResponseModel() };

            var result = await _customerAddressService.GetCustomerAddressById(Id);
            if (result != null)
            {
                response.Data = result;
            }
            response.Success = true;
            return response;
        }


        /// <summary>
        /// Delete Customer Address 
        /// </summary>
        /// <param name="customerAddressId"></param>
        /// <returns></returns>
        /// 
        [HttpPost("delete/{Id}")]
        public async Task<BaseApiResponse> DeletePaymentType(int Id)
        {
            BaseApiResponse response = new BaseApiResponse();
            var result = await _customerAddressService.DeleteCustomerAddress(Id);
            if (result == Status.Success)
            {
                response.Message = ErrorMessages.DeleteTestimonialReviewSuccess;
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
