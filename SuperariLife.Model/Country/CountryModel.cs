namespace SuperariLife.Model.Country
{
    public class CountryModel
    {
        public int CountryId { get; set; }
        public string Countryname { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

     public class CountryRequestModel
     {
        public int CountryId { get; set; }
        public string Countryname { get; set; }
        public long UpdatedBy { get; set; }

     }
}
