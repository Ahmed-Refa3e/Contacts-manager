using Microsoft.AspNetCore.Http;
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
        Task<CountryResponse> AddCountry(CountryAddRequest? request);

        /// <summary>
        /// gets all countries
        /// </summary>
        /// <returns></returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// gets a country by its country id
        /// </summary>
        /// <param name="CountryID"></param>
        /// <returns>matching country a a country object</returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? CountryID);

        /// <summary>
        /// uploads countries from an excel file
        /// </summary>
        /// <param name="formFile">excel file</param>
        /// <returns>Number of countries that have been added</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
