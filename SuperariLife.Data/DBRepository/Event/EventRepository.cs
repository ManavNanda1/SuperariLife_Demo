using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Config;
using SuperariLife.Model.Event;
using System.Data;



namespace SuperariLife.Data.DBRepository.Event
{
    public class EventRepository: BaseRepository, IEventRepository
    {
        #region Fields
        private readonly IConfiguration _config;
        #endregion

        #region Constructor
        public EventRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion
 

        public async Task<long> DeleteEvent(CommonDeleteModel eventDeleteInfo)
        {
            var param = new DynamicParameters();
            param.Add("@EventId", eventDeleteInfo.Id);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.DeleteEvent, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> DeleteEventGalleryImage(string galleryImageName)
        { 
            var param = new DynamicParameters();
            param.Add("@GalleryImageName", galleryImageName);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.DeleteEventGalleryImage, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> DeleteEventQuestion(long eventQuestionId)
        {
            var param = new DynamicParameters();
            param.Add("@EventQuestionId", eventQuestionId);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.DeleteEventQuestion, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<EventResponseModel> GetEventByIdForAdmin(long eventId)
        {
            var param = new DynamicParameters();
            param.Add("@EventId", eventId);
            return await QueryFirstOrDefaultAsync<EventResponseModel>(StoredProcedures.GetEventByIdForAdmin, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<EventCustomerResponseModel>> GetEventDetailOfCustomerParticipant(EventCustomerReqModel eventInfo)
        {
            var param = new DynamicParameters();
            param.Add("@EventId", eventInfo.EventId);
            param.Add("@StrSearch", eventInfo.SearchStringForCustomer);
            var data = await QueryAsync<EventCustomerResponseModel>(StoredProcedures.GetEventDetailOfCustomerParticipant, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public  async Task<List<QuestionEventResponseModel>> GetEventDetailOfQuestion(string questionId)
        {
            var param = new DynamicParameters();
            param.Add("@QuestionIdString", questionId);
            var data = await QueryAsync<QuestionEventResponseModel>(StoredProcedures.GetEventDetailOfQuestion, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<List<EventResponseModel>> GetEventListByAdmin(CommonPaginationModel info)
        {
            var param = new DynamicParameters();
            param.Add("@pageIndex", info.PageNumber);
            param.Add("@pageSize", info.PageSize);
            param.Add("@orderBy", info.SortColumn);
            param.Add("@sortOrder", info.SortOrder);
            param.Add("@strSearch", info.StrSearch.Trim());
        
            var data = await QueryAsync<EventResponseModel>(StoredProcedures.GetEventListByAdmin, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        public async Task<long> InsertUpdateEvent(EventReqModel eventInfo, List<EventGalleryImages> eventGalleryImagName)
        {
            DataTable dtQuestion = new DataTable("tbl_EventQuestion");
            DataTable dtDates = new DataTable("tbl_EventDate");
            DataTable dtGallerImage = new DataTable("tbl_EventGalleryImage");
            dtQuestion.Columns.Add("QuestionId");
            dtDates.Columns.Add("EventDate");
            dtGallerImage.Columns.Add("EventGalleryImage");
            if (eventInfo.QuestionId != null && eventInfo.QuestionId.Count > 0)
            {
                foreach (var item in eventInfo.QuestionId)
                {
                    DataRow dtRow = dtQuestion.NewRow();
                    dtRow["QuestionId"] = item;
                    dtQuestion.Rows.Add(dtRow);
                }
            }
            if (eventInfo.GalleryImagesFile != null && eventInfo.GalleryImagesFile.Count > 0 && (eventGalleryImagName != null || eventGalleryImagName.Count > 0))
            {
                foreach (var item in eventGalleryImagName)
                {
                    DataRow dtRow = dtGallerImage.NewRow();
                    if (item.EventGalleryImageName != null || item.EventGalleryImageName != "")
                    {
                        dtRow["EventGalleryImage"] = item.EventGalleryImageName;
                    }
                    dtGallerImage.Rows.Add(dtRow);
                }
            }
            if (eventInfo.EventDate != null && eventInfo.EventDate.Count > 0)
            {
                foreach (var item in eventInfo.EventDate)
                {
                    DataRow dtRow = dtDates.NewRow();
                    dtRow["EventDate"] = item;
                    dtDates.Rows.Add(dtRow);
                }
            }
            var param = new DynamicParameters();
            param.Add("@EventId", eventInfo.EventId);
            param.Add("@CreatedBy", eventInfo.CreatedBy);
            param.Add("@EventImage", eventInfo.EventImage);
            param.Add("@EventName", eventInfo.EventName);
            param.Add("@EventHostedBy", eventInfo.EventHostedBy);
            param.Add("@EventFees", eventInfo.EventFees);
            param.Add("@EventMaxParticipantLimit", eventInfo.EventMaxParticipantLimit);
            param.Add("@EventHostNumber", eventInfo.EventHostNumber);
            param.Add("@EventStartTime", eventInfo.EventStartTime);
            param.Add("@EventEndTime", eventInfo.EventEndTime);
            param.Add("@EventDescription", eventInfo.EventDescription);
            param.Add("@EventLatitudeAndLogitude", eventInfo.EventLatitudeAndLogitude);
            param.Add("@EventAddress", eventInfo.EventAddress);
            param.Add("@EventCityId", eventInfo.EventCityId);
            param.Add("@EventStateId", eventInfo.EventStateId);
            param.Add("@EventCountryId", eventInfo.EventCountryId);
            param.Add("@EventZipCode", eventInfo.EventZipCode);
            param.Add("@GalleryImages", dtGallerImage.AsTableValuedParameter("[dbo].[tbl_EventGalleryImage]"));
            param.Add("@EventQuestions", dtQuestion.AsTableValuedParameter("[dbo].[tbl_EventQuestion]"));
            param.Add("@EventDates", dtDates.AsTableValuedParameter("[dbo].[tbl_EventDate]"));
            var result = await QueryFirstOrDefaultAsync<long>(StoredProcedures.InsertUpdateEvent, param, commandType: CommandType.StoredProcedure);
            return result;

        }
    }
}
