using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Config;
using SuperariLife.Model.SettingPages;
using System.Data;


namespace SuperariLife.Data.DBRepository.SettingPage.TestimonialReviewPage
{
    public class TestimonialReviewPageRepository:BaseRepository,ITestimonialReviewPageRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public TestimonialReviewPageRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        public async Task<int> DeleteTestimonialPagesReview(int testimonialPageReviewId)
        {
            var param = new DynamicParameters();
            param.Add("@TestimonialPageReviewId", testimonialPageReviewId);
            return await QueryFirstOrDefaultAsync<int>(StoredProcedures.DeleteTestimonialPagesReview, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<TestimonialPagesReviewResponseModel> GetTestimonialPagesReviewById(int testimonialPageReviewId)
        {
            var param = new DynamicParameters();
            param.Add("@TestimonialPageReviewId", testimonialPageReviewId);
            return await QueryFirstOrDefaultAsync<TestimonialPagesReviewResponseModel>(StoredProcedures.GetTestimonialPagesReviewById, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<TestimonialPagesReviewResponseModel>> GetTestimonialPagesReviewList(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
            var data = await QueryAsync<TestimonialPagesReviewResponseModel>(StoredProcedures.GetTestimonialPagesReviewList, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }


        public async Task<long> InsertUpdateTestimonialPagesReview(TestimonialPagesReviewReqModel testimonialreviewInfo)
        {
            var param = new DynamicParameters();
            param.Add("@TestimonialPageReviewId", testimonialreviewInfo.TestimonialPageReviewId);
            param.Add("@TestimonialPageReviewContent", testimonialreviewInfo.TestimonialPageReviewContent.Trim());
            param.Add("@TestimonialPageReviewPersonName", testimonialreviewInfo.TestimonialPageReviewPersonName.Trim());
            param.Add("@UserId", testimonialreviewInfo.UserId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.InsertUpdateTestimonialPagesReview, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<TestimonialPagesReviewResponseModel>> GetTestimonialPagesReviewForCustomer()
        {
            var data = await QueryAsync<TestimonialPagesReviewResponseModel>(StoredProcedures.GetTestimonialPagesReviewForCustomer, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
    }
}
