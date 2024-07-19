using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService(ContactsDbContext contactsDbContext) : ICountriesService
    {
        private readonly ContactsDbContext _DBcontext = contactsDbContext;

        public CountryResponse AddCountry(CountryAddRequest? request)
        {
            
            if (request?.CountryName == null)
            {
                throw new System.ArgumentException("CountryName cannot be null");
            }

            if (_DBcontext.Countries.Any(c => c.CountryName == request.CountryName))
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

            _DBcontext.Countries.Add(country);
            _DBcontext.SaveChanges();

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return [.. _DBcontext.Countries.Select(country => country.ToCountryResponse())];
        }

        public CountryResponse? GetCountryByCountryID(Guid? CountryID)
        {
            if (CountryID == null)
            {
                return null;
            }

            Country? country = _DBcontext.Countries.FirstOrDefault(c => c.CountryID == CountryID);

            if (country == null)
            {
                return null;
            }

            return country.ToCountryResponse();
        }
    }
}
