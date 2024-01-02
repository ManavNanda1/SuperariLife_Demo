using Microsoft.AspNetCore.Http;
using SuperariLife.Model.Event;

namespace SuperariLife.Model.SettingPages
{

    // About Page

    #region AboutSection

    public class AboutPageSectionReqModel
    { 
        public int? AboutPageSectionId { get; set; }
        public int? AboutPageSectionLayoutTypeId { get; set; }  
        public bool? AboutPageSectionIsSetupFreeButtonEnable { get; set; } 
        public string? AboutPageSectionContent { get; set; }
        public string? AboutPageSectionContentTitle { get; set; }   
        public IFormFile? AboutPageSectionImage { get; set; }   
        public string? AboutPageSectionImageName { get; set; }  
        public long? UserId { get; set; }   
    }

    public class AboutPageSectionResponseModel
    {
        public int? AboutPageSectionId { get; set; }
        public int? AboutPageSectionLayoutTypeId { get; set; }
        public string? AboutPageSectionLayoutTypeOptionName { get; set; }    
        public bool? AboutPageSectionIsSetupFreeButtonEnable { get; set; }
        public string? AboutPageSectionContent { get; set; }
        public string? AboutPageSectionContentTitle { get; set; }
        public string? AboutPageSectionImage { get; set; }
        public bool? IsActive { get; set; } 
        public long? RowNumber { get; set; }
        public long? TotalRecords { get; set; }
    }
    #endregion
    #region AboutImage
    public class AboutImageReqModel
    {
        public int? AboutPageImageId { get; set; }
        public List<IFormFile> AboutPageImages { get; set; }
        public List<string>? AboutPageImagesName { get; set; }
        public long? UserId { get; set; }
    }

    public class AboutImageResponseModel 
    {
        public int? AboutPageImageId { get; set; }
        public string? AboutPageImages { get; set; }
        public long? RowNumber { get; set; }
        public long? TotalRecord { get; set; }
    }

    public class CommonAboutPageDeleteModel
    {
        public string? ImageName { get; set; } 
         public int? StatusOfDelete { get; set; }  
        
    }

    public class AboutInsertUpdateResponseModel
    {
        public string? AboutPageSectionImageName { get; set; }
        public int? StatusOfInsertUpdate { get; set; }
    }

    public class PageSectionLayoutTypeModel
    {
        public int? AboutPageSectionLayoutTypeId { get; set; }
        public string? AboutPageSectionLayoutOption { get; set; }
    }
    #endregion

    //Privacy Page
    public class PrivacyPageReqModel
    {
        public int? PrivacyPageId { get; set; }
        public string? PrivacyPageContent { get; set; }
        public long? UserId { get; set; }   

    }

    public class PrivacyPageResponseModel
    {
        public int? PrivacyPageId { get; set; }
        public string? PrivacyPageContent { get; set; }
    }


    //Terms And Condition Page
    public class TermsAndConditionPageReqModel
    {
        public int? TermsAndConditionPageId { get; set; }
        public string? TermsAndConditionPageContent { get; set; }
        public long? UserId { get; set; }
    }

    public class TermsAndConditionPageResponseModel
    {
        public int? TermsAndConditionPageId { get; set; }
        public string? TermsAndConditionPageContent { get; set; }
    }


    //Testimonial Page
    public class TestimonialPagesReviewReqModel
    {
        public int? TestimonialPageReviewId { get; set; }
        public string? TestimonialPageReviewContent { get; set; }
        public string? TestimonialPageReviewPersonName { get;set; }
        public long? UserId { get; set; }
    }

    public class TestimonialPagesReviewResponseModel
    {
        public bool? IsActive { get; set; }
        public int? TestimonialPageReviewId { get; set; }
        public string? TestimonialPageReviewContent { get; set; }
        public string? TestimonialPageReviewPersonName { get; set; }

        public long? rowNumber { get; set; }

        public long? TotalRecords { get; set; } 
    }


}
