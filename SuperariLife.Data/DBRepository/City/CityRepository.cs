using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.City;
using SuperariLife.Model.Config;
using System.Data;

namespace SuperariLife.Data.DBRepository.City
{
    public class CityRepository:BaseRepository,ICityRepository
    {

        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public CityRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }


        #endregion

        public async Task<int> AddUpdateCity(CityRequestModel city)
        {
            var param = new DynamicParameters();
            param.Add("@CityId", city.CityId);
            param.Add("@CityName", city.Cityname);
            param.Add("@StateId", city.StateId);
            param.Add("@UserId", city.UpdatedBy);
            return await QueryFirstOrDefaultAsync<int>(StoredProcedures.InsertUpdateCity, param, commandType: CommandType.StoredProcedure);
        }
        public async Task<int> DeleteCity(int Id)
        {
            var param = new DynamicParameters();
            param.Add("@CityId", Id);
            return await QueryFirstOrDefaultAsync<int>(StoredProcedures.DeleteCity, param, commandType: CommandType.StoredProcedure);
        }
        public async Task<List<CityModel>> GetCityByStateId(long stateId)
        {
            var param = new DynamicParameters();
            param.Add("@StateId", stateId);
            var data =  await QueryAsync<CityModel>(StoredProcedures.GetCityByStateId, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        public async  Task<CityModel> GetCityById(long id)
        {
            var param = new DynamicParameters();
            param.Add("@CityId", id);
           return  await QueryFirstOrDefaultAsync<CityModel>(StoredProcedures.GetCityById, param, commandType: CommandType.StoredProcedure);
     
        }
    }
}
