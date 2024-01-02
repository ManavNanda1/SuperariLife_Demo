using SuperariLife.Model.City;

namespace SuperariLife.Data.DBRepository.City
{
    public interface ICityService
    {
        public Task<int> AddUpdateCity(CityRequestModel city);
        public Task<int> DeleteCity(int Id);
        public Task<List<CityModel>> GetCityList(long StateId);
        public Task<CityModel> GetCityById(long CityId);
    }
}
