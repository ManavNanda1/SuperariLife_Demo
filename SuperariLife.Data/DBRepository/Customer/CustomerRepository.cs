using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Config;
using SuperariLife.Model.Customer;
using System.Data;

namespace SuperariLife.Data.DBRepository.Customer
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public CustomerRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion


        #region Admin Portal
        public async Task<CustomerResponseModel> ActiveDeactiveCustomerByAdmin(long customerId)
        {
            var param = new DynamicParameters();
            param.Add("@CustomerId", customerId);
            return await QueryFirstOrDefaultAsync<CustomerResponseModel>(StoredProcedures.ActiveDeactiveCustomerByAdmin, param, commandType: CommandType.StoredProcedure);
        }
        public async  Task<CustomerDeleteResponseModel> DeleteCustomerByAdmin(long customerId)
        {
            var param = new DynamicParameters();
            param.Add("@CustomerId", customerId);
            return await QueryFirstOrDefaultAsync<CustomerDeleteResponseModel>(StoredProcedures.DeleteCustomerByAdmin, param, commandType: CommandType.StoredProcedure);
        }
        public async Task<CustomerResponseModel> GetCustomerByIdByAdmin(long customerId)
        {
            var param = new DynamicParameters();
            param.Add("@CustomerId", customerId);
            return await QueryFirstOrDefaultAsync<CustomerResponseModel>(StoredProcedures.GetCustomerByIdByAdmin, param, commandType: CommandType.StoredProcedure);
        }
        public async Task<List<CustomerResponseModel>> GetCustomerListByAdmin(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
            param.Add("@AllCustomer ", info.AllUser);
            var data = await QueryAsync<CustomerResponseModel>(StoredProcedures.GetCustomerListByAdmin, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public  async Task<CustomerInsertUpdateResponseModel> InsertUpdateCustomerByAdmin(CustomerReqModelForAdmin customerInfo)
        {
            var param = new DynamicParameters();
            param.Add("@CustomerId",customerInfo.CustomerId);
            param.Add("@CustomerImage", customerInfo.ImageName);
            param.Add("@Customerfirstname", customerInfo.Customerfirstname);
            param.Add("@Customerlastname", customerInfo.Customerlastname);
            param.Add("@CustomerEmail", customerInfo.CustomerEmail);
            param.Add("@CustomerPhoneNumber", customerInfo.CustomerPhoneNumber);
            param.Add("@CustomerAddress", customerInfo.CustomerAddress);
            param.Add("@CountryId", customerInfo.CountryId);
            param.Add("@StateId", customerInfo.StateId);
            param.Add("@CityId", customerInfo.CityId);
            param.Add("@PostalCode", customerInfo.PostalCode);
            param.Add("@UserId", customerInfo.UserId);
            param.Add("@CustomerPassword",customerInfo.CustomerPassword);
            param.Add("@CustomerPasswordSalt", customerInfo.CustomerPasswordSalt);
            return await QueryFirstOrDefaultAsync<CustomerInsertUpdateResponseModel>(StoredProcedures.InsertUpdateCustomerByAdmin, param, commandType: CommandType.StoredProcedure);
        }
        #endregion

        #region Customer Portal

        #region Customer
                    public async Task<CustomerInsertUpdateResponseModel> InsertUpdateCustomer(CustomerReqModelForAdmin customerInfo)
                    {
                        var param = new DynamicParameters();
                        param.Add("@CustomerId", customerInfo.CustomerId);
                        param.Add("@CustomerImage", customerInfo.ImageName);
                        param.Add("@Customerfirstname", customerInfo.Customerfirstname);
                        param.Add("@Customerlastname", customerInfo.Customerlastname);
                        param.Add("@CustomerEmail", customerInfo.CustomerEmail);
                        param.Add("@CustomerPhoneNumber", customerInfo.CustomerPhoneNumber);
                        param.Add("@CustomerAddress", customerInfo.CustomerAddress);
                        param.Add("@CountryId", customerInfo.CountryId);
                        param.Add("@StateId", customerInfo.StateId);
                        param.Add("@CityId", customerInfo.CityId);
                        param.Add("@PostalCode", customerInfo.PostalCode);
                        param.Add("@CustomerPassword", customerInfo.CustomerPassword);
                        param.Add("@CustomerPasswordSalt", customerInfo.CustomerPasswordSalt);
                        return await QueryFirstOrDefaultAsync<CustomerInsertUpdateResponseModel>(StoredProcedures.InsertUpdateCustomer, param, commandType: CommandType.StoredProcedure);
                    }

                 #endregion

        #endregion
    }

}

