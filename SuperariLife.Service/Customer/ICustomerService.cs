using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Customer;

namespace SuperariLife.Data.DBRepository.Customer
{
    public interface ICustomerService
    {
        #region Admin Portal
        Task<CustomerResponseModel> ActiveDeactiveCustomerByAdmin(long customerId);
        Task<CustomerDeleteResponseModel> DeleteCustomerByAdmin(long customerId);
        Task<List<CustomerResponseModel>> GetCustomerListByAdmin(CommonPaginationModel info);
        Task<CustomerResponseModel> GetCustomerByIdByAdmin(long customerId);
        Task<CustomerInsertUpdateResponseModel> InsertUpdateCustomerByAdmin(CustomerReqModelForAdmin customerInfo);
        #endregion

        #region Customer Portal
        Task<CustomerInsertUpdateResponseModel> InsertUpdateCustomer(CustomerReqModelForAdmin customerInfo);
        #endregion
    }
}
