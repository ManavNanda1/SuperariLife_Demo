using SuperariLife.Data.DBRepository.SettingPage.TermsAndConditionPage;
using SuperariLife.Model.SettingPages;

namespace SuperariLife.Service.SettingPage.TermsAndConditionPage
{
    public class TermsAndConditionPageService: ITermsAndConditionPageService
    {
        #region Fields
        private readonly ITermsAndConditionRepository _repository;
        #endregion

        #region Construtor
        public TermsAndConditionPageService(ITermsAndConditionRepository repository)
        {
            _repository = repository;
        }
        #endregion

        public async Task<List<TermsAndConditionPageResponseModel>> GetTermsAndConditionPage()
        {
            return await _repository.GetTermsAndConditionPage();  
        }

        public async Task<long> InsertUpdateTermsAndConditionPage(TermsAndConditionPageReqModel termsAndConditionPageInfo)
        {
            return await _repository.InsertUpdateTermsAndConditionPage(termsAndConditionPageInfo);
        }
    }
}
