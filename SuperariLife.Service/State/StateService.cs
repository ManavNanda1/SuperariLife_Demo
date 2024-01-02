using SuperariLife.Data.DBRepository.State;
using SuperariLife.Model.State;

namespace SuperariLife.Service.State
{
    public class StateService:IStateService
    {
        #region Fields
        private readonly IStateRepository _repository;
        #endregion

        #region Constructor
        public StateService(IStateRepository stateRepository)
        {
            _repository = stateRepository;
        }

        #endregion

        #region Fields
        public async  Task<int> AddUpdateState(StateRequestModel state)
        {
            return await _repository.AddUpdateState(state);
        }

        public async Task<int> DeleteState(int Id)
        {
            return await _repository.DeleteState(Id);
        }

        public async Task<List<StateModel>> GetStateList(int CountryId)
        {
           return await _repository.GetStateList(CountryId);
        }

        public async Task<StateModel> GetStateListById(int Id)
        {
           return await _repository.GetStateListById(Id);    
        }
        #endregion


    }
}
