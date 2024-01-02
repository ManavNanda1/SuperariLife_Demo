using SuperariLife.Model.SettingPages;


namespace SuperariLife.Service.SettingPage.TermsAndConditionPage
{
    public interface ITermsAndConditionPageService
    {
        Task<List<TermsAndConditionPageResponseModel>> GetTermsAndConditionPage();
        Task<long> InsertUpdateTermsAndConditionPage(TermsAndConditionPageReqModel termsAndConditionPageInfo);
    }
}
