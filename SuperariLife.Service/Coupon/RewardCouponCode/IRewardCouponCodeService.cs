

using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.CouponCode;

namespace SuperariLife.Service.Coupon.RewardCouponCode
{
    public interface IRewardCouponCodeService
    {
        Task<List<RewardCouponCodeResponseModel>> GetRewardCouponCodeListByAdmin(CommonPaginationModel info);
        Task<List<CustomerListForSendingCouponCodeRewardResponseModel>> GetCustomerListForSendingCouponCodeReward(string searchStr);
        Task<long> InsertUpdateRewardCouponCode(RewardCouponCodeReqModel rewardcouponInfo);
    }
}
