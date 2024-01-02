using Microsoft.AspNetCore.Http;



namespace SuperariLife.Model.Customer.CustomerAddress
{
    public class CustomerAddressResponseModel
    {


        public long CustomerId { get; set; }
        public long CustomerAddressId { get; set; } 
        public string? CustomerFirstname { get; set; }
        public string? CustomerLastname { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public long? StateId { get; set; }
        public string? StateName { get; set; }
        public long? CityId { get; set; }
        public string? CityName { get; set; }
        public string? PostalCode { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? TotalRecords { get; set; }
        public int? RowNumber { get; set; }
        public long? StatusOfResponse { get; set; }
    }

    public class CustomerAddressReqModelForAdmin
    {

        public long CustomerAddressId { get; set; }
        public long CustomerId { get; set; }
        public string? CustomerFirstname { get; set; }
        public string? CustomerLastname { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerAddress { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? CityId { get; set; }
        public bool IsDefaultAddress { get; set; }
        public string? PostalCode { get; set; }
        public long? UserId { get; set; }


    }

   
    public class CustomerAddressInsertUpdateResponseModel
    {
        public int? StatusOfInsertUpdate { get; set; }
    }
}
