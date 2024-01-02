
using SuperariLife.Model.SettingPages;

namespace SuperariLife.Data.DBRepository.SettingPage.TermsAndConditionPage
{
    public interface ITermsAndConditionRepository
    {
        Task<List<TermsAndConditionPageResponseModel>> GetTermsAndConditionPage();
        Task<long> InsertUpdateTermsAndConditionPage(TermsAndConditionPageReqModel termsAndConditionPageInfo);
    }
}
