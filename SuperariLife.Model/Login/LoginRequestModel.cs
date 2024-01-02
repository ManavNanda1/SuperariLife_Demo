using System.ComponentModel.DataAnnotations;

namespace SuperariLife.Model.Login
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Email id required!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password required!")]
        public string Password { get; set; }
    }

    public class SaltResponseModel
    {
        public string? PasswordSalt { get; set; }
        public string? Password { get; set; }
        public string? CustomerPassword { get; set; }           
        public bool? IsDeleted { get; set; }
        public int? Status { get; set; }    
    }

    public class ForgetPasswordRequestModel
    {
        [Required]
        public string EmailId { get; set; }
        public string ForgetPasswordUrl { get; set; }  
    }

    public class PasswordChangeRequestModel
    {
        public string? EncryptedUserId { get; set; }

        public string? EncryptedCustomerId { get; set; }    

        public string? Password { get; set; }

        public long? UserId { get; set; }    
        
        public long? CustomerId { get; set; }   
        public string? PasswordSalt { get; set; }
    }

    public class VerificationOTPRequestModel
    {
        [Required(ErrorMessage = "Email id required!")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Verification-code is required!")]
        public int OTP { get; set; }

    }
}
