using SuperariLife.Model.Login;

namespace SuperariLife.Data.DBRepository.Account
{
    public interface IAccountRepository
    {
        Task<long> ValidateUserTokenData(long Id, string jwtToken, DateTime TokenValidDate, bool IsSuperAdmin);
        Task<SaltResponseModel> GetUserSalt(string Email);

        Task<SaltResponseModel> GetCustomerSaltByEmail(string CustomerEmail);
        Task<LoginResponseModel> LoginUser(LoginRequestModel model);

        Task<CustomerLoginResponseModel> CustomerLogin(LoginRequestModel model);
        Task<long> UpdateLoginToken(string Token, long UserId);

        Task<long> UpdateLoginTokenForCustomer(string Token, long CustomerId);
        Task<long> LogoutUser(long UserId);

        Task<long> LogoutCustomer(long CustomerId); 
        Task<ForgetPasswordResponseModel> ForgetPassword(string EmailId);
        Task<ForgetPasswordResponseModel> ForgetPasswordForCustomer(string EmailId);
        Task<int> SaveOTP(long UserId, int randomNumer, string EmailId);
        Task<long> GetUserIDByEmail(string EmailId);
        Task<string> ResetPassword(long UserId, string EmailId, string Password, string Salt);
        Task<string> VerificationCode(long UserId, int OTP, int PasswrodValid);
        Task<string> ChangePassword(long UserId, string Password, string Salt);
        Task<string> ChangePasswordForCustomer(long CustomerId, string Password, string Salt);
        Task<long> SuccessForgetPasswordChangeURL(PasswordChangeRequestModel passwordInfo);
        Task<long> SuccessForgetPasswordChangeURLForCustomer(PasswordChangeRequestModel passwordInfo);
      
    }
}
