using System.ComponentModel.DataAnnotations;


namespace SuperariLife.Model.PaymentType
{
    public class PaymentTypeResponseModel
    {
        [Key]
        public long? PaymentTypeId { get; set; }
        public string? PaymentTypeName { get; set; }
        public  string? PaymentText { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? TotalRecords { get; set; }
        public int? RowNumber { get; set; }
        public decimal? Amount { get; set; }    
        public int? ExpiryDays { get; set; }
        public int? NoOfPass { get; set; }
        public int? TypeOfPass { get; set; }   
    }

    public class PaymentTypeReqModel
    {
       public long PaymentTypeId { get; set; }
       public string? PaymentTypeName { get; set; }
       public long? UserId { get; set; } 
       public string? PaymentText { get; set; }
       public decimal Amount { get; set; }
       public int ExpiryDays { get; set; }
       public int NoOfPass { get;set; }
      public int? TypeOfPass { get; set; }
    }
}
