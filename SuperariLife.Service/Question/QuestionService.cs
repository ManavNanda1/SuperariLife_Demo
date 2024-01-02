using SuperariLife.Data.DBRepository.Question;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Question;

namespace SuperariLife.Service.Question
{
    public class QuestionService:IQuestionService
    {
        #region Fields
        private readonly IQuestionRepository _repository;
        #endregion

        #region Construtor
        public QuestionService(IQuestionRepository repository)
        {
            _repository = repository;
        }
        #endregion

        public async Task<long> DeleteQuestion(long questionId)
        {
           return await _repository.DeleteQuestion(questionId);
        }

        public async Task<QuestionResponseModel> GetQuestionById(long questionId)
        {
           return await _repository.GetQuestionById(questionId);
        }

        public async Task<List<QuestionResponseModel>> GetQuestionListByAdmin(CommonPaginationModel info)
        {
           return await _repository.GetQuestionListByAdmin(info);
        }

        public async Task<List<QuestionResponseModel>> GetQuestionListForDropDownList()
        {
            return await _repository.GetQuestionListForDropDownList();  
        }

        public async Task<List<QuestionTypeResponseModel>> GetQuestionTypeListByAdmin()
        {
            return await _repository.GetQuestionTypeListByAdmin();  
        }

        public async Task<long> InsertUpdateQuestion(QuestionReqModel questionInfo)
        {
            return await _repository.InsertUpdateQuestion(questionInfo);    
        }
    }
}
