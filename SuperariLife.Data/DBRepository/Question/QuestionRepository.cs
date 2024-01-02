using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Config;
using SuperariLife.Model.Question;
using System.Data;


namespace SuperariLife.Data.DBRepository.Question
{
    public  class QuestionRepository:BaseRepository,IQuestionRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public QuestionRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        public  async Task<long> DeleteQuestion(long questionId)
        {
            var param = new DynamicParameters();
            param.Add("@QuestionId", questionId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.DeleteQuestion, param, commandType: CommandType.StoredProcedure);
        }
        public async  Task<QuestionResponseModel> GetQuestionById(long questionId)
        {
            var param = new DynamicParameters();
            param.Add("@QuestionId", questionId);
            return await QueryFirstOrDefaultAsync<QuestionResponseModel>(StoredProcedures.GetQuestionById, param, commandType: CommandType.StoredProcedure);
        }
        public  async Task<List<QuestionResponseModel>> GetQuestionListByAdmin(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
            var data = await QueryAsync<QuestionResponseModel>(StoredProcedures.GetQuestionListByAdmin, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        public  async Task<List<QuestionResponseModel>> GetQuestionListForDropDownList()
        {
            var data = await QueryAsync<QuestionResponseModel>(StoredProcedures.GetQuestionListForDropDownList, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        public async Task<List<QuestionTypeResponseModel>> GetQuestionTypeListByAdmin()
        {
            var data = await QueryAsync<QuestionTypeResponseModel>(StoredProcedures.GetQuestionTypeListByAdmin, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        public async Task<long> InsertUpdateQuestion(QuestionReqModel questionInfo)
        {
            DataTable dtQuestionOption = new DataTable("tbl_QueOpt");
            dtQuestionOption.Columns.Add("QuestionOption");
            if(questionInfo.QuestionOptionObj !=null && questionInfo.QuestionOptionObj.Count >0)
            {
                foreach (var item in questionInfo.QuestionOptionObj)
                {
                    DataRow dtRow = dtQuestionOption.NewRow();
                    dtRow["QuestionOption"] = item.QuestionOption.Trim();
                    dtQuestionOption.Rows.Add(dtRow);
                }
            }
            var param = new DynamicParameters();
            param.Add("@QuestionTypeId", questionInfo.QuestionTypeId);
            param.Add("@QuestionId", questionInfo.QuestionId);
            param.Add("@Question", questionInfo.Question);
            param.Add("@UserId", questionInfo.UserId);
            param.Add("@QuestionOption", dtQuestionOption.AsTableValuedParameter("[dbo].[tbl_QueOpt]"));
            var result = await QueryFirstOrDefaultAsync<int>(StoredProcedures.InsertUpdateQuestion, param, commandType: CommandType.StoredProcedure);
            return result;

        }
    }
}
