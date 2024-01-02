using Microsoft.AspNetCore.Http;

namespace SuperariLife.Model.Event
{


    public class EventResponseModel
    {
        public long? EventId { get; set; }
        public string? EventImage { get; set; }
        public string? EventImageUrl { get; set; }
        public string? EventName { get; set; }
        public long? EventHostedBy { get; set; }
        public string? EventHostName { get; set; }
        public decimal? EventFees { get; set; }
        public int? EventMaxParticipantLimit { get; set; }
        public string? EventHostNumber { get; set; }
        public string? EventStartTime { get; set; }
        public string? EventEndTime { get; set; }
        public string? EventGalleryImages { get; set; }        
        public string? EventDates { get; set; }  
        public string? EventDescription { get; set; }
        public string? EventLatitudeAndLongitude { get; set; }
        public string? EventAddress { get; set; }
        public long? EventCityId { get; set; }
        public string? EventCity { get; set; }
        public long? EventStateId { get; set; }
        public string? EventState { get; set; } 
        public int?  EventCountryId { get; set; }
        public string? EventCountry { get; set; }   
        public string? EventZipCode { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } 
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? EventParticipant { get; set; }  
        public bool? IsDeleted { get; set; } 
        public bool? IsActive { get; set; }
        public string? EventLatitudeAndLogitude { get;set; }
        public int? TotalRecords { get; set; }
        public int? RowNumber { get; set; }
        public string? EventDuration { get;set; }
        public string? EventQuestions { get; set; }  
        public long? TotalParticipant { get;set; }
       public  List<EventGalleryImages> galleryImagesPath { get; set; }    

    }

    public class EventReqModel
    {
        public long? EventId { get; set; }
        public long? CreatedBy { get; set; }   
        public string? EventImage { get; set; }
        public string? EventName { get; set;}
        public long? EventHostedBy { get; set; }       
        public  decimal? EventFees { get; set; }       
        public int? EventMaxParticipantLimit { get; set; }  
        public string? EventHostNumber { get; set; }    
        public string? EventStartTime { get; set; } 
        public string? EventEndTime { get; set; }
        public string? EventDescription { get; set; }
        public string? EventLatitudeAndLogitude { get; set; }
        public string? EventAddress { get; set; }
        public long?  EventCityId { get; set; }
        public long? EventStateId { get; set; }
        public int? EventCountryId { get; set; }
        public string? EventZipCode { get; set; }
        public IFormFile? EventImageFile { get; set; }       
        public List<IFormFile>? GalleryImagesFile { get; set; }

        public List<EventGalleryImages>? EventGalleryImagesName { get; set; }

        public List<long>? QuestionId { get; set; }  
        
        public List<DateTime>? EventDate { get; set; }
  
    }

    public class EventGalleryImages
    {
        public string? EventGalleryImagePath { get; set; }  
        public string EventGalleryImageName { get; set; } 
    }
    public class EventQuestion
    {
        public long QuestionId { get; set;}
    }

    public class EventDates
    {
        public DateTime EventDate { get; set; }
    }

    public class QuestionEventResponseModel
    {
         public long QuestionId { get; set;}
         public string? Question { get; set; }
        public string? QuestionTypeName { get; set; }
         public long? QuestionTypeId { get; set;}
         public string? QuestionOptions { get; set; }    
    }

    public class EventCustomerResponseModel
    {
        public long? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public long? NoOfSeatReserved { get; set; } 
        public decimal? TotalAmount { get; set; }
        public string? CustomerImage { get; set; }

        public string? CustomerEmail { get; set; }  
    }

    public class EventCustomerReqModel
    {
        public long? EventId { get; set; }
        public string? SearchStringForCustomer { get; set; }
    }

}
