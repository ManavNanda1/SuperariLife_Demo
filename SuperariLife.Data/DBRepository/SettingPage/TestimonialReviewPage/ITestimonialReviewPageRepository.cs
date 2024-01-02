

using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.SettingPages;

namespace SuperariLife.Data.DBRepository.SettingPage.TestimonialReviewPage
{
    public interface ITestimonialReviewPageRepository
    {
        Task<int> DeleteTestimonialPagesReview(int testimonialPageReviewId);
        Task<List<TestimonialPagesReviewResponseModel>> GetTestimonialPagesReviewList(CommonPaginationModel info);
        Task<List<TestimonialPagesReviewResponseModel>> GetTestimonialPagesReviewForCustomer();
        Task<TestimonialPagesReviewResponseModel> GetTestimonialPagesReviewById(int testimonialPageReviewId);
        Task<long> InsertUpdateTestimonialPagesReview(TestimonialPagesReviewReqModel testimonialreviewInfo);
    }
}
