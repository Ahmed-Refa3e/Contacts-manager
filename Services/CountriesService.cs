using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService(ContactsDbContext contactsDbContext) : ICountriesService
    {
        private readonly ContactsDbContext _DBcontext = contactsDbContext;

        public async Task<CountryResponse> AddCountry(CountryAddRequest? request)
        {
            
            if (request?.CountryName == null)
            {
                throw new System.ArgumentException("CountryName cannot be null");
            }

            if (await _DBcontext.Countries.AnyAsync(c => c.CountryName == request.CountryName))
            {
                throw new System.ArgumentException("CountryName already exists");
            }

            if (request == null)
            {
                throw new System.ArgumentException("Request cannot be null");
            }

            Country country = request.ToCountry();

            //country.CountryID = Guid.NewGuid();

            country.CountryName = request.CountryName;

            _DBcontext.Countries.Add(country);
            await _DBcontext.SaveChangesAsync();

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await _DBcontext.Countries.Select(c => c.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? CountryID)
        {
            if (CountryID == null)
            {
                return null;
            }

            Country? country = await _DBcontext.Countries.FirstOrDefaultAsync(c => c.CountryID == CountryID);

            if (country == null)
            {
                return null;
            }

            return country.ToCountryResponse();
        }
    }
}
