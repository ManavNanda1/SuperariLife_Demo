

namespace SuperariLife.Model.CouponCode
{
    public  class RewardCouponCodeReqModel
    { 
        public long  CouponCodeId { get; set; } 
        public long  CustomerId { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CouponCode { get; set; }
        public long RewardCouponCodeId { get; set; }
        public long  UserId { get; set; }  
        public DateTime ExpireDateOfCoupon { get; set; }    
    }

    public class RewardCouponCodeResponseModel
    {
        public long? CouponCodeId { get; set; }
        public long? CustomerId { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CouponCode { get; set; }
        public string? CustomerImage { get; set; } 
        public string? CustomerName { get; set; } 
        public string? CustomerPhoneNumber { get; set; }
        public int? CouponType { get; set; }
        public decimal? DiscountAmountORPercentage { get; set; }
        public int? DiscountType { get; set; }
        public DateTime? ExpireDateOfCoupon { get; set; }
        public long? RewardCouponCodeId { get; set; }
        public long? RowNumber { get;set; }
        public DateTime? StartDateOfCoupon { get; set; }
        public long? TotalRecords { get; set; } 
    }


    public class CustomerListForSendingCouponCodeRewardResponseModel
    {
        public long? CustomerId { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerImage { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public long? TotalNumberOfSessionAttend { get; set; }   
    }

}
