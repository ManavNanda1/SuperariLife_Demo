
namespace SuperariLife.Model.Question
{
    public class QuestionResponseModel
    {
        public long QuestionId { get; set; }
        public string? Question { get; set; }
        public long? QuestionTypeId { get; set; }
        public string? QuestionTypeName { get; set; }
        public string QuestionOptions { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; } 
        public DateTime? DeletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? TotalRecords { get; set; }
        public int? RowNumber { get; set; }
    }

    public class QuestionReqModel
    {
        public long QuestionTypeId { get; set; }
        public long QuestionId { get; set; }
        public string Question { get; set; }
        public long UserId { get; set; }
        public List<QuestionOptionModel>? QuestionOptionObj { get; set; }
    }

    public class QuestionOptionModel
    {
        //public long QuestionId { get; set; }
        public string QuestionOption { get; set; }
    }

    public class QuestionTypeResponseModel
    {
        public long QuestionTypeId { get; set; }
        public string? QuestionTypeName { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

    }
}