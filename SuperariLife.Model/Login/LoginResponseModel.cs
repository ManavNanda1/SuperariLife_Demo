using System.ComponentModel.DataAnnotations;

namespace SuperariLife.Model.Login
{
    public class CustomerLoginResponseModel
    {
        public long CustomerId { get; set; }
        public string Customerfirstname { get; set; }
        public string Customerlastname { get; set;}
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public bool? IsActive { get; set; } 
        public bool? IsDeleted { get; set; }
        public string Token { get; set; }
        public string? CustomerImage { get; set; }      

    }
    public class LoginResponseModel
    {
        public long UserId { get; set; }
        public int RoleManagementId { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime TokenCreatedDate { get; set;}
        public string PasswordSalt { get; set;}
        public int UserStatus { get; set; }
        public string? UserImage { get; set; }

    }

    public class ForgetPasswordResponseModel
    {
        public long UserId { get; set; }
        public long CustomerId { get; set; }    
        public DateTime LastForgetPasswordSend { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
    }



    public class ResetPasswordRequestModel
    {
        [Required(ErrorMessage = "Email Id required!")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "The Create Password is required.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "The Confirm Password is required.")]
        public string ConfirmPassword { get; set; }
    }
    public class ChangePasswordRequestModel
    {
        //[Required(ErrorMessage = "Id required!")]
        //public long Id { get; set; }
        [Required(ErrorMessage = "The Old Password is required.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "The Create Password is required.")]
        public string CreatePassword { get; set; }
        [Required(ErrorMessage = "The Confirm Password is required.")]
        public string ConfirmPassword { get; set; }
    }
}
