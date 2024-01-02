using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Config;
using SuperariLife.Model.State;
using System.Data;


namespace SuperariLife.Data.DBRepository.State
{
    public class StateRepository : BaseRepository, IStateRepository
    {
        #region Fields
        private IConfiguration _config;

        #endregion

        #region Constructor
        public StateRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        public async  Task<int> AddUpdateState(StateRequestModel state)
        {
            var param = new DynamicParameters();
            param.Add("@StateId", state.StateId);
            param.Add("@Statename", state.Statename);
            param.Add("@CountryId", state.CountryId);
            param.Add("@UserId", state.UpdatedBy);
            return await QueryFirstOrDefaultAsync<int>(StoredProcedures.InsertUpdateState, param, commandType: CommandType.StoredProcedure);
        }
        public async  Task<int> DeleteState(int Id)
        {
            var param = new DynamicParameters();
            param.Add("@StateId", Id);
            return await QueryFirstOrDefaultAsync<int>(StoredProcedures.DeleteState,param,commandType: CommandType.StoredProcedure);
        }
        public async  Task<List<StateModel>> GetStateList(int CountryId)
        {
            var param = new DynamicParameters();
            param.Add("@CountryId", CountryId);
            var data = await QueryAsync<StateModel>(StoredProcedures.GetStateByCountryId, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        public async Task<StateModel> GetStateListById(int Id)
        {
            var param = new DynamicParameters();
            param.Add("@StateId ", Id);
            return await QueryFirstOrDefaultAsync<StateModel>(StoredProcedures.GetStateById, param, commandType: CommandType.StoredProcedure);
        }
    }
}
