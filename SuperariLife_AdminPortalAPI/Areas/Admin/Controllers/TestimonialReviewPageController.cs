using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.SettingPages;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.TestimonialReviewPage;

namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/testimonial-review")]
    [ApiController]
    [Authorize]
    public class TestimonialReviewPageController : ControllerBase
    {
        #region Fields
        private readonly ITestimonialReviewPageService _testimonialReviewPageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        #endregion

        #region constructor
        public TestimonialReviewPageController(ITestimonialReviewPageService testimonialReviewPageService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService
         )
        {
            _testimonialReviewPageService = testimonialReviewPageService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
        }
        #endregion

        /// <summary>
        /// Add Update Testimonial Review
        /// </summary>
        /// <param name="TestimonialPagesReviewReqModel"></param>
        /// <returns></returns>
        [HttpPost("save")]

        public async Task<BaseApiResponse> InsertUpdateTestimonialPagesReview([FromBody] TestimonialPagesReviewReqModel model)
        {
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.UserId = tokenModel.Id;
            BaseApiResponse response = new BaseApiResponse();
            var result = await _testimonialReviewPageService.InsertUpdateTestimonialPagesReview(model);
            if (result > StatusResult.Updated)
            {
                response.Message = ErrorMessages.SaveTestimonialReviewSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.Updated)
            {
                response.Message = ErrorMessages.UpdateTestimonialReviewSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.AlreadyExists)
            {
                response.Message = ErrorMessages.TestimonialReviewExists;
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
        /// Get testimonial review List for admin
        /// </summary>
        /// <param model="CommonPaginationModel" ></param>
        /// <returns></returns>

        [HttpPost("testimonial-review-list")]
        public async Task<ApiResponse<TestimonialPagesReviewResponseModel>> GetTestimonialPagesReviewList(CommonPaginationModel info)
        {
            ApiResponse<TestimonialPagesReviewResponseModel> response = new ApiResponse<TestimonialPagesReviewResponseModel>() { Data = new List<TestimonialPagesReviewResponseModel>() };
            var result = await _testimonialReviewPageService.GetTestimonialPagesReviewList(info);
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
        /// Get testimonial review By Id
        /// </summary>
        /// <param name="testimonialReviewId"></param>
        /// <returns></returns>
        [HttpGet("testimonial-review/{Id}")]
        public async Task<ApiPostResponse<TestimonialPagesReviewResponseModel>> GetTestimonialPagesReviewById(int Id)
        {
            ApiPostResponse<TestimonialPagesReviewResponseModel> response = new ApiPostResponse<TestimonialPagesReviewResponseModel>() { Data = new TestimonialPagesReviewResponseModel() };

            var result = await _testimonialReviewPageService.GetTestimonialPagesReviewById(Id);
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
        /// <param name="testimonialReviewId"></param>
        /// <returns></returns>
        /// 
        [HttpPost("delete/{testimonialReviewId}")]
        public async Task<BaseApiResponse> DeletePaymentType(int testimonialReviewId)
        {
            BaseApiResponse response = new BaseApiResponse();
            var result = await _testimonialReviewPageService.DeleteTestimonialPagesReview(testimonialReviewId);
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
