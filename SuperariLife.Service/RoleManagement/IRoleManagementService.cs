using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.RoleManagement;


namespace SuperariLife.Data.DBRepository.RoleManagement
{
    public interface IRoleManagementService
    {
        Task<RoleManagementDeleteResponseModel> DeleteRoleManagement(long roleId);  
        Task<List<RoleManagementResponseModel>> GetRoleManagementList(CommonPaginationModel info);
        Task<RoleManagementResponseModel> GetRoleManagementById(long roleId);
        Task<List<RoleManagementResponseModel>> GetRoleListForDropDown();
        Task<long> InsertUpdateRoleManagement(RoleManagementReqModel roleInfo);
    }
}
