using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.RoleManagement;

namespace SuperariLife.Data.DBRepository.RoleManagement
{
    public interface IRoleManagementRepository
    {
        Task<RoleManagementDeleteResponseModel> DeleteRoleManagement(long roleId);
        Task<List<RoleManagementResponseModel>> GetRoleManagementList(CommonPaginationModel info);
        Task<List<RoleManagementResponseModel>> GetRoleListForDropDown();
        Task<RoleManagementResponseModel> GetRoleManagementById(long roleId);    
        Task<long> InsertUpdateRoleManagement(RoleManagementReqModel roleInfo);
    }
}
