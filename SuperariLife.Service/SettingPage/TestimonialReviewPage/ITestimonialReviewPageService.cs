using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.SettingPages;

namespace SuperariLife.Service.SettingPage.TestimonialReviewPage
{
    public  interface ITestimonialReviewPageService
    {
        Task<int> DeleteTestimonialPagesReview(int testimonialPageReviewId);
        Task<List<TestimonialPagesReviewResponseModel>> GetTestimonialPagesReviewList(CommonPaginationModel info);
        Task<List<TestimonialPagesReviewResponseModel>> GetTestimonialPagesReviewForCustomer();
        Task<TestimonialPagesReviewResponseModel> GetTestimonialPagesReviewById(int testimonialPageReviewId);
        Task<long> InsertUpdateTestimonialPagesReview(TestimonialPagesReviewReqModel testimonialreviewInfo);
    }
}
