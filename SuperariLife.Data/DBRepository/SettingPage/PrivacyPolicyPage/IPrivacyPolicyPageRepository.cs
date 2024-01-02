using SuperariLife.Model.SettingPages;

namespace SuperariLife.Data.DBRepository.SettingPage.PrivacyPolicyPage
{
    public interface IPrivacyPolicyPageRepository
    {
        Task<List<PrivacyPageResponseModel>> GetPrivacyPage();
        Task<long> InsertUpdatePrivacyPage(PrivacyPageReqModel privacyPageInfo);
    }
}
