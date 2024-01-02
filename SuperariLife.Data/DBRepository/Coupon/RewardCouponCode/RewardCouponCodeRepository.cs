using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Config;
using SuperariLife.Model.CouponCode;
using System.Data;

namespace SuperariLife.Data.DBRepository.Coupon.RewardCouponCode
{
    public class RewardCouponCodeRepository:BaseRepository, IRewardCouponCodeRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public RewardCouponCodeRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        public async Task<List<CustomerListForSendingCouponCodeRewardResponseModel>> GetCustomerListForSendingCouponCodeReward(string searchStr)
        {
            var param = new DynamicParameters();
            param.Add("@StrSearch", searchStr);
            var data = await QueryAsync<CustomerListForSendingCouponCodeRewardResponseModel>(StoredProcedures.GetCustomerListForSendingCouponCodeReward, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async  Task<List<RewardCouponCodeResponseModel>> GetRewardCouponCodeListByAdmin(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
            var data = await QueryAsync<RewardCouponCodeResponseModel>(StoredProcedures.GetRewardCouponCodeListByAdmin, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<long> InsertUpdateRewardCouponCode(RewardCouponCodeReqModel rewardcouponInfo)
        {
            var param = new DynamicParameters();
            param.Add("@CouponCodeId", rewardcouponInfo.CouponCodeId);
            param.Add("@CustomerId", rewardcouponInfo.CustomerId);
            param.Add("@RewardCouponCodeId", rewardcouponInfo.RewardCouponCodeId);
            param.Add("@UserId", rewardcouponInfo.UserId);        
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.InsertUpdateRewardCouponCode, param, commandType: CommandType.StoredProcedure);
        }
    }
}
