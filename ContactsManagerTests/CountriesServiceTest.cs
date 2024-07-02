using ServiceContracts;
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

    }
}