
using SuperariLife.Data.DBRepository.PaymentType;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.PaymentType;

namespace SuperariLife.Service.PaymentType
{
    public class PaymentTypeService : IPaymentTypeService
    {

        #region Fields
        private readonly IPaymentTypeRepository _repository;
        #endregion

        #region Construtor
        public PaymentTypeService(IPaymentTypeRepository repository)
        {
            _repository = repository;
        }
        #endregion
        public async Task<long> DeletePaymentType(long paymentTypeId)
        {
            return await _repository.DeletePaymentType(paymentTypeId);
        }

        public async Task<PaymentTypeResponseModel> GetPaymentTypeById(long paymentTypeId)
        {
            return await _repository.GetPaymentTypeById(paymentTypeId);
        }

        public async Task<List<PaymentTypeResponseModel>> GetPaymentTypeList(CommonPaginationModel info)
        {
            return await _repository.GetPaymentTypeList(info);
        }

        public async Task<long> InsertUpdatePaymentType(PaymentTypeReqModel paymentTypeInfo)
        {
            return await _repository.InsertUpdatePaymentType(paymentTypeInfo);
        }
    }
}
