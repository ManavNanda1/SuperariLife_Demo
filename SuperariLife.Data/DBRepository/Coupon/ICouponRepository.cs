using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.CouponCode;

namespace SuperariLife.Data.DBRepository.Coupon
{
    public interface ICouponRepository
    {
        Task<long> DeleteCouponCode(long couponId);
        Task<List<CouponCodeResponseModel>> GetCouponListByAdmin(CommonPaginationModel info);
        Task<CouponCodeResponseModel> GetCouponCodeById(long couponId);
        Task<List<CouponCodeResponseModel>> GetCouponCodeDetailById(CouponCodeReqDetailModel couponDetailInfo);
        Task<long> InsertUpdateCouponCode(CouponCodeReqModel couponInfo);
    }
}
