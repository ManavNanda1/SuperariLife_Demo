using SuperariLife.Data.DBRepository.SettingPage.PrivacyPolicyPage;
using SuperariLife.Model.SettingPages;

namespace SuperariLife.Service.SettingPage.PrivacyPolicyPage
{
    public class PrivacyPolicyPageService: IPrivacyPolicyPageService
    {
        #region Fields
        private readonly IPrivacyPolicyPageRepository _repository;
        #endregion

        #region Construtor
        public PrivacyPolicyPageService(IPrivacyPolicyPageRepository repository)
        {
            _repository = repository;
        }
        #endregion

        public async Task<List<PrivacyPageResponseModel>> GetPrivacyPage()
        {
            return await _repository.GetPrivacyPage();
        }

        public async Task<long> InsertUpdatePrivacyPage(PrivacyPageReqModel privacyPageInfo)
        {
           return await _repository.InsertUpdatePrivacyPage(privacyPageInfo);   
        }
    }
}
