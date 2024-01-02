using SuperariLife.Model.Appointment;
using SuperariLife.Model.CommonPagination;


namespace SuperariLife.Data.DBRepository.Appointment
{
    public interface IAppointmentRepository
    {
        #region Admin
        Task<List<AppointmentResponseModelForAdmin>> GetAppointmentListByAdmin(CommonPaginationModel info);
        Task<AppointmentResponseModelForAdmin> GetAppointmentDetailByIdForAdmin(long appointmentId);
        Task<long> InsertUpdateAppointmentByAdmin(AppointmentReqModelByAdmin appointmentInfo);
        #endregion

        #region Customer

        Task<long> DeleteAppointmentByCustomer(long appointmentId);
        Task<List<AppointmentResponseModelForCustomer>> GetAppointmentListByCustomer(CommonPaginationModel info);
        Task<AppointmentResponseModelForCustomer> GetAppointmentDetailByIdForCustomer(long appointmentId);
        Task<long> InsertUpdateAppointmentByCustomer(AppointmentReqModelByCustomer appointmentInfo);
        #endregion

        #region Common
        Task<List<AppointmentResponseDropDownModel>> GetAppointmentTypeForDropDown();
        #endregion

    }
}
