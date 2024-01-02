using SuperariLife.Data.DBRepository.Event;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Event;

namespace SuperariLife.Service.Event
{
    public class EventService:IEventService
    {
        #region Fields
        private readonly IEventRepository _repository;
        #endregion

        #region Construtor
        public EventService(IEventRepository repository)
        {
            _repository = repository;
        }
        #endregion

        public async Task<long> DeleteEvent(CommonDeleteModel eventDeleteInfo)
        {
            return await _repository.DeleteEvent(eventDeleteInfo);  
        }

        public async Task<long> DeleteEventGalleryImage(string galleryImageName)
        {
            return await _repository.DeleteEventGalleryImage(galleryImageName); 
        }

        public async Task<long> DeleteEventQuestion(long eventQuestionId)
        {
            return await _repository.DeleteEventQuestion(eventQuestionId);
        }

        public async Task<EventResponseModel> GetEventByIdForAdmin(long eventId)
        {
            return await _repository.GetEventByIdForAdmin(eventId);
        }

        public async Task<List<EventCustomerResponseModel>> GetEventDetailOfCustomerParticipant(EventCustomerReqModel eventInfo)
        {
            return await _repository.GetEventDetailOfCustomerParticipant(eventInfo);
        }

        public async Task<List<QuestionEventResponseModel>> GetEventDetailOfQuestion(string questionId)
        {
            return await _repository.GetEventDetailOfQuestion(questionId);
        }

        public async Task<List<EventResponseModel>> GetEventListByAdmin(CommonPaginationModel info)
        {
           return await _repository.GetEventListByAdmin(info);  
        }
        public async Task<long> InsertUpdateEvent(EventReqModel eventInfo, List<EventGalleryImages> eventGalleryImagName)
        {
            return await _repository.InsertUpdateEvent(eventInfo, eventGalleryImagName);
        }
    }
}
