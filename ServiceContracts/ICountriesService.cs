using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface ICountriesService
    {
        /// <summary>
        /// adds a country to the list of countries
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CountryResponse AddCountry(CountryAddRequest? request);

        /// <summary>
        /// gets all countries
        /// </summary>
        /// <returns></returns>
        List<CountryResponse> GetAllCountries();

        /// <summary>
        /// gets a country by its country id
        /// </summary>
        /// <param name="CountryID"></param>
        /// <returns>matching country a a country object</returns>
        CountryResponse? GetCountryByCountryID(Guid? CountryID);
    }
}
