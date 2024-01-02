using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.RoleManagement;


namespace SuperariLife.Data.DBRepository.RoleManagement
{
    public class RoleManagementService:IRoleManagementService
    {
        #region Fields
        private readonly IRoleManagementRepository _repository;
        #endregion

        #region Construtor
        public RoleManagementService(IRoleManagementRepository repository)
        {
            _repository = repository;
        }
        #endregion

        public async Task<RoleManagementDeleteResponseModel> DeleteRoleManagement(long roleId)
        {
           return await _repository.DeleteRoleManagement(roleId);
        }

        public async Task<List<RoleManagementResponseModel>> GetRoleListForDropDown()
        {
            return await  _repository.GetRoleListForDropDown();  
        }

        public async Task<RoleManagementResponseModel> GetRoleManagementById(long roleId)
        {
            return await _repository.GetRoleManagementById(roleId);
        }

        public async Task<List<RoleManagementResponseModel>> GetRoleManagementList(CommonPaginationModel info)
        {
            return await _repository.GetRoleManagementList(info);
        }

        public async Task<long> InsertUpdateRoleManagement(RoleManagementReqModel roleInfo)
        {
           return await _repository.InsertUpdateRoleManagement(roleInfo);
        }
      
    }
}
