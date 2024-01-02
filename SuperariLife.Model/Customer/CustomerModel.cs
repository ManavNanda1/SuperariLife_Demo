using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SuperariLife.Model.Customer
{
    public  class CustomerResponseModel
    {

            [Key]
            public long CustomerId { get; set; }
            public string? Customerfirstname { get; set; }
            public string? Customerlastname { get; set; }
            public string?   CustomerPassword { get; set; }
            public string? CustomerEmail { get; set; }
            public string?   CustomerAddress { get; set; }
            public string? CustomerPhoneNumber { get; set; }
            public string?   CustomerImage { get; set; }
            public string?   CustomerImageUrl { get; set; }
            public long? CurrentlyActivePaymentId { get; set; }

            public string? PaymentTransactionId { get;set; }      
            public int?   CountryId { get; set; }
            public string? CountryName { get; set; }
            public long? StateId { get; set; }

            public string? StateName { get; set; }
            public long?   CityId { get; set; }

            public string? CityName { get; set; }   
            public string? PostalCode { get; set; }
            public bool?  IsActive { get; set; }
            public bool? IsDeleted { get; set; }
            public DateTime?  DeletedDate { get; set; }
            public DateTime? CreatedDate { get; set; }
            public long?   CreatedBy { get; set; }
            public long?  UpdatedBy { get; set; }
            public DateTime?  UpdatedDate  { get; set; }
            public long?  CurrentlyActivePaymentTypeId { get; set; }

            public string? PaymentTypeName { get; set; }
            public int? TotalRecords { get; set; }
            public int? RowNumber { get; set; }
            public long? StatusOfResponse { get; set; }
        }

        public class CustomerReqModelForAdmin
        {
            public long CustomerId { get; set; }
            public IFormFile? CustomerImage { get; set; }
            public string? Customerfirstname { get; set; }
            public string? Customerlastname { get; set; }
            public string? CustomerPhoneNumber { get; set; }
            public string? CustomerEmail { get; set; }
            public string? CustomerAddress { get; set; }
            public long? CountryId { get; set; }
            public long? StateId { get; set; }
            public long? CityId { get; set; }
            public string? PostalCode { get; set; }
            public string? ImageName { get; set;}
           public long? UserId { get; set; }
           public string? CustomerPassword { get; set;}
           public string? CustomerPasswordSalt { get; set;}
            
        }

    public class CustomerDeleteResponseModel
    {
        public string? CustomerImageName { get; set; }
        public int? StatusOfDelete { get; set; }
    }
    public class CustomerInsertUpdateResponseModel
    {
        public string? CustomerImageName { get; set; }
        public int? StatusOfInsertUpdate { get; set; }
    }

}
