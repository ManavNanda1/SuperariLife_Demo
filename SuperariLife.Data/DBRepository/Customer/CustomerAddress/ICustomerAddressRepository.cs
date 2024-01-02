using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Customer.CustomerAddress;

namespace SuperariLife.Data.DBRepository.Customer.CustomerAddress
{
    public interface ICustomerAddressRepository
    {
        #region Customer Address
        Task<long> DeleteCustomerAddress(long customerAddressId);
        Task<List<CustomerAddressResponseModel>> GetCustomerAddressList(CommonPaginationModel info);
        Task<CustomerAddressResponseModel> GetCustomerAddressById(long customerId);
        Task<CustomerAddressInsertUpdateResponseModel> InsertUpdateCustomerAddressByCustomer(CustomerAddressReqModelForAdmin customerAddressInfo);
        #endregion
    }
}
