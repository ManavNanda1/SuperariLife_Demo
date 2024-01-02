

namespace SuperariLife.Model.State
{
    public class StateModel
    {
        public long StateId { get; set; }
        public string Statename { get; set; }
        public int CountryId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class StateRequestModel
    {
        public long StateId { get; set; }
        public string Statename { get; set; }
        public int CountryId { get; set; }
        public long UpdatedBy { get; set; }


    }
}
