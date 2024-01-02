using System.ComponentModel.DataAnnotations;

namespace SuperariLife.Model.RoleManagement
{
    public class RoleManagementResponseModel
    {
        [Key]
        public  long?  RoleManagementId { get; set; }   
        public string? RoleName { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }   
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? TotalRecords { get; set; }
        public int? RowNumber { get; set; }
     
    }

    public class RoleManagementReqModel
    {
        public long? RoleManagementId { get; set;}
        public string? RoleName { get; set;}
        public long? UserId { get; set; }
    }

    public class RoleManagementDeleteResponseModel
    {
        public string? RoleName { get; set; }
       public int? StatusOfRole {  get; set; } 
    }



}
