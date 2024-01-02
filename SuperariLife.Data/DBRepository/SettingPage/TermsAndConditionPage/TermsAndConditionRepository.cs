using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Config;
using SuperariLife.Model.SettingPages;
using System.Data;

namespace SuperariLife.Data.DBRepository.SettingPage.TermsAndConditionPage
{
    public class TermsAndConditionRepository:BaseRepository,ITermsAndConditionRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public TermsAndConditionRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        public async  Task<List<TermsAndConditionPageResponseModel>> GetTermsAndConditionPage()
        {
            var data = await QueryAsync<TermsAndConditionPageResponseModel>(StoredProcedures.GetTermsAndConditionPage, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<long> InsertUpdateTermsAndConditionPage(TermsAndConditionPageReqModel termsAndConditionPageInfo)
        {
            var param = new DynamicParameters();
            param.Add("@TermsAndConditionPageId", termsAndConditionPageInfo.TermsAndConditionPageId);
            param.Add("@TermsAndConditionPageContent", termsAndConditionPageInfo.TermsAndConditionPageContent);
            param.Add("@UserId", termsAndConditionPageInfo.UserId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.InsertUpdateTermsAndConditionPage, param, commandType: CommandType.StoredProcedure);
        }

    }
}
