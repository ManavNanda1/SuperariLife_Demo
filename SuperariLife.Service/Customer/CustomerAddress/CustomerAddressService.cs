using SuperariLife.Data.DBRepository.Customer.CustomerAddress;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Customer.CustomerAddress;

namespace SuperariLife.Service.Customer.CustomerAddress
{
    public class CustomerAddressService:ICustomerAddressService
    {
        #region Fields
        private readonly ICustomerAddressRepository _repository;
        #endregion

        #region Construtor
        public CustomerAddressService(ICustomerAddressRepository repository)
        {
            _repository = repository;
        }
        #endregion

        public async Task<long> DeleteCustomerAddress(long customerAddressId)
        {
            return await _repository.DeleteCustomerAddress(customerAddressId);
        }

        public async Task<CustomerAddressResponseModel> GetCustomerAddressById(long customerId)
        {
            return await _repository.GetCustomerAddressById(customerId);  
        }

        public async Task<List<CustomerAddressResponseModel>> GetCustomerAddressList(CommonPaginationModel info)
        {
            return await _repository.GetCustomerAddressList(info);  
        }

        public async Task<CustomerAddressInsertUpdateResponseModel> InsertUpdateCustomerAddressByCustomer(CustomerAddressReqModelForAdmin customerAddressInfo)
        {
            return await _repository.InsertUpdateCustomerAddressByCustomer(customerAddressInfo);
        }
    }
}
