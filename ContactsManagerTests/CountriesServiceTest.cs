using ServiceContracts.DTO;
using Services;

namespace ContactsManagerTests
{
    public class CountriesServiceTest
    {
        private readonly CountriesService _countriesService;
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }

        #region AddCountry
        //When the CountryName is null, it should throw ArgumentException
        [Fact]
        public void AddCountry_NullCountryName()
        {
            //Arrange
            CountryAddRequest request = new()
            {
                CountryName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest request1 = new() { CountryName = "Egypt" };
            CountryAddRequest request2 = new() { CountryName = "Egypt" };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        [Fact]
        public void AddCountry_ProperCountryName()
        {
            //Arrange
            CountryAddRequest request = new() { CountryName = "Egypt" };

            //Act
            CountryResponse response = _countriesService.AddCountry(request);

            //Assert
            Assert.NotEqual(Guid.Empty, response.CountryID);
            Assert.Equal("Egypt", response.CountryName);
        }

        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullRequest()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }
        #endregion

        #region GetAllCountries

        //The list of countries should be empty by default (before adding any countries)
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            //Act
            List<CountryResponse> countries = _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(countries);
        }
        [Fact]
        public void GetAllCountries_NotEmptyList()
        {
            //Arrange
            CountryAddRequest request1 = new() { CountryName = "Egypt" };
            CountryAddRequest request2 = new() { CountryName = "China" };
            _countriesService.AddCountry(request1);
            _countriesService.AddCountry(request2);

            //Act
            List<CountryResponse> countries = _countriesService.GetAllCountries();

            //Assert
            Assert.Contains(countries, c => c.CountryName == "Egypt");
            Assert.Contains(countries, c => c.CountryName == "China");

        }
        #endregion

        #region GetCountryByCountryID
        //When the CountryID is null, it should return null exception
        [Fact]
        public void GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? countryID = null;

            //Act
            CountryResponse? response = _countriesService.GetCountryByCountryID(countryID);

            //Assert
            Assert.Null(response);
        }
        //When the CountryID is not null, it should return valid country if the country id match any country
        [Fact]
        public void GetCountryByCountryID_NotNullCountryID()
        {
            //Arrange
            CountryAddRequest request = new() { CountryName = "Egypt" };
            CountryResponse response = _countriesService.AddCountry(request);

            //Act
            CountryResponse? country = _countriesService.GetCountryByCountryID(response.CountryID);

            //Assert
            Assert.NotNull(country);
            Assert.Equal(response.CountryName, country.CountryName);
            Assert.Equal(response.CountryID, country.CountryID);
        }

        //When the CountryID is not null, it should return null if the country id doesn't match any country
        [Fact]
        public void GetCountryByCountryID_NotNullCountryID_NotFound()
        {
            //Arrange
            Guid countryID = Guid.NewGuid();

            //Act
            CountryResponse? country = _countriesService.GetCountryByCountryID(countryID);

            //Assert
            Assert.Null(country);
        }
        #endregion
    }
}