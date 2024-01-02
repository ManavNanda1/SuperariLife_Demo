using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.PaymentType;


namespace SuperariLife.Service.PaymentType
{
    public interface IPaymentTypeService
    {
        Task<long> DeletePaymentType(long paymentTypeId);
        Task<List<PaymentTypeResponseModel>> GetPaymentTypeList(CommonPaginationModel info);
        Task<PaymentTypeResponseModel> GetPaymentTypeById(long paymentTypeId);
        Task<long> InsertUpdatePaymentType(PaymentTypeReqModel paymentTypeInfo);
    }
}
