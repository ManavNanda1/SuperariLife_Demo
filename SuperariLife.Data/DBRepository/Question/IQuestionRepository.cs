using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Question;


namespace SuperariLife.Data.DBRepository.Question
{
    public  interface IQuestionRepository
    {
        Task<long> DeleteQuestion(long questionId);
        Task<List<QuestionResponseModel>> GetQuestionListByAdmin(CommonPaginationModel info);
        Task<QuestionResponseModel> GetQuestionById(long questionId);
        Task<List<QuestionTypeResponseModel>> GetQuestionTypeListByAdmin();
        Task<List<QuestionResponseModel>> GetQuestionListForDropDownList();
        Task<long> InsertUpdateQuestion(QuestionReqModel questionInfo);
    }
}
