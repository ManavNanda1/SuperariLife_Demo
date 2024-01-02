using SuperariLife.Model.Country;


namespace SuperariLife.Data.DBRepository.Country
{
    public interface ICountryService
    {
        public Task<int> DeleteCountry(int Id);
        public Task<List<CountryModel>> GetCountryList();
        public Task<CountryModel> GetCountryById(int id);
        public Task<int> InsertUpdateCountry(CountryRequestModel country);      
    }
}
