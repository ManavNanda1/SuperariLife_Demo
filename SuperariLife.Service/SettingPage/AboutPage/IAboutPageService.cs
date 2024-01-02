

using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.SettingPages;

namespace SuperariLife.Service.SettingPage.AboutPage
{
    public interface IAboutPageService
    {
        #region AboutPage Image
        Task<CommonAboutPageDeleteModel> DeleteAboutPageImage(int aboutPageImageId);
        Task<List<AboutImageResponseModel>> GetAboutPageImageList();
        Task<AboutImageResponseModel> GetAboutPageImageById(int aboutPageImageId);
        Task<long> InsertUpdateAboutPageImage(AboutImageReqModel aboutPageImageInfo,List<string> aboutPageImageName);
        #endregion


        #region AboutSection
        Task<CommonAboutPageDeleteModel> DeleteAboutPageSection(int aboutPageSectionId);
        Task<List<PageSectionLayoutTypeModel>> GetAboutPageSectionLayoutTypeForDropDown();
        Task<List<AboutPageSectionResponseModel>> GetAboutPageSectionList(CommonPaginationModel info);
        Task<List<AboutPageSectionResponseModel>> GetAboutPageSectionByCustomer();
        Task<AboutPageSectionResponseModel> GetAboutPageSectionById(int aboutPageSectionId);
        Task<AboutInsertUpdateResponseModel> InsertUpdateAboutPageSection(AboutPageSectionReqModel aboutPageSectionInfo);
        Task<long> DeleteAboutSectionImage(long aboutSectionImageId);
        #endregion
    }
}
