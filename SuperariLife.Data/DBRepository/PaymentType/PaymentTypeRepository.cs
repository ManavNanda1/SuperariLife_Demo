using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Config;
using SuperariLife.Model.PaymentType;
using System.Data;


namespace SuperariLife.Data.DBRepository.PaymentType
{
    public class PaymentTypeRepository: BaseRepository,IPaymentTypeRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public PaymentTypeRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        public async  Task<long> DeletePaymentType(long paymentTypeId)
        {
            var param = new DynamicParameters();
            param.Add("@PaymentTypeId", paymentTypeId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.DeletePaymentType, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<PaymentTypeResponseModel> GetPaymentTypeById(long paymentTypeId)
        {
            var param = new DynamicParameters();
            param.Add("@PaymentTypeId", paymentTypeId);
            return await QueryFirstOrDefaultAsync<PaymentTypeResponseModel>(StoredProcedures.GetPaymentTypeById, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<PaymentTypeResponseModel>> GetPaymentTypeList(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
            var data = await QueryAsync<PaymentTypeResponseModel>(StoredProcedures.GetPaymentTypeList, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<long> InsertUpdatePaymentType(PaymentTypeReqModel paymentTypeInfo)
        {
            var param = new DynamicParameters();
            param.Add("@PaymentTypeId", paymentTypeInfo.PaymentTypeId);
            param.Add("@PaymentTypeName", paymentTypeInfo.PaymentTypeName);
            param.Add("@UserId", paymentTypeInfo.UserId);
            param.Add("@PaymentText", paymentTypeInfo.PaymentText);
            param.Add("@Amount", paymentTypeInfo.Amount);
            param.Add("@ExpiryDays", paymentTypeInfo.ExpiryDays);
            param.Add("@NoOfPass", paymentTypeInfo.NoOfPass);
            param.Add("@TypeOfPass", paymentTypeInfo.TypeOfPass);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.InsertUpdatePaymentType, param, commandType: CommandType.StoredProcedure);
        }

    }
}
