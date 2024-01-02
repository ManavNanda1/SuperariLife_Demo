
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Customer;

namespace SuperariLife.Data.DBRepository.Customer
{
    public class CustomerService: ICustomerService
    {
        #region Fields
        private readonly ICustomerRepository _repository;
        #endregion

        #region Construtor
        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Admin Portal
        public async Task<CustomerResponseModel> ActiveDeactiveCustomerByAdmin(long customerId)
        {
            return await _repository.ActiveDeactiveCustomerByAdmin(customerId);
        }

        public async Task<CustomerDeleteResponseModel> DeleteCustomerByAdmin(long customerId)
        {
            return await _repository.DeleteCustomerByAdmin(customerId);
        }

        public async Task<CustomerResponseModel> GetCustomerByIdByAdmin(long customerId)
        {
            return await _repository.GetCustomerByIdByAdmin(customerId);
        }

        public async Task<List<CustomerResponseModel>> GetCustomerListByAdmin(CommonPaginationModel info)
        {
            return await _repository.GetCustomerListByAdmin(info);
        }
        public async Task<CustomerInsertUpdateResponseModel> InsertUpdateCustomerByAdmin(CustomerReqModelForAdmin customerInfo)
        {
            return await _repository.InsertUpdateCustomerByAdmin(customerInfo);
        }
        #endregion

        #region Customer Portal
        public async Task<CustomerInsertUpdateResponseModel> InsertUpdateCustomer(CustomerReqModelForAdmin customerInfo)
        {
           return await _repository.InsertUpdateCustomer(customerInfo);     
        }

        #endregion


    }
}
