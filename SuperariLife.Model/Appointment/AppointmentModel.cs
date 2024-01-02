namespace SuperariLife.Model.Appointment
{
    public  class AppointmentReqModelByCustomer
    {
        public string? AppointmentAddress { get; set; }
        public long AppointmentId { get; set; }
        public long AppointmentTypeId { get; set; }  
        public DateTime AppointmentDate { get; set; }    
        public string AppointmentTime { get; set;}
        public string CustomerMessage { get; set; }  
        public long?  CustomerId { get; set; }
        public int? AppointmentCountryId { get; set; }
        public long? AppointmentCityId { get; set; }
        public long? AppointmentStateId { get; set; }
        public string? Zipcode { get; set; }

    }

    public class AppointmentReqModelByAdmin
    {
        public long AppointmentId { get; set; }
        public long AppointmentTypeId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string AdminMessage { get; set; }
        public decimal? AppointmentChargeAmount { get; set; }    
        public string? AppointmentAddress { get; set; } 
        public string? AppointmentLink { get; set;}
        public int? AppointmentCountryId { get; set;}
        public long? AppointmentCityId { get; set; }
        public int? IsAppointmentAccepted { get; set; }
        public long? AppointmentStateId { get; set; }
        public long? UserId { get; set; }
        public string? Zipcode { get; set; }    
   
     

    }

    public class AppointmentResponseModelForAdmin
    {

        public string? AppointmentAddress { get; set; }

        public string? AdminMessage { get; set; }   
        public string? AppointmentCountry { get; set; }
        public string? AppointmentCity { get; set; }
        public decimal AppointmentChargeAmount { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public long? AppointmentId { get; set; }
        public string? AppointmentState { get; set; }
        public string? AppointmentTime { get; set; }
        public string? AppointmentLink { get; set; }
        public long? AppointmentTypeId { get; set; }
        public string? AppointmentTypeName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerEmail { get; set; }  
        public string? CustomerImage { get; set; }
        public int? AppointmentCountryId { get; set; }
        public long? AppointmentCityId { get; set; }
        public long? CustomerId { get; set; }   
        public string? CustomerMessage { get; set; }
        public string? CustomerPhoneNumber { get; set; }    
        public string? CustomerName { get; set; }      
        public int? IsAppointmentAccepted { get; set; } 
        public long? RowNumber { get; set; }
        public long? AppointmentStateId { get; set; }
        public long? TotalRecords { get; set; }        
        public string? ZipCode { get; set; }    
    }


    public class AppointmentResponseModelForCustomer
    {
        public string? AppointmentAddress { get; set; }
        public string? AppointmentCountry { get; set; }
        public string? AppointmentCity { get; set; }
        public decimal AppointmentChargeAmount { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? AdminMessage { get; set; }
        public long? AppointmentId { get; set; }
        public string? AppointmentState { get; set; }
        public string? AppointmentTime { get; set; }
        public string? AppointmentLink { get; set; }
        public long? AppointmentTypeId { get; set; }
        public string? AppointmentTypeName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerImage { get; set; }
        public int? AppointmentCountryId { get; set; }
        public long? AppointmentCityId { get; set; }
        public string? CustomerMessage { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public string? CustomerName { get; set; }
        public int? IsAppointmentAccepted { get; set; }
        public long? RowNumber { get; set; }
        public long? AppointmentStateId { get; set; }
        public long? TotalRecords { get; set; }
        public string? ZipCode { get; set; }
        public bool? AppointmentTransactionStatus { get; set; } 
        public DateTime? AppointmentPaymentTransactionDateTime { get; set; }
        public string? AppointmentPaymentMode { get; set; } 
        public string? AppointmentTransactionId { get;  set; }

    }

    public class AppointmentResponseDropDownModel
    {
        public long? AppointmentTypeId { get; set; }
        public string? AppointmentTypeName { get; set; }
    } 

}
