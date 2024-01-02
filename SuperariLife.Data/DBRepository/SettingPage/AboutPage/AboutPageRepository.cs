using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Config;
using SuperariLife.Model.RoleManagement;
using SuperariLife.Model.SettingPages;
using SuperariLife.Model.User;
using System.Data;

namespace SuperariLife.Data.DBRepository.SettingPage.AboutPage
{
    public class AboutPageRepository:BaseRepository,IAboutPageRepository
    {

        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public AboutPageRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion


        #region AboutPageImage
        public async  Task<CommonAboutPageDeleteModel> DeleteAboutPageImage(int aboutPageImageId)
        {
            var param = new DynamicParameters();
            param.Add("@AboutPageImageId", aboutPageImageId);
            return await QueryFirstOrDefaultAsync<CommonAboutPageDeleteModel>(StoredProcedures.DeleteAboutPageImage, param, commandType: CommandType.StoredProcedure);
        }

      

        public  async Task<AboutImageResponseModel> GetAboutPageImageById(int aboutPageImageId)
        {
            var param = new DynamicParameters();
            param.Add("@AboutPageImageId", aboutPageImageId);
            return await QueryFirstOrDefaultAsync<AboutImageResponseModel>(StoredProcedures.GetAboutPageImageById, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<AboutImageResponseModel>> GetAboutPageImageList()
        {
            var data = await QueryAsync<AboutImageResponseModel>(StoredProcedures.GetAboutPageImageList, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

    

        public async Task<long> InsertUpdateAboutPageImage(AboutImageReqModel aboutPageImageInfo,List<string> aboutPageImagesName)
        {
            DataTable dtAboutPageImage = new DataTable("tbl_AboutPageImage");
            dtAboutPageImage.Columns.Add("AboutPageImage");
            if(aboutPageImageInfo.AboutPageImages.Count>0 && aboutPageImageInfo.AboutPageImages!=null && aboutPageImagesName != null && aboutPageImagesName.Count>0)
            {
                foreach(var imageName in aboutPageImagesName)
                {
                    DataRow dtRow = dtAboutPageImage.NewRow();
                    if (imageName != null || imageName != "")
                    {
                        dtRow["AboutPageImage"] = imageName;
                    }
                    dtAboutPageImage.Rows.Add(dtRow);
                }
            }
            var param = new DynamicParameters();
            param.Add("@AboutPageImageId", aboutPageImageInfo.AboutPageImageId);
            param.Add("@AboutPageImages",dtAboutPageImage.AsTableValuedParameter("[dbo].[tbl_AboutPageImage]"));
            param.Add("@UserId", aboutPageImageInfo.UserId);
            var result = await QueryFirstOrDefaultAsync<long>(StoredProcedures.InsertUpdateAboutPageImage, param, commandType: CommandType.StoredProcedure);
            return result;
        }


        #endregion


        #region About Pages Section
        public async  Task<CommonAboutPageDeleteModel> DeleteAboutPageSection(int aboutPageSectionId)
        {
            var param = new DynamicParameters();
            param.Add("@AboutPageSectionId", aboutPageSectionId);
            return await QueryFirstOrDefaultAsync<CommonAboutPageDeleteModel>(StoredProcedures.DeleteAboutPageSection, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<AboutPageSectionResponseModel> GetAboutPageSectionById(int aboutPageSectionId)
        {
            var param = new DynamicParameters();
            param.Add("@AboutPageSectionId", aboutPageSectionId);
            return await QueryFirstOrDefaultAsync<AboutPageSectionResponseModel>(StoredProcedures.GetAboutPageSectionById, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<PageSectionLayoutTypeModel>> GetAboutPageSectionLayoutTypeForDropDown()
        {
            var data = await QueryAsync<PageSectionLayoutTypeModel>(StoredProcedures.GetAboutPageSectionLayoutTypeForDropDown, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<List<AboutPageSectionResponseModel>> GetAboutPageSectionList(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
            var data = await QueryAsync<AboutPageSectionResponseModel>(StoredProcedures.GetAboutPageSectionList, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<AboutInsertUpdateResponseModel> InsertUpdateAboutPageSection(AboutPageSectionReqModel aboutPageSectionInfo)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", aboutPageSectionInfo.UserId);
            param.Add("@AboutPageSectionId", aboutPageSectionInfo.AboutPageSectionId);
            param.Add("@AboutPageSectionLayoutTypeId", aboutPageSectionInfo.AboutPageSectionLayoutTypeId);
            param.Add("@AboutPageSectionIsSetupFreeButtonEnable", aboutPageSectionInfo.AboutPageSectionIsSetupFreeButtonEnable);
            param.Add("@AboutPageSectionContent ", aboutPageSectionInfo.AboutPageSectionContent);
            param.Add("@AboutPageSectionContentTitle", aboutPageSectionInfo.AboutPageSectionContentTitle);
            param.Add("@AboutPageSectionImage", aboutPageSectionInfo.AboutPageSectionImageName);
            return await QueryFirstOrDefaultAsync<AboutInsertUpdateResponseModel>(StoredProcedures.InsertUpdateAboutPageSection, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> DeleteAboutSectionImage(long aboutSectionImageId)
        {
            var param = new DynamicParameters();
            param.Add("@AboutPageSectionId", aboutSectionImageId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.RemoveSectionAboutImage , param , commandType: CommandType.StoredProcedure);
        }

        public async Task<List<AboutPageSectionResponseModel>> GetAboutPageSectionByCustomer()
        {
            var data = await QueryAsync<AboutPageSectionResponseModel>(StoredProcedures.GetAboutPageSectionByCustomer, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        #endregion

    }
}
