using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Event;

namespace SuperariLife.Data.DBRepository.Event
{
    public interface IEventRepository
    {
        Task<long> DeleteEvent(CommonDeleteModel eventId);
        Task<long> DeleteEventGalleryImage(string galleryImageName);
        Task<long> DeleteEventQuestion(long eventQuestionId);
        Task<List<EventResponseModel>> GetEventListByAdmin(CommonPaginationModel info);
        Task<EventResponseModel> GetEventByIdForAdmin(long eventId);  
        Task<List<EventCustomerResponseModel>> GetEventDetailOfCustomerParticipant(EventCustomerReqModel eventInfo);
        Task<List<QuestionEventResponseModel>> GetEventDetailOfQuestion(string questionId);
        Task<long> InsertUpdateEvent(EventReqModel eventInfo, List<EventGalleryImages> eventGalleryImagName);


    }
}
