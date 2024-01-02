using System.ComponentModel.DataAnnotations;


namespace SuperariLife.Model.CouponCode
{
    public class CouponCodeResponseModel
    {
        [Key]
        public long CouponCodeId { get; set; }
        public long? CustomerId { get; set; }   
        public string? CouponCode { get; set; }
        public DateTime? StartDateOfCoupon { get; set; }
        public DateTime? ExpireDateOfCoupon { get; set; }
        public int CouponType { get; set; }
        public decimal DiscountAmountORPercentage { get; set; }
        public string? CustomerName { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? TotalRecords { get; set; }
        public int? RowNumber { get; set; }
        public string? CouponCodeName { get; set; }
        public string? CustomerPhoneNumber { get;set; }
        public string? CustomerEmail { get; set; }  
        public string? CustomerImage { get; set; }      
        public int? TotalParticipant { get; set; }
        public int? DiscountType { get; set; }  
    }

    public class CouponCodeReqModel
    {
        [Key]
        public long CouponCodeId { get; set; }
        public string? CouponCode { get; set; }
        public DateTime? StartDateOfCoupon { get; set; }
        public DateTime? ExpireDateOfCoupon { get; set; }
        public int? CouponType { get; set; }
        public decimal? DiscountAmountORPercentage { get; set; }
        public long? UserId { get; set; }
        public int? DiscountType { get; set; }
    }
    public class CouponCodeReqDetailModel
    {
        public long CouponCodeId { get; set;}
        public string? CouponCodeSearchString { get;set; }
    }

}
