
namespace SuperariLife.Model.City
{
    public class CityModel
    {
        public long CityId { get; set; }
        public string Cityname { get; set; }
        public long StateId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
    }
    
    public class CityRequestModel
    {
        public long CityId { get; set; }
        public string Cityname { get; set; }
        public long StateId { get; set; }
        public long UpdatedBy { get; set; }

    }
}
