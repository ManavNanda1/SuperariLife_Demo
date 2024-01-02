using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Config;
using SuperariLife.Model.CouponCode;
using System.Data;


namespace SuperariLife.Data.DBRepository.Coupon
{
    public class CouponRepository:BaseRepository,ICouponRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public CouponRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion
        public async Task<long> DeleteCouponCode(long couponId)
        {
            var param = new DynamicParameters();
            param.Add("@CouponCodeId", couponId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.DeleteCouponCode, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<CouponCodeResponseModel> GetCouponCodeById(long couponId)
        {
            var param = new DynamicParameters();
            param.Add("@CouponCodeId", couponId);
            return await QueryFirstOrDefaultAsync<CouponCodeResponseModel>(StoredProcedures.GetCouponCodeById, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<CouponCodeResponseModel>> GetCouponCodeDetailById(CouponCodeReqDetailModel couponDetaailInfo)
        {
            var param = new DynamicParameters();
            param.Add("@CouponCodeId", couponDetaailInfo.CouponCodeId);
            param.Add("@StrSearch", couponDetaailInfo.CouponCodeSearchString);
            var data= await QueryAsync<CouponCodeResponseModel>(StoredProcedures.GetCouponCodeDetailById, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<List<CouponCodeResponseModel>> GetCouponListByAdmin(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
            param.Add("@AllCoupon", info.AllUser);
            var data = await QueryAsync<CouponCodeResponseModel>(StoredProcedures.GetCouponListByAdmin, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<long> InsertUpdateCouponCode(CouponCodeReqModel couponInfo)
        {
             var param = new DynamicParameters();
            param.Add("@CouponCodeId", couponInfo.CouponCodeId);
            param.Add("@CouponCode", couponInfo.CouponCode);
            param.Add("@UserId", couponInfo.UserId);
            param.Add("@StartDateOfCoupon", couponInfo.StartDateOfCoupon);
            param.Add("@ExpireDateOfCoupon", couponInfo.ExpireDateOfCoupon);
            param.Add("@CouponType", couponInfo.CouponType);
            param.Add("@DiscountAmountORPercentage", couponInfo.DiscountAmountORPercentage);
            param.Add("@DiscountType", couponInfo.DiscountType);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.InsertUpdateCouponCode, param, commandType: CommandType.StoredProcedure);
        }
   
    }
}
