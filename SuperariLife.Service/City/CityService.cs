using SuperariLife.Model.City;


namespace SuperariLife.Data.DBRepository.City
{
    public class CityService:ICityService
    {
        #region Fields
        private readonly ICityRepository _repository;
        #endregion

        #region Construtor
        public CityService(ICityRepository repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<int> AddUpdateCity(CityRequestModel city)
        {
            return await _repository.AddUpdateCity(city);
        }
        public async Task<int> DeleteCity(int Id)
        {
            return await _repository.DeleteCity(Id);
        }
        public async Task<List<CityModel>> GetCityList(long StateId)
        {
            return await _repository.GetCityByStateId(StateId);
        }
        public async Task<CityModel> GetCityById(long CityId)
        {
            return await _repository.GetCityById(CityId);
        }

    }
}
