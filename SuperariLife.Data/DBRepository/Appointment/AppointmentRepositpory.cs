using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Appointment;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Config;
using System.Data;

namespace SuperariLife.Data.DBRepository.Appointment
{
    public  class AppointmentRepositpory: BaseRepository,IAppointmentRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public AppointmentRepositpory(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        public async Task<long> DeleteAppointmentByCustomer(long appointmentId)
        {
            var param = new DynamicParameters();
            param.Add("@AppointmentId", appointmentId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.DeleteAppointmentByCustomer, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<AppointmentResponseModelForAdmin> GetAppointmentDetailByIdForAdmin(long appointmentId)
        {
            var param = new DynamicParameters();
            param.Add("@AppointmentId", appointmentId);
            return await QueryFirstOrDefaultAsync<AppointmentResponseModelForAdmin>(StoredProcedures.GetAppointmentDetailByIdForAdmin, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<AppointmentResponseModelForCustomer> GetAppointmentDetailByIdForCustomer(long appointmentId)
        {
            var param = new DynamicParameters();
            param.Add("@AppointmentId", appointmentId);
            return await QueryFirstOrDefaultAsync<AppointmentResponseModelForCustomer>(StoredProcedures.GetAppointmentDetailByIdForCustomer, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<AppointmentResponseModelForAdmin>> GetAppointmentListByAdmin(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
            var data = await QueryAsync<AppointmentResponseModelForAdmin>(StoredProcedures.GetAppointmentListByAdmin, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
    

        public async  Task<List<AppointmentResponseModelForCustomer>> GetAppointmentListByCustomer(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
            param.Add("@CustomerId ", info.CustomerId);
            var data = await QueryAsync<AppointmentResponseModelForCustomer>(StoredProcedures.GetAppointmentListByCustomer, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<List<AppointmentResponseDropDownModel>> GetAppointmentTypeForDropDown()
        {
            var data = await QueryAsync<AppointmentResponseDropDownModel>(StoredProcedures.GetAppointmentTypeForDropDown,  commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<long> InsertUpdateAppointmentByAdmin(AppointmentReqModelByAdmin appointmentInfo)
        {
            var param = new DynamicParameters();
            param.Add("@AppointmentId", appointmentInfo.AppointmentId);
            param.Add("@AppointmentTypeId", appointmentInfo.AppointmentTypeId);
            param.Add("@AppointmentDate", appointmentInfo.AppointmentDate);
            param.Add("@AppointmentTime", appointmentInfo.AppointmentTime);
            param.Add("@AppointmentChargeAmount", appointmentInfo.AppointmentChargeAmount);
            param.Add("@AppointmentAddress", appointmentInfo.AppointmentAddress);
            param.Add("@AppointmentLink", appointmentInfo.AppointmentLink);
            param.Add("@CountryId", appointmentInfo.AppointmentCountryId);
            param.Add("@StateId", appointmentInfo.AppointmentStateId);
            param.Add("@CityId", appointmentInfo.AppointmentCityId);
            param.Add("@ZipCode", appointmentInfo.Zipcode);
            param.Add("@AdminMessage", appointmentInfo.AdminMessage);
            param.Add("@UserId", appointmentInfo.UserId);
            param.Add("@IsAppointmentAccepted", appointmentInfo.IsAppointmentAccepted);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.InsertUpdateAppointmentByAdmin, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> InsertUpdateAppointmentByCustomer(AppointmentReqModelByCustomer appointmentInfo)
        {
            var param = new DynamicParameters();
            param.Add("@AppointmentAddress", appointmentInfo.AppointmentAddress);
            param.Add("@AppointmentId", appointmentInfo.AppointmentId);
            param.Add("@AppointmentTypeId", appointmentInfo.AppointmentTypeId);
            param.Add("@AppointmentDate", appointmentInfo.AppointmentDate);
            param.Add("@AppointmentTime", appointmentInfo.AppointmentTime);
            param.Add("@CustomerMessage", appointmentInfo.CustomerMessage);
            param.Add("@CustomerId", appointmentInfo.CustomerId);
            param.Add("@CountryId", appointmentInfo.AppointmentCountryId);
            param.Add("@CityId", appointmentInfo.AppointmentCityId);
            param.Add("@StateId", appointmentInfo.AppointmentStateId);
            param.Add("@ZipCode", appointmentInfo.Zipcode);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.InsertUpdateAppointmentByCustomer, param, commandType: CommandType.StoredProcedure);
        }
      
    }
}
