using SuperariLife.Data.DBRepository.Appointment;
using SuperariLife.Model.Appointment;
using SuperariLife.Model.CommonPagination;

namespace SuperariLife.Service.Appointment
{
    public class AppointmentService: IAppointmentService
    {
        #region Fields
        private readonly IAppointmentRepository _repository;
        #endregion

        #region Construtor
        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository;
        }
        #endregion

        public async Task<long> DeleteAppointmentByCustomer(long appointmentId)
        {
            return await _repository.DeleteAppointmentByCustomer(appointmentId);
        }

        public async Task<AppointmentResponseModelForAdmin> GetAppointmentDetailByIdForAdmin(long appointmentId)
        {
            return await _repository.GetAppointmentDetailByIdForAdmin(appointmentId);
        }

        public async Task<AppointmentResponseModelForCustomer> GetAppointmentDetailByIdForCustomer(long appointmentId)
        {
            return await _repository.GetAppointmentDetailByIdForCustomer(appointmentId);
        }

        public async Task<List<AppointmentResponseModelForAdmin>> GetAppointmentListByAdmin(CommonPaginationModel info)
        {
            return await _repository.GetAppointmentListByAdmin(info);
        }

        public async Task<List<AppointmentResponseModelForCustomer>> GetAppointmentListByCustomer(CommonPaginationModel info)
        {
           return await _repository.GetAppointmentListByCustomer(info);
        }

        public async Task<List<AppointmentResponseDropDownModel>> GetAppointmentTypeForDropDown()
        {
            return await _repository.GetAppointmentTypeForDropDown();
        }

        public async Task<long> InsertUpdateAppointmentByAdmin(AppointmentReqModelByAdmin appointmentInfo)
        {
            return await _repository.InsertUpdateAppointmentByAdmin(appointmentInfo);
        }

        public async Task<long> InsertUpdateAppointmentByCustomer(AppointmentReqModelByCustomer appointmentInfo)
        {
            return await _repository.InsertUpdateAppointmentByCustomer(appointmentInfo);
        }
       
    }
}
