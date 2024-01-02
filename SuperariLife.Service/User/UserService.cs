using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.User;

namespace SuperariLife.Data.DBRepository.User
{
    public class UserService: IUserService
    {
        #region Fields
        private readonly IUserRepository _repository;
        #endregion

        #region Construtor
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        #endregion
        public async Task<ResponseUserModel> ActiveDeactiveUserByAdmin(long userId)
        {
            return await _repository.ActiveDeactiveUserByAdmin(userId);
        }

        public  async Task<UserDeleteResponseModel> DeleteUserByAdmin(long userId)
        {
            return await _repository.DeleteUserByAdmin(userId);
        }

        public async Task<ResponseUserModel> GetUserById(long userId)
        {
            return await _repository.GetUserById(userId);
        }

        public async Task<ResponseUserModel> GetUserByIdByAdmin(long userId)
        {
            return await _repository.GetUserByIdByAdmin(userId);
        }

        public async Task<List<ResponseUserModel>> GetUserListByAdmin(CommonPaginationModel info)
        {
            return await _repository.GetUserListByAdmin(info);
        }

        public async Task<List<ResponseUserModel>> GetUserListForDropDownList()
        {
            return await _repository.GetUserListForDropDownList();  
        }

        public async Task<UserInsertUpdateResponseModel> InsertUpdateUserByAdmin(UserReqModelForAdmin userInfo)
        {
            return await _repository.InsertUpdateUserByAdmin(userInfo);
        }

        public async Task<UserInsertUpdateResponseModel> InsertUpdateUserByUser(UserReqModelForUser userInfo)
        {
            return await _repository.InsertUpdateUserByUser(userInfo);
        }
       
    }
}
