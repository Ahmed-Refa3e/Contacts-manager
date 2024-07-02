using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries = [];
        public CountryResponse AddCountry(CountryAddRequest? request)
        {
            
            if (request?.CountryName == null)
            {
                throw new System.ArgumentException("CountryName cannot be null");
            }

            if (_countries.Any(c => c.CountryName == request.CountryName))
            {
                throw new System.ArgumentException("CountryName already exists");
            }

            if (request == null)
            {
                throw new System.ArgumentException("Request cannot be null");
            }

            Country country = request.ToCountry();

            country.CountryID = Guid.NewGuid();

            country.CountryName = request.CountryName;

            _countries.Add(country);

            return country.ToCountryResponse();
        }
    }
}
