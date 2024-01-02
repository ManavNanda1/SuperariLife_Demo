using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.User;


namespace SuperariLife.Data.DBRepository.User
{
    public interface IUserService
    {
        #region Admin
        Task<ResponseUserModel> ActiveDeactiveUserByAdmin(long userId);
        Task<UserDeleteResponseModel> DeleteUserByAdmin(long userId); 
        Task<List<ResponseUserModel>> GetUserListByAdmin(CommonPaginationModel info);
        Task<ResponseUserModel> GetUserByIdByAdmin(long userId);
        Task<List<ResponseUserModel>> GetUserListForDropDownList();
        Task<UserInsertUpdateResponseModel> InsertUpdateUserByAdmin(UserReqModelForAdmin userInfo);
        #endregion

        #region User
        Task<ResponseUserModel> GetUserById(long userId);
        Task<UserInsertUpdateResponseModel> InsertUpdateUserByUser(UserReqModelForUser userInfo);
        #endregion
    }
}
