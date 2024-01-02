using SuperariLife.Data.DBRepository.Account;
using SuperariLife.Model.Login;


namespace SuperariLife.Service.Account
{
    public class AccountService : IAccountService
    {
        #region Fields
        private readonly IAccountRepository _repository;
        #endregion

        #region Construtor
        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }
        #endregion

        public async Task<long> ValidateUserTokenData(long Id, string jwtToken, DateTime TokenValidDate, bool IsSuperAdmin)
        {
            return await _repository.ValidateUserTokenData(Id, jwtToken, TokenValidDate,IsSuperAdmin);
        }

        public async Task<SaltResponseModel> GetUserSalt(string Email)
        {
            return await _repository.GetUserSalt(Email);
        }
        public async Task<LoginResponseModel> LoginUser(LoginRequestModel model)
        {
            return await _repository.LoginUser(model);
        }
        public async Task<long> UpdateLoginToken(string Token, long UserId)
        {
            return await _repository.UpdateLoginToken(Token, UserId);
        }

        public async Task<long> LogoutUser(long UserId)
        {
            return await _repository.LogoutUser(UserId);
        }

        public async Task<ForgetPasswordResponseModel> ForgetPassword(string EmailId)
        {
            return await _repository.ForgetPassword(EmailId);
        }

        public async Task<int> SaveOTP(long UserId, int randomNumer, string EmailId)
        {
            return await _repository.SaveOTP(UserId, randomNumer, EmailId);
        }
        public async Task<long> GetUserIDByEmail(string EmailId)
        {
            return await _repository.GetUserIDByEmail(EmailId);
        }
        public async Task<string> ResetPassword(long UserId, string EmailId, string Password, string Salt)
        {
            return await _repository.ResetPassword(UserId, EmailId, Password, Salt);
        }

        public async Task<string> VerificationCode(long UserId, int OTP, int PasswrodValid)
        {
            return await _repository.VerificationCode(UserId, OTP, PasswrodValid);
        }

        public async Task<string> ChangePassword(long UserId, string Password, string Salt)
        {
           return await _repository.ChangePassword(UserId, Password, Salt); 
        }

        public async Task<long> SuccessForgetPasswordChangeURL(PasswordChangeRequestModel passwordInfo)
        {
           return await _repository.SuccessForgetPasswordChangeURL(passwordInfo);
        }

        public async Task<CustomerLoginResponseModel> CustomerLogin(LoginRequestModel model)
        {
            return await _repository.CustomerLogin(model);  
        }

        public async Task<long> UpdateLoginTokenForCustomer(string Token, long CustomerId)
        {
            return await _repository.UpdateLoginTokenForCustomer(Token, CustomerId);    
        }

        public async Task<SaltResponseModel> GetCustomerSaltByEmail(string CustomerEmail)
        {
            return await _repository.GetCustomerSaltByEmail(CustomerEmail);
        }

        public async Task<long> LogoutCustomer(long CustomerId)
        {
            return await _repository.LogoutCustomer(CustomerId);
        }

        public async Task<ForgetPasswordResponseModel> ForgetPasswordForCustomer(string EmailId)
        {
            return await _repository.ForgetPasswordForCustomer(EmailId);    
        }

        public async Task<long> SuccessForgetPasswordChangeURLForCustomer(PasswordChangeRequestModel passwordInfo)
        {
            return await _repository.SuccessForgetPasswordChangeURLForCustomer(passwordInfo);
        }

        public async Task<string> ChangePasswordForCustomer(long CustomerId, string Password, string Salt)
        {
            return await _repository.ChangePasswordForCustomer(CustomerId, Password, Salt);
        }
    }
}
