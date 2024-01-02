using SuperariLife.Model.Country;


namespace SuperariLife.Data.DBRepository.Country
{
    public class CountryService:ICountryService
    {
        #region Fields
        private readonly ICountryRepository _repository;
        #endregion

        #region Constructor
        public CountryService(ICountryRepository countryRepository)
        {
            _repository = countryRepository;
        }


        public async Task<CountryModel> GetCountryById(int id)
        {
            return await _repository.GetCountryById(id); 
        }

        #endregion

        #region Methods

        public async Task<int> InsertUpdateCountry(CountryRequestModel country)
        {
            return await _repository.AddUpdateCountry(country);
        }
        public async Task<int> DeleteCountry(int Id)
        {
            return await _repository.DeleteCountry(Id);
        }
        public async Task<List<CountryModel>> GetCountryList()
        {
            return await _repository.GetCountryList();
        }

        #endregion
    }
}
