using SuperariLife.Model.SettingPages;

namespace SuperariLife.Service.SettingPage.PrivacyPolicyPage
{
    public interface IPrivacyPolicyPageService
    {
        Task<List<PrivacyPageResponseModel>> GetPrivacyPage();
        Task<long> InsertUpdatePrivacyPage(PrivacyPageReqModel privacyPageInfo);
    }
}
