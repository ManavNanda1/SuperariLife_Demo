using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Question;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.Question;

namespace SuperariLifeAPI.Areas.Admin.Controllers
{
    [Route("api/admin/question")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        #region Fields
        private readonly IQuestionService _questionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        #endregion

        #region constructor
        public QuestionController(IQuestionService questionService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthenticationService jwtAuthenticationService
         )
        {
            _questionService = questionService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
        }
        #endregion

        /// <summary>
        /// Add Udoate Question
        /// </summary>
        /// <param name="QuestionReqModel"></param>
        /// <returns></returns>
        [HttpPost("save")]

        public async Task<BaseApiResponse> InsertUpdateQuestion([FromBody] QuestionReqModel model)
        {
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.UserId = tokenModel.Id;
            BaseApiResponse response = new BaseApiResponse();
            var result = await _questionService.InsertUpdateQuestion(model);
            if (result > StatusResult.Updated)
            {
                response.Message = ErrorMessages.SaveQuestionSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.Updated)
            {
                response.Message = ErrorMessages.UpdateQuestionSuccess;
                response.Success = true;
            }
            else if (result == StatusResult.AlreadyExists)
            {
                response.Message = ErrorMessages.QuestionExist;
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
        /// Get Question List 
        /// </summary>
        /// <param model="CommonPaginationModel" ></param>
        /// <returns></returns>

        [HttpPost("list")]
        public async Task<ApiResponse<QuestionResponseModel>> GetQuestionListByAdmin(CommonPaginationModel info)
        {
            ApiResponse<QuestionResponseModel> response = new ApiResponse<QuestionResponseModel>() { Data = new List<QuestionResponseModel>() };
            var result = await _questionService.GetQuestionListByAdmin(info);
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
        /// Get Question List for drop down
        /// </summary>
        /// <param  ></param>
        /// <returns></returns>
        [HttpGet("question-dropdown-list")]
        public async Task<ApiResponse<QuestionResponseModel>> GetQuestionListForDropDownList()
        {
            ApiResponse<QuestionResponseModel> response = new ApiResponse<QuestionResponseModel>() { Data = new List<QuestionResponseModel>() };
            var result = await _questionService.GetQuestionListForDropDownList();
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
        /// Get QuestionType List 
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        [HttpGet("question-type-list")]
        public async Task<ApiResponse<QuestionTypeResponseModel>> GetQuestionTypeListByAdmin( )
        {
            ApiResponse<QuestionTypeResponseModel> response = new ApiResponse<QuestionTypeResponseModel>() { Data = new List<QuestionTypeResponseModel>() };
            var result = await _questionService.GetQuestionTypeListByAdmin();
            if (result != null)
            {
                response.Data = result;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Get Question Details By ID 
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [HttpGet("get-question/{Id}")]
        public async Task<ApiPostResponse<QuestionResponseModel>> GetQuestionById(long Id)
        {
            ApiPostResponse<QuestionResponseModel> response = new ApiPostResponse<QuestionResponseModel>() { Data = new QuestionResponseModel() };

            var result = await _questionService.GetQuestionById(Id);
            if (result != null)
            {
                response.Data = result;
            }
            response.Success = true;
            return response;
        }

        /// <summary>
        /// Delete Question 
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [HttpPost("delete/{Id}")]
        public async Task<BaseApiResponse> DeleteQuestion(long Id)
        {
            BaseApiResponse response = new BaseApiResponse();
            var result = await _questionService.DeleteQuestion(Id);
            if (result == Status.InUse)
            {
                response.Message = ErrorMessages.QuestionInUse;
                response.Success = false;
            }
          else  if (result == Status.Success)
            {
                response.Message = ErrorMessages.DeleteQuestionSuccess;
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
