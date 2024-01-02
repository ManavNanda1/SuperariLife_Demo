using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Config;
using SuperariLife.Model.SettingPages;
using System.Data;


namespace SuperariLife.Data.DBRepository.SettingPage.PrivacyPolicyPage
{
    public class PrivacyPolicyPageRepository:BaseRepository,IPrivacyPolicyPageRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public PrivacyPolicyPageRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        public async Task<List<PrivacyPageResponseModel>> GetPrivacyPage()
        {
            var data = await QueryAsync<PrivacyPageResponseModel>(StoredProcedures.GetPrivacyPage, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<long> InsertUpdatePrivacyPage(PrivacyPageReqModel privacyPageInfo)
        {
            var param = new DynamicParameters();
            param.Add("@PrivacyPageId", privacyPageInfo.PrivacyPageId);
            param.Add("@PrivacyPageContent", privacyPageInfo.PrivacyPageContent);
            param.Add("@UserId", privacyPageInfo.UserId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.InsertUpdatePrivacyPage, param, commandType: CommandType.StoredProcedure);
        }
    }
}
