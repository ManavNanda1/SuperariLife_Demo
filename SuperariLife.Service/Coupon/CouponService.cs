using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.CouponCode;

namespace SuperariLife.Data.DBRepository.Coupon
{
    public class CouponService:ICouponService
    {
        #region Fields
        private readonly ICouponRepository _repository;
        #endregion

        #region Construtor
        public CouponService(ICouponRepository repository)
        {
            _repository = repository;
        }
        #endregion
        public async Task<long> DeleteCouponCode(long couponId)
        {
           return await _repository.DeleteCouponCode(couponId);
        }

        public async Task<CouponCodeResponseModel> GetCouponCodeById(long couponId)
        {
            return await _repository.GetCouponCodeById(couponId);   
        }

        public async Task<List<CouponCodeResponseModel>> GetCouponCodeDetailById(CouponCodeReqDetailModel couponCodeDetailInfo)
        {
            return await _repository.GetCouponCodeDetailById(couponCodeDetailInfo);
        }

        public async Task<List<CouponCodeResponseModel>> GetCouponListByAdmin(CommonPaginationModel info)
        {
            return await _repository.GetCouponListByAdmin(info);
        }

        public async Task<long> InsertUpdateCouponCode(CouponCodeReqModel couponInfo)
        {
            return await _repository.InsertUpdateCouponCode(couponInfo);
        }
    
    }
}
