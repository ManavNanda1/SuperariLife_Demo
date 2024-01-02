using SuperariLife.Data.DBRepository.SettingPage.AboutPage;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.SettingPages;

namespace SuperariLife.Service.SettingPage.AboutPage
{
    internal class AboutPageService: IAboutPageService
    {
        #region Fields
        private readonly IAboutPageRepository _repository;
        #endregion

        #region Construtor
        public AboutPageService(IAboutPageRepository repository)
        {
            _repository = repository;
        }
        #endregion


        #region AboutPage Image
        public async Task<CommonAboutPageDeleteModel> DeleteAboutPageImage(int aboutPageImageId)
        {
            return await _repository.DeleteAboutPageImage(aboutPageImageId);    
        }


        public async Task<AboutImageResponseModel> GetAboutPageImageById(int aboutPageImageId)
        {
           return await _repository.GetAboutPageImageById(aboutPageImageId);    
        }

        public async Task<List<AboutImageResponseModel>> GetAboutPageImageList()
        {
            return await _repository.GetAboutPageImageList();
        }
        public async Task<long> InsertUpdateAboutPageImage(AboutImageReqModel aboutPageImageInfo, List<string> aboutPageImageName)
        {
            return await _repository.InsertUpdateAboutPageImage(aboutPageImageInfo, aboutPageImageName);
        }



        #endregion

        #region About Page Section

        public async Task<CommonAboutPageDeleteModel> DeleteAboutPageSection(int aboutPageSectionId)
        {
            return await _repository.DeleteAboutPageSection(aboutPageSectionId);
        }

        public async Task<AboutPageSectionResponseModel> GetAboutPageSectionById(int aboutPageSectionId)
        {
            return await _repository.GetAboutPageSectionById((int)aboutPageSectionId);  
        }

        public async Task<List<PageSectionLayoutTypeModel>> GetAboutPageSectionLayoutTypeForDropDown()
        {
            return await _repository.GetAboutPageSectionLayoutTypeForDropDown();
        }

        public async Task<List<AboutPageSectionResponseModel>> GetAboutPageSectionList(CommonPaginationModel info)
        {
            return await _repository.GetAboutPageSectionList(info);
        }

        public async Task<AboutInsertUpdateResponseModel> InsertUpdateAboutPageSection(AboutPageSectionReqModel aboutPageSectionInfo)
        {
            return await _repository.InsertUpdateAboutPageSection(aboutPageSectionInfo);
        }

        public async Task<long> DeleteAboutSectionImage(long aboutSectionImageId)
        {
            return await _repository.DeleteAboutSectionImage(aboutSectionImageId);
        }

        public async  Task<List<AboutPageSectionResponseModel>> GetAboutPageSectionByCustomer()
        {
            return await _repository.GetAboutPageSectionByCustomer();   
        }
        #endregion
    }
}
