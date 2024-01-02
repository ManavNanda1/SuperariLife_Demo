using SuperariLife.Model.State;

namespace SuperariLife.Data.DBRepository.State
{
    public interface IStateRepository
    {
        Task<int> AddUpdateState(StateRequestModel state);
        Task<int> DeleteState(int Id);
        Task<List<StateModel>> GetStateList(int CountryId);
        Task<StateModel> GetStateListById(int Id);
       
    }
}
