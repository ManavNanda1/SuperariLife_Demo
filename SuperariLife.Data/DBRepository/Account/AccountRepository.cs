using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Config;
using SuperariLife.Model.Login;
using System.Data;


namespace SuperariLife.Data.DBRepository.Account
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public AccountRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        #region Post
       
        public async Task<long> ValidateUserTokenData(long Id, string jwtToken, DateTime TokenValidDate, bool IsSuperAdmin)
        {
            var param = new DynamicParameters();
            param.Add("@Id", Id);   
            param.Add("@jwtToken", jwtToken);
            param.Add("@TokenValidDate", TokenValidDate);
            param.Add("@IsSuperAdmin", IsSuperAdmin);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.ValidateToken, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<SaltResponseModel> GetUserSalt(string EmailId)
        {
            var param = new DynamicParameters();
            param.Add("@EmailId", EmailId);
            return await QueryFirstOrDefaultAsync<SaltResponseModel>(StoredProcedures.GetUserSaltByEmail, param, commandType: CommandType.StoredProcedure);
        }
        public async Task<SaltResponseModel> GetCustomerSaltByEmail(string CustomerEmail)
        {
            var param = new DynamicParameters();
            param.Add("@EmailId", CustomerEmail);
            return await QueryFirstOrDefaultAsync<SaltResponseModel>(StoredProcedures.GetCustomerSaltByEmail, param, commandType: CommandType.StoredProcedure);
        }
        public async Task<string> ChangePassword(long UserId, string Password, string Salt)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            param.Add("@NewPassword", Password);
            param.Add("@PasswordSalt", Salt);
            return await QueryFirstOrDefaultAsync<string>(StoredProcedures.ChangePassword, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<string> ChangePasswordForCustomer(long CustomerId, string Password, string Salt)
        {
            var param = new DynamicParameters();
            param.Add("@CustomerId", CustomerId);
            param.Add("@NewPassword", Password);
            param.Add("@PasswordSalt", Salt);
            return await QueryFirstOrDefaultAsync<string>(StoredProcedures.ChangePasswordForCustomer, param, commandType: CommandType.StoredProcedure);
        }
        public async Task<LoginResponseModel> LoginUser(LoginRequestModel model)
        {
            var param = new DynamicParameters();
            param.Add("@Email", model.Email);
            param.Add("@Password", model.Password);
            return await QueryFirstOrDefaultAsync<LoginResponseModel>(StoredProcedures.LoginUser, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<CustomerLoginResponseModel> CustomerLogin(LoginRequestModel model)
        {
            var param = new DynamicParameters();
            param.Add("@Email", model.Email);
            param.Add("@Password", model.Password);
            return await QueryFirstOrDefaultAsync<CustomerLoginResponseModel>(StoredProcedures.CustomerLogin, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> UpdateLoginToken(string Token, long UserId)
        {
            var param = new DynamicParameters();
            param.Add("@Token", Token);
            param.Add("@UserId", UserId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.UpdateLoginToken, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> UpdateLoginTokenForCustomer(string Token, long CustomerId)
        {
            var param = new DynamicParameters();
            param.Add("@Token", Token);
            param.Add("@CustomerId", CustomerId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.UpdateLoginTokenForCustomer, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> LogoutUser(long UserId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.LogoutUser, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> LogoutCustomer(long CustomerId)
        {
            var param = new DynamicParameters();
            param.Add("@CustomerId", CustomerId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.LogoutCustomer, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<ForgetPasswordResponseModel> ForgetPassword(string EmailId)
        {
            var param = new DynamicParameters();
            param.Add("@EmailId", EmailId);
            return await QueryFirstOrDefaultAsync<ForgetPasswordResponseModel>(StoredProcedures.ForgetPasswordForUser, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<ForgetPasswordResponseModel> ForgetPasswordForCustomer(string EmailId)
        {
            var param = new DynamicParameters();
            param.Add("@EmailId", EmailId);
            return await QueryFirstOrDefaultAsync<ForgetPasswordResponseModel>(StoredProcedures.ForgetPasswordForCustomer, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SaveOTP(long UserId, int randomNumer, string EmailId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            param.Add("@OTP", randomNumer);
            param.Add("@EmailId", EmailId);
            return await ExecuteAsync<int>(StoredProcedures.SaveOTP, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> GetUserIDByEmail(string EmailId)
        {
            var param = new DynamicParameters();
            param.Add("@EmailId", EmailId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.GetUserIdByEmail, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<string> ResetPassword(long UserId, string EmailId, string Password, string Salt)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            param.Add("@EmailId", EmailId);
            param.Add("@Password", Password);
            param.Add("@Salt", Salt);
            return await QueryFirstOrDefaultAsync<string>(StoredProcedures.ResetPassword, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<string> VerificationCode(long UserId, int OTP, int PasswrodValid)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            param.Add("@OTP", OTP);
            param.Add("@Minute", PasswrodValid);
            return await QueryFirstOrDefaultAsync<string>(StoredProcedures.VerifyOTP, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> SuccessForgetPasswordChangeURL(PasswordChangeRequestModel passwordInfo)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", passwordInfo.UserId);
            param.Add("@Password", passwordInfo.Password);
            param.Add("@PasswordSalt", passwordInfo.PasswordSalt);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.ForgetPasswordChangeWithURL, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> SuccessForgetPasswordChangeURLForCustomer(PasswordChangeRequestModel passwordInfo)
        {
            var param = new DynamicParameters();
            param.Add("@CustomerId", passwordInfo.CustomerId);
            param.Add("@Password", passwordInfo.Password);
            param.Add("@PasswordSalt", passwordInfo.PasswordSalt);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.ForgetPasswordChangeWithURLForCustomer, param, commandType: CommandType.StoredProcedure);
        }
        #endregion

    }
}
