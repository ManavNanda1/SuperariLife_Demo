using SuperariLife.Model.Country;


namespace SuperariLife.Data.DBRepository.Country
{
    public interface ICountryRepository
    {
        Task<int> AddUpdateCountry(CountryRequestModel country);
        Task<int> DeleteCountry(int Id);
        Task<List<CountryModel>> GetCountryList();
        Task<CountryModel> GetCountryById(int Id);
       
    }
}
