using SuperariLife.Model.State;


namespace SuperariLife.Service.State
{
    public interface IStateService
    {
        Task<int> AddUpdateState(StateRequestModel country);
        Task<int> DeleteState(int Id);
        Task<List<StateModel>> GetStateList(int state);
        Task<StateModel> GetStateListById(int Id);

   
    }
}
