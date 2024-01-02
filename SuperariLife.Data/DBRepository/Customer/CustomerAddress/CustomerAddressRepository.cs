using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Config;
using SuperariLife.Model.Customer.CustomerAddress;
using System.Data;


namespace SuperariLife.Data.DBRepository.Customer.CustomerAddress
{
    public class CustomerAddressRepository:BaseRepository, ICustomerAddressRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public CustomerAddressRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        #region Customer Address
        public async Task<long> DeleteCustomerAddress(long customerAddressId)
        {
            var param = new DynamicParameters();
            param.Add("@CustomerAddressId", customerAddressId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.DeleteCustomerAddress, param, commandType: CommandType.StoredProcedure);
        }
        public async Task<List<CustomerAddressResponseModel>> GetCustomerAddressList(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
            var data = await QueryAsync<CustomerAddressResponseModel>(StoredProcedures.GetCustomerAddressList, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<CustomerAddressResponseModel> GetCustomerAddressById(long customerId)
        {
            var param = new DynamicParameters();
            param.Add("@CustomerId", customerId);
            return await QueryFirstOrDefaultAsync<CustomerAddressResponseModel>(StoredProcedures.GetCustomerAddressById, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<CustomerAddressInsertUpdateResponseModel> InsertUpdateCustomerAddressByCustomer(CustomerAddressReqModelForAdmin customerAddressInfo)
        {
            var param = new DynamicParameters();
            param.Add("@CustomerAddressId", customerAddressInfo.CustomerAddressId);
            param.Add("@CustomerId", customerAddressInfo.CustomerId);
            param.Add("@Customerfirstname", customerAddressInfo.CustomerFirstname);
            param.Add("@Customerlastname", customerAddressInfo.CustomerLastname);
            param.Add("@CustomerPhoneNumber", customerAddressInfo.CustomerPhoneNumber);
            param.Add("@CustomerAddress", customerAddressInfo.CustomerAddress);
            param.Add("@CountryId", customerAddressInfo.CountryId);
            param.Add("@StateId", customerAddressInfo.StateId);
            param.Add("@CityId", customerAddressInfo.CityId);
            param.Add("@UserId", customerAddressInfo.UserId);
            param.Add("@PostalCode", customerAddressInfo.PostalCode);
            param.Add("@IsDefaultAddress", customerAddressInfo.IsDefaultAddress);
            return await QueryFirstOrDefaultAsync<CustomerAddressInsertUpdateResponseModel>(StoredProcedures.InsertUpdateCustomerAddressByCustomer, param, commandType: CommandType.StoredProcedure);
        }
        #endregion
    }
}
