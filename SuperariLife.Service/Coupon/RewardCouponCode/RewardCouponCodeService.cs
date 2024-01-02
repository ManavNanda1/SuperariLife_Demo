
using SuperariLife.Data.DBRepository.Coupon.RewardCouponCode;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.CouponCode;

namespace SuperariLife.Service.Coupon.RewardCouponCode
{
    public class RewardCouponCodeService: IRewardCouponCodeService
    {
        #region Fields
        private readonly IRewardCouponCodeRepository _repository;
        #endregion

        #region Construtor
        public RewardCouponCodeService(IRewardCouponCodeRepository repository)
        {
            _repository = repository;
        }
        #endregion

        public async Task<List<CustomerListForSendingCouponCodeRewardResponseModel>> GetCustomerListForSendingCouponCodeReward(string searchStr)
        {
            return await _repository.GetCustomerListForSendingCouponCodeReward(searchStr);
        }

        public async Task<List<RewardCouponCodeResponseModel>> GetRewardCouponCodeListByAdmin(CommonPaginationModel info)
        {
            return await _repository.GetRewardCouponCodeListByAdmin(info);
        }

        public async Task<long> InsertUpdateRewardCouponCode(RewardCouponCodeReqModel rewardcouponInfo)
        {
           return await _repository.InsertUpdateRewardCouponCode(rewardcouponInfo);   
        }
    }
}
