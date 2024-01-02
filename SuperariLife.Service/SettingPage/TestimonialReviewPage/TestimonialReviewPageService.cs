
using SuperariLife.Data.DBRepository.SettingPage.TestimonialReviewPage;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.SettingPages;

namespace SuperariLife.Service.SettingPage.TestimonialReviewPage
{
    public class TestimonialReviewPageService: ITestimonialReviewPageService
    {


        #region Fields
        private readonly ITestimonialReviewPageRepository _repository;
        #endregion

        #region Construtor
        public TestimonialReviewPageService(ITestimonialReviewPageRepository repository)
        {
            _repository = repository;
        }
        #endregion

        public async Task<int> DeleteTestimonialPagesReview(int testimonialPageReviewId)
        {
          return await _repository.DeleteTestimonialPagesReview(testimonialPageReviewId);
        }

        public async Task<TestimonialPagesReviewResponseModel> GetTestimonialPagesReviewById(int testimonialPageReviewId)
        {
            return await _repository.GetTestimonialPagesReviewById(testimonialPageReviewId);
        }

        public async Task<List<TestimonialPagesReviewResponseModel>> GetTestimonialPagesReviewForCustomer()
        {
            return await _repository.GetTestimonialPagesReviewForCustomer();
        }

        public async Task<List<TestimonialPagesReviewResponseModel>> GetTestimonialPagesReviewList(CommonPaginationModel info)
        {
            return await _repository.GetTestimonialPagesReviewList(info);
        }

        public async Task<long> InsertUpdateTestimonialPagesReview(TestimonialPagesReviewReqModel testimonialreviewInfo)
        {
            return await _repository.InsertUpdateTestimonialPagesReview(testimonialreviewInfo);
        }

    }
}
