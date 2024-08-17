using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;
using Services;

namespace ContactsManagerTests
{
    public class CountriesServiceTest
    {
        private readonly CountriesService _countriesService;
        public CountriesServiceTest()
        {
            var countriesInitialData = new List<Country>() { };

            DbContextMock<ApplicationDbContext> dbContextMock = new(
              new DbContextOptionsBuilder<ApplicationDbContext>().Options
             );

            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);

            _countriesService = new CountriesService(dbContext);
        }

        #region AddCountry
        //When the CountryName is null, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_NullCountryName()
        {
            //Arrange
            CountryAddRequest request = new()
            {
                CountryName = null
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest request1 = new() { CountryName = "Egypt" };
            CountryAddRequest request2 = new() { CountryName = "Egypt" };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });
        }

        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        [Fact]
        public async Task AddCountry_ProperCountryName()
        {
            //Arrange
            CountryAddRequest request = new() { CountryName = "Egypt" };

            //Act
            CountryResponse response = await _countriesService.AddCountry(request);

            //Assert
            Assert.NotEqual(Guid.Empty, response.CountryID);
            Assert.Equal("Egypt", response.CountryName);
        }

        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullRequest()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }
        #endregion

        #region GetAllCountries

        //The list of countries should be empty by default (before adding any countries)
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            //Act
            List<CountryResponse> countries = await _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(countries);
        }
        [Fact]
        public async Task GetAllCountries_NotEmptyList()
        {
            //Arrange
            CountryAddRequest request1 = new() { CountryName = "Egypt" };
            CountryAddRequest request2 = new() { CountryName = "China" };
            await _countriesService.AddCountry(request1);
            await _countriesService.AddCountry(request2);

            //Act
            List<CountryResponse> countries = await _countriesService.GetAllCountries();

            //Assert
            Assert.Contains(countries, c => c.CountryName == "Egypt");
            Assert.Contains(countries, c => c.CountryName == "China");

        }
        #endregion

        #region GetCountryByCountryID
        //When the CountryID is null, it should return null exception
        [Fact]
        public async Task GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? countryID = null;

            //Act
            CountryResponse? response = await _countriesService.GetCountryByCountryID(countryID);

            //Assert
            Assert.Null(response);
        }
        //When the CountryID is not null, it should return valid country if the country id match any country
        [Fact]
        public async Task GetCountryByCountryID_NotNullCountryID()
        {
            //Arrange
            CountryAddRequest request = new() { CountryName = "Egypt" };
            CountryResponse response = await _countriesService.AddCountry(request);

            //Act
            CountryResponse? country = await _countriesService.GetCountryByCountryID(response.CountryID);

            //Assert
            Assert.NotNull(country);
            Assert.Equal(response.CountryName, country.CountryName);
            Assert.Equal(response.CountryID, country.CountryID);
        }

        //When the CountryID is not null, it should return null if the country id doesn't match any country
        [Fact]
        public async Task GetCountryByCountryID_NotNullCountryID_NotFound()
        {
            //Arrange
            Guid countryID = Guid.NewGuid();

            //Act
            CountryResponse? country = await _countriesService.GetCountryByCountryID(countryID);

            //Assert
            Assert.Null(country);
        }
        #endregion
    }
}