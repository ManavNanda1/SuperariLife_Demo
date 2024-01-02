using SuperariLife.Model.City;


namespace SuperariLife.Data.DBRepository.City
{
    public interface ICityRepository
    {
        Task<int> AddUpdateCity(CityRequestModel city);
        Task<int> DeleteCity(int Id);
        Task<List<CityModel>> GetCityByStateId (long stateId);
        Task<CityModel>GetCityById (long id);

    }
}
