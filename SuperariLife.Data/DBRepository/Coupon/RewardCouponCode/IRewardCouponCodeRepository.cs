using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.CouponCode;

namespace SuperariLife.Data.DBRepository.Coupon.RewardCouponCode
{
    public interface IRewardCouponCodeRepository
    {
        Task<List<RewardCouponCodeResponseModel>> GetRewardCouponCodeListByAdmin(CommonPaginationModel info);
        Task<List<CustomerListForSendingCouponCodeRewardResponseModel>> GetCustomerListForSendingCouponCodeReward(string searchStr);
        Task<long> InsertUpdateRewardCouponCode(RewardCouponCodeReqModel rewardcouponInfo);
    }
}
