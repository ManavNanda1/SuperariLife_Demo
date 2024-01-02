using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.SettingPages;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.SettingPage.TestimonialReviewPage;

namespace SuperariLifeAPI.Areas.CustomerPortal.Controllers
{
    [Route("api/customer/about-us")]
    [ApiController]
 
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
        /// Get testimonial review List 
        /// </summary>
        /// <param ></param>
        /// <returns></returns>

        [HttpGet("testimonial-review-list")]
        public async Task<ApiResponse<TestimonialPagesReviewResponseModel>> GetTestimonialPagesReviewForCustomer( )
        {
            ApiResponse<TestimonialPagesReviewResponseModel> response = new ApiResponse<TestimonialPagesReviewResponseModel>() { Data = new List<TestimonialPagesReviewResponseModel>() };
            var result = await _testimonialReviewPageService.GetTestimonialPagesReviewForCustomer();
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

    }
}
