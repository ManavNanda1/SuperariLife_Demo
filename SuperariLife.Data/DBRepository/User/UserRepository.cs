using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Config;
using SuperariLife.Model.User;
using System.Data;


namespace SuperariLife.Data.DBRepository.User
{
    public class UserRepository: BaseRepository, IUserRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public UserRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        public async Task<ResponseUserModel> ActiveDeactiveUserByAdmin(long userId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            return await QueryFirstOrDefaultAsync<ResponseUserModel>(StoredProcedures.ActiveDeactiveUserByAdmin, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<UserDeleteResponseModel> DeleteUserByAdmin(long userId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            return await QueryFirstOrDefaultAsync<UserDeleteResponseModel>(StoredProcedures.DeleteUserByAdmin, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<ResponseUserModel> GetUserById(long userId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            return await QueryFirstOrDefaultAsync<ResponseUserModel>(StoredProcedures.GetUserById, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<ResponseUserModel> GetUserByIdByAdmin(long userId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            return await QueryFirstOrDefaultAsync<ResponseUserModel>(StoredProcedures.GetUserByIdByAdmin, param, commandType: CommandType.StoredProcedure);
        }

        public async  Task<List<ResponseUserModel>> GetUserListByAdmin(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
            param.Add("@AllUser ", info.AllUser);
            var data = await QueryAsync<ResponseUserModel>(StoredProcedures.GetUserListByAdmin, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<List<ResponseUserModel>> GetUserListForDropDownList()
        {    
            var data = await QueryAsync<ResponseUserModel>(StoredProcedures.GetUserListForDropDownList, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<UserInsertUpdateResponseModel> InsertUpdateUserByAdmin(UserReqModelForAdmin userInfo)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", userInfo.UserId);
            param.Add("@RoleManagementId", userInfo.RoleManagementId);
            param.Add("@Email", userInfo.Email);
            param.Add("@Firstname", userInfo.Firstname);
            param.Add("@Lastname", userInfo.Lastname);
            param.Add("@Password ", userInfo.Password);
            param.Add("@CreatedBy", userInfo.CreatedBy);
            param.Add("@PasswordSalt", userInfo.PasswordSalt);
            param.Add("@UserImage", userInfo.ImageName);
            param.Add("@UserAddress", userInfo.UserAddress);
            param.Add("@UserMobileNumber", userInfo.UserMobileNumber);
            param.Add("@UserCountryId", userInfo.UserCountryId);
            param.Add("@UserStateId", userInfo.UserStateId);
            param.Add("@UserCityId", userInfo.UserCityId);
            param.Add("@UserPostalCode", userInfo.UserPostalCode);
            return await QueryFirstOrDefaultAsync<UserInsertUpdateResponseModel>(StoredProcedures.InsertUpdateUserByAdmin, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<UserInsertUpdateResponseModel> InsertUpdateUserByUser(UserReqModelForUser userInfo)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", userInfo.UserId);
            param.Add("@Email", userInfo.Email);
            param.Add("@Firstname", userInfo.Firstname);
            param.Add("@Lastname", userInfo.Lastname);
            param.Add("@Password ", userInfo.Password);
            param.Add("@UserImage", userInfo.ImageName);
            param.Add("@UserMobileNumber", userInfo.UserMobileNumber);
            param.Add("@UserAddress", userInfo.UserAddress);
            param.Add("@UserCountryId", userInfo.UserCountryId);
            param.Add("@UserStateId", userInfo.UserStateId);
            param.Add("@UserCityId", userInfo.UserCityId);
            param.Add("@UserPostalCode", userInfo.UserPostalCode);
            param.Add("@PasswordSalt", userInfo.PasswordSalt);
            return await QueryFirstOrDefaultAsync<UserInsertUpdateResponseModel>(StoredProcedures.InsertUpdateUserByUser, param, commandType: CommandType.StoredProcedure);
        }

    }
}
