using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Config;
using SuperariLife.Model.RoleManagement;
using System.Data;


namespace SuperariLife.Data.DBRepository.RoleManagement
{
    public class RoleManagementRepository: BaseRepository, IRoleManagementRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public RoleManagementRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        public async Task<RoleManagementDeleteResponseModel> DeleteRoleManagement(long roleId)
        {
            var param = new DynamicParameters();
            param.Add("@RoleManagementId", roleId);
            return await QueryFirstOrDefaultAsync<RoleManagementDeleteResponseModel>(StoredProcedures.DeleteRoleManagement, param, commandType: CommandType.StoredProcedure);
        }
        public async Task<List<RoleManagementResponseModel>> GetRoleListForDropDown()
        {
            var data = await QueryAsync<RoleManagementResponseModel>(StoredProcedures.GetRoleListForDropDown, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        public async  Task<RoleManagementResponseModel> GetRoleManagementById(long roleId)
        {
            var param = new DynamicParameters();
            param.Add("@RoleManagementId", roleId);
            return await QueryFirstOrDefaultAsync<RoleManagementResponseModel>(StoredProcedures.GetRoleManagementById, param, commandType: CommandType.StoredProcedure);
        }
        public async Task<List<RoleManagementResponseModel>> GetRoleManagementList(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
            var data = await QueryAsync<RoleManagementResponseModel>(StoredProcedures.GetRoleManagement, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        public async Task<long> InsertUpdateRoleManagement(RoleManagementReqModel roleInfo)
        {
            var param = new DynamicParameters();
            param.Add("@RoleManagementId", roleInfo.RoleManagementId);
            param.Add("@RoleName", roleInfo.RoleName);
            param.Add("@UserId", roleInfo.UserId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.InsertUpdateRoleManagement, param, commandType: CommandType.StoredProcedure);
        }
    }
}
