using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.Config;
using SuperariLife.Model.Country;
using System.Data;


namespace SuperariLife.Data.DBRepository.Country
{

    public class CountryRepository:BaseRepository, ICountryRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public CountryRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }

        #endregion

        public async Task<int> AddUpdateCountry(CountryRequestModel country)
        {
            var param = new DynamicParameters();
            param.Add("@CountryId", country.CountryId);
            param.Add("@Countryname", country.Countryname);
            param.Add("@UserId", country.UpdatedBy);
            return await QueryFirstOrDefaultAsync<int>(StoredProcedures.InsertUpdateCountry, param, commandType: CommandType.StoredProcedure);
        }
        public async Task<int> DeleteCountry(int Id)
        {
            var param = new DynamicParameters();
            param.Add("@CountryId ", Id);
            return await QueryFirstOrDefaultAsync<int>(StoredProcedures.DeleteCountry, param, commandType: CommandType.StoredProcedure);

        }
        public async Task<List<CountryModel>> GetCountryList()
        {
                var data =  await QueryAsync<CountryModel>(StoredProcedures.GetCountry, commandType: CommandType.StoredProcedure);
                return data.ToList();
        }
        public async  Task<CountryModel> GetCountryById(int Id)
        {
            var param = new DynamicParameters();
            param.Add("@CountryId", Id);
            return await QueryFirstOrDefaultAsync<CountryModel>(StoredProcedures.GetCountryById ,param, commandType: CommandType.StoredProcedure);
        }
      
    }
}
