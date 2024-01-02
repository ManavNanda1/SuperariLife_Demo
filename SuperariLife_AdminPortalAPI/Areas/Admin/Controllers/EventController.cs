using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.CommonMethod;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Event;
using SuperariLife.Model.Token;
using SuperariLife.Service.Event;
using SuperariLife.Service.JWTAuthentication;


namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/event")]
    [ApiController]
    [Authorize]
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
        /// Add Update Event  
        /// </summary>
        /// <param name="EventReqModel"></param>
        /// <returns></returns>

        [HttpPost("save")]
        public async Task<BaseApiResponse> InsertUpdateEvent([FromForm] EventReqModel model)
        {
            List<EventGalleryImages> EventGalleryImageListName = new List<EventGalleryImages> { new EventGalleryImages() };
            BaseApiResponse response = new BaseApiResponse();
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);       
                if (model.EventImageFile != null)
                {
                    var path = _hostingEnvironment.WebRootPath + _config["Path:EventImagePath"] + model.EventName + "/";
                    model.EventImage = await CommonMethods.UploadImage(model.EventImageFile, path, model.EventName);
                   
                }
                if(model.GalleryImagesFile != null)
                {
                    if (model.GalleryImagesFile.Count > 0)
                    {

                        string gallerypath = _hostingEnvironment.WebRootPath + _config["Path:EventImagePath"] + model.EventName + "/" + model.EventName + "GalleryImage" + "/";
                        for (int i = 0; i < model.GalleryImagesFile.Count; i++)
                        {
                            EventGalleryImages galleryImage = new EventGalleryImages
                            {

                                EventGalleryImageName = await CommonMethods.UploadImage(model.GalleryImagesFile[i], gallerypath, model.EventName + (i))
                            };
                            EventGalleryImageListName.Add(galleryImage);
                        }
                    }
                }
                
            }
            model.CreatedBy = tokenModel.Id;

            var result = await _eventService.InsertUpdateEvent(model, EventGalleryImageListName);
            if (result > StatusResult.Updated)
            {
                response.Message = ErrorMessages.SaveEventSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.Updated)
            {
                response.Message = ErrorMessages.UpdateEventSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.AlreadyExists)
            {
                response.Message = ErrorMessages.EventExists;
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
        /// Get Event List 
        /// </summary>
        /// <param model="CommonPaginationModel"></param>
        /// <returns></returns>
        [HttpPost("event-list")]
        public async Task<ApiResponse<EventResponseModel>> GetEventListByAdmin([FromBody] CommonPaginationModel info)
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
        public async Task<ApiPostResponse<EventResponseModel>> GetEventDetailByIdForAdmin(long eventId)
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


        /// <summary>
        /// Get List of Event Question By Multiple Id passed as string ('1,2,3') 
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [HttpGet("event-question-list/{questionId}")]
        public async Task<ApiResponse<QuestionEventResponseModel>> GetEventDetailOfQuestion(string questionId)
        {
            ApiResponse<QuestionEventResponseModel> response = new ApiResponse<QuestionEventResponseModel>() { Data = new List<QuestionEventResponseModel>() };

            var result = await _eventService.GetEventDetailOfQuestion(questionId);
            if (result != null)
            {
                response.Data = result;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Get Event Details By ID For Customer By Admin
        /// </summary>
        /// <param model="EventCustomerReqModel"></param>
        /// <returns></returns>
        [HttpPost("get-event-customerdetail")]
        public async Task<ApiResponse<EventCustomerResponseModel>> GetEventDetailOfCustomerParticipant(EventCustomerReqModel eventInfo)
        {
            ApiResponse<EventCustomerResponseModel> response = new ApiResponse<EventCustomerResponseModel>() { Data = new List<EventCustomerResponseModel>() };
            var Path = Constants.https + HttpContext.Request.Host.Value;
            var result = await _eventService.GetEventDetailOfCustomerParticipant(eventInfo);

            if (result != null)
            {
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].CustomerImage != null)
                    {
                        result[i].CustomerImage = Path + _config["Path:CustomerProfileImagePath"] + result[i].CustomerEmail + '/' + result[i].CustomerImage;
                    }
                }
                response.Data = result;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Delete Event Question 
        /// </summary>
        /// <param name="eventQuestionId"></param>
        /// <returns></returns>
        [HttpPost("delete-event-quetion/{eventQuestionId}")]
        public async Task<BaseApiResponse> DeleteEventQuestion(long eventQuestionId)
        {
            BaseApiResponse response = new BaseApiResponse();
            var result = await _eventService.DeleteEventQuestion(eventQuestionId);
            if (result == Status.Success)
            {
                response.Message = ErrorMessages.DeleteEventQuestionSuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }
            return response;
        }

        /// <summary>
        /// Delete GalleryImage 
        /// </summary>
        /// <param name="galleryImageName"></param>
        /// <returns></returns>
        [HttpPost("delete-event-gallery-image/{galleryImageName}")]
        public async Task<BaseApiResponse> DeleteEventGalleryImage(string galleryImageName)
        {
            BaseApiResponse response = new BaseApiResponse();
            var result = await _eventService.DeleteEventGalleryImage(galleryImageName);
            if (result == Status.Success)
            {
                response.Message = ErrorMessages.DeleteGalleryImageSuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }
            return response;
        }

        /// <summary>
        /// Delete Event 
        /// </summary>
        /// <param model="CommonDeleteModel"></param>
        /// <returns></returns>
        [HttpPost("delete-event")]
        public async Task<BaseApiResponse> DeleteEvent(CommonDeleteModel eventDeleteInfo)
        {
            BaseApiResponse response = new BaseApiResponse();
           
            var result = await _eventService.DeleteEvent(eventDeleteInfo);
            if (result == Status.Success)
            {
                string Path = _hostingEnvironment.WebRootPath + _config["Path:EventImagePath"] + eventDeleteInfo.Name + "/";
                CommonMethods.DeleteDirectory(Path);
                response.Message = ErrorMessages.DeleteEventSuccess;
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
