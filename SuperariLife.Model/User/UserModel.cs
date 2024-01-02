using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace SuperariLife.Model.User
{
    public class ResponseUserModel
    {
        [Key]
        public long? UserId { get; set; }    
        public long? RoleManagementId { get; set; }
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; } 
        public string? Password  { get; set; } 
        public string? Token   { get; set; } 
        public bool? IsDeleted  { get; set; } 
        public DateTime? DeletedDate  { get; set; }
        public DateTime?  CreatedDate  { get; set; } 
        public long? CreatedBy  { get; set; } 
        public long? UpdatedBy  { get; set; }
        public DateTime?  UpdatedDate  { get; set; } 
        public bool? IsActive   { get; set; } 
        public DateTime? TokenCreatedDate  { get; set; } 
        public string? PasswordSalt  { get; set; } 
        public string? UserImage  { get; set; } 

        public string? UserImageUrl { get; set; }
        public string? UserMobileNumber  { get; set; } 
        public string? UserAddress  { get; set; } 
        public int? UserCountryId  { get; set; } 
        public string? CountryName { get;set; }
        public long? UserStateId  { get; set; } 
        public string?  StateName { get; set; }  
        public long? UserCityId  { get; set; } 
        public string? CityName { get; set; }   
        public string? UserPostalCode  { get; set; } 
        public string? RoleName { get; set; }   
        public int? TotalRecords { get; set; }
        public int? RowNumber { get; set; }
        public long? StatusOfResponse { get; set; }
    }

    public class UserReqModelForAdmin
    {
        public long? UserId { get; set; }
        public long? RoleManagementId { get; set; }
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Password { get; set; }
        public long? CreatedBy { get; set; }         
        public string? PasswordSalt { get; set; }
        public IFormFile? UserImage { get; set; }
        public string? ImageName { get; set; }
        public string? UserAddress { get; set; }
        public int? UserCountryId { get; set; }
        public long? UserStateId { get; set; }
        public long? UserCityId { get; set; }
        public string? UserPostalCode { get; set; }

        public string? UserMobileNumber { get; set; }
    }

    public class UserReqModelForUser: UserReqModelForAdmin
    {   
    }

    public class UserChangePasswordRequestModel
    {
        [Required(ErrorMessage = "The Old Password is required.")]
        [MaxLength(15, ErrorMessage = "Maxlength is 15 characters.")]
        public string OldPassword { get; set; }
        [MaxLength(15, ErrorMessage = "Maxlength is 15 characters.")]
        [Required(ErrorMessage = "The Create Password is required.")]
        public string CreatePassword { get; set; }
        [Required(ErrorMessage = "The Confirm Password is required.")]
        [MaxLength(15, ErrorMessage = "Maxlength is 15 characters.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserDeleteResponseModel
    {
        public string? UserImageName { get; set; }
        public int? StatusOfDelete { get; set; }
    }

    public class UserInsertUpdateResponseModel
    {
        public string? UserImageName { get; set; }
        public int? StatusOfInsertUpdate { get; set; }
    }


}
