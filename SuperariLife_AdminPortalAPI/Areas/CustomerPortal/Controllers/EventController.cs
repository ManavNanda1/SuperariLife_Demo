using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Event;
using SuperariLife.Service.Event;
using SuperariLife.Service.JWTAuthentication;

namespace SuperariLifeAPI.Areas.CustomerPortal.Controllers
{
    [Route("api/customer/event")]
    [ApiController]

    public class EventController : ControllerBase
    {
        #region Fields
        private readonly IEventService _eventService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        #endregion

        #region Constructor
        public EventController(IEventService eventService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
           IConfiguration config
         )
        {
            _eventService = eventService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
            _hostingEnvironment = hostingEnvironment;
            _config = config;
        }
        #endregion


        /// <summary>
        /// Get Event List 
        /// </summary>
        /// <param model="CommonPaginationModel"></param>
        /// <returns></returns>
        [HttpPost("event-list")]
        public async Task<ApiResponse<EventResponseModel>> GetEventListForCustomer([FromBody] CommonPaginationModel info)
        {
            var Path = Constants.https + HttpContext.Request.Host.Value;
            ApiResponse<EventResponseModel> response = new ApiResponse<EventResponseModel>() { Data = new List<EventResponseModel>() };
            var result = await _eventService.GetEventListByAdmin(info);
            if (result.Count != 0)
            {
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].EventImage != null)
                    {
                        result[i].EventImage = Path + _config["Path:EventImagePath"] + result[i].EventName + '/' + result[i].EventImage;
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
        /// Get Event Details By ID   
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet("event-detail/{eventId}")]
        public async Task<ApiPostResponse<EventResponseModel>> GetEventDetailByIdForCustomer(long eventId)
        {
            var Path = Constants.https + HttpContext.Request.Host.Value;


            ApiPostResponse<EventResponseModel> response = new ApiPostResponse<EventResponseModel>() { Data = new EventResponseModel() };

            var result = await _eventService.GetEventByIdForAdmin(eventId);
            List<EventGalleryImages> galleryImageTempList = new List<EventGalleryImages>();
            if (result != null)
            {
                if (result.EventImage != null)
                {
                    result.EventImage = Path + _config["Path:EventImagePath"] + result.EventName + '/' + result.EventImage;
                }
                if (result.EventGalleryImages != null)
                {
                    string[] galleryImageName = result.EventGalleryImages.Split("##^&");


                    for (int i = 0; i < galleryImageName.Length; i++)
                    {
                        galleryImageTempList.Add(new EventGalleryImages
                        {
                            EventGalleryImagePath = Path + _config["Path:EventImagePath"] + result.EventName + "/" + result.EventName + "GalleryImage" + "/" + galleryImageName[i],
                            EventGalleryImageName = galleryImageName[i]
                        });

                    }
                }

                result.galleryImagesPath = galleryImageTempList;
                response.Data = result;
            }
            response.Success = true;
            return response;
        }

    }
}
