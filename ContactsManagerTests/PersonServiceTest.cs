using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
namespace ContactsManagerTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService? _personService;
        private readonly ICountriesService? _countriesService;

        //constructor
        public PersonServiceTest()
        {
            _personService = new PersonService();
            _countriesService = new CountriesService();
        }
        #region AddPerson
        //if we supply null request, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullRequest()
        {
            //Arrange
            PersonAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService?.AddPerson(request);
            });
        }
        //when we supply person name as null, it should throw ArgumentException
        [Fact]
        public void AddPerson_NullPersonName()
        {
            //Arrange
            PersonAddRequest request = new()
            {
                PersonName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService?.AddPerson(request);
            });
        }
        //when we supply proper request, it should add the person to the list of persons
        [Fact]
        public void AddPerson_ProperRequest()
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = _countriesService?.AddCountry(countryRequest);
            PersonAddRequest request = new()
            {
                PersonName = "Ahmed",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "Ahmed@example.com",
                Gender = Gender.male,
                Address = "Cairo",
                CountryID = country_response?.CountryID,
                ReceiveNewsLetters = true
            };
            //Act
            PersonResponse? response = _personService?.AddPerson(request);
            List<PersonResponse>? list = _personService?.GetAllPersons();

            //Assert
            Assert.NotNull(response);
            Assert.NotEqual(Guid.Empty, response?.PersonID);
            Assert.NotNull(list);
            Assert.Equal(response?.PersonID, list.FirstOrDefault()?.PersonID);
            Assert.Equal(response?.CountryID, country_response?.CountryID);
            //Assert.Equal(country_response?.CountryName, response?.Country);
        }
        #endregion

        #region GetPersonByPersonID
        //if we supply null person id, it should return null
        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Act
            PersonResponse? response = _personService?.GetPersonByPersonID(personID);

            //Assert
            Assert.Null(response);
        }
        //if we supply a person id that does not exist, it should return null
        [Fact]
        public void GetPersonByPersonID_PersonIDDoesNotExist()
        {
            //Arrange
            Guid personID = Guid.NewGuid();

            //Act
            PersonResponse? response = _personService?.GetPersonByPersonID(personID);

            //Assert
            Assert.Null(response);
        }
        //if we supply a person id that exists, it should return the person
        [Fact]
        public void GetPersonByPersonID_PersonIDExists()
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = _countriesService?.AddCountry(countryRequest);
            PersonAddRequest request = new()
            {
                PersonName = "Ahmed",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "Ahmed@example.com",
                Gender = Gender.male,
                Address = "Cairo",
                CountryID = country_response?.CountryID,
                ReceiveNewsLetters = true
            };
            //Act
            PersonResponse? addedPerson = _personService?.AddPerson(request);
            PersonResponse? response = _personService?.GetPersonByPersonID(addedPerson?.PersonID);
            //Assert
            Assert.NotNull(response);
            Assert.Equal(addedPerson?.PersonID, response.PersonID);
            Assert.Equal(addedPerson?.PersonName, response.PersonName);
            Assert.Equal(country_response?.CountryID, response.CountryID);
        }
        #endregion

        #region GetAllPersons
        //if there are no persons, it should return an empty list
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse>? persons = _personService?.GetAllPersons();

            //Assert
            Assert.NotNull(persons);
            Assert.Empty(persons);
        }
        //if there are persons, it should return a list of persons
        [Fact]
        public void GetAllPersons_NonEmptyList()
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = _countriesService?.AddCountry(countryRequest);
            PersonAddRequest request1 = new()
            {
                PersonName = "Ahmed",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "Ahmed@example.com",
                Gender = Gender.male,
                Address = "Cairo",
                CountryID = country_response?.CountryID,
                ReceiveNewsLetters = true
            };
            PersonAddRequest request2 = new()
            {
                PersonName = "Mohamed",
                DateOfBirth = new DateTime(1995, 1, 1),
                Email = "Mohamed@example.com",
                Gender = Gender.male,
                Address = "Tanta",
                CountryID = country_response?.CountryID,
                ReceiveNewsLetters = true
            };
            //Act
            PersonResponse? response1 = _personService?.AddPerson(request1);
            PersonResponse? response2 = _personService?.AddPerson(request2);
            List<PersonResponse>? people = _personService?.GetAllPersons();
            //Assert
            Assert.NotNull(people);
            Assert.True(people.Count == 2);
            Assert.Contains(people, p => p.PersonID == response1?.PersonID);
            Assert.Contains(people, p => p.PersonID == response2?.PersonID);
            Assert.Contains(people, p => p.Country == response2?.Country);
            Assert.Contains(people, p => p.Country == response1?.Country);
        }
        #endregion

        #region GetFilteredPersons
        //if we supply null search by and null search value, it should return the same list as get all persons
        [Fact]
        public void GetFilteredPersons_NullSearchByAndNullSearchValue()
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = _countriesService?.AddCountry(countryRequest);
            PersonAddRequest request1 = new()
            {
                PersonName = "Ahmed",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "Ahmed@example.com",
                Gender = Gender.male,
                Address = "Cairo",
                CountryID = country_response?.CountryID,
                ReceiveNewsLetters = true
            };
            PersonAddRequest request2 = new()
            {
                PersonName = "Mohamed",
                DateOfBirth = new DateTime(1995, 1, 1),
                Email = "Mohamed@example.com",
                Gender = Gender.male,
                Address = "Tanta",
                CountryID = country_response?.CountryID,
                ReceiveNewsLetters = true
            };
            //Act
            _personService?.AddPerson(request1);
            _personService?.AddPerson(request2);
            List<PersonResponse>? people1 = _personService?.GetAllPersons();
            List<PersonResponse>? people2 = _personService?.GetFilteredPersons(null, null);
            //Assert
            Assert.NotNull(people2);
            Assert.True(people2.Count == 2);
            Assert.Equal(people1?[0].PersonName, people2[0].PersonName);
            Assert.Equal(people1?[1].PersonName, people2[1].PersonName);
        }
        //if we supply search by as person name and search value as "ah", it should return a list of persons that contain "ah" in their name regardless of the case
        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = _countriesService?.AddCountry(countryRequest);
            PersonAddRequest request1 = new()
            {
                PersonName = "Ahmed",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "Ahmed@example.com",
                Gender = Gender.male,
                Address = "Cairo",
                CountryID = country_response?.CountryID,
                ReceiveNewsLetters = true
            };
            PersonAddRequest request2 = new()
            {
                PersonName = "Mohamed",
                DateOfBirth = new DateTime(1995, 1, 1),
                Email = "Mohamed@example.com",
                Gender = Gender.male,
                Address = "Tanta",
                CountryID = country_response?.CountryID,
                ReceiveNewsLetters = true
            };

            //Act
            _personService?.AddPerson(request1);
            _personService?.AddPerson(request2);
            List<PersonResponse>? people1 = _personService?.GetFilteredPersons(nameof(Person.PersonName), "ah");
            List<PersonResponse>? people2 = _personService?.GetAllPersons();

            //Assert
            Assert.NotNull(people1);
            Assert.True(people1?.Count == 1);
            Assert.True(people2?.Count == 2);
            Assert.Contains(people1, p => p.PersonName == "Ahmed");
        }
        #endregion

        #region sortedPersons
        //if we supply proper input, it should return a list of persons sorted by the specified property in the specified order
        [Fact]
        public void GetSortedPersons_properInput() 
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = _countriesService?.AddCountry(countryRequest);
            PersonAddRequest request1 = new()
            {
                PersonName = "Ahmed",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "Ahmed@example.com",
                Gender = Gender.male,
                Address = "Cairo",
                CountryID = country_response?.CountryID,
                ReceiveNewsLetters = true
            };
            PersonAddRequest request2 = new()
            {
                PersonName = "Mohamed",
                DateOfBirth = new DateTime(1995, 1, 1),
                Email = "Mohamed@example.com",
                Gender = Gender.male,
                Address = "Tanta",
                CountryID = country_response?.CountryID,
                ReceiveNewsLetters = true
            };

            //Act
            _personService?.AddPerson(request1);
            _personService?.AddPerson(request2);
            List<PersonResponse>? people = _personService?.GetAllPersons();
            List<PersonResponse>? SortedPeople = _personService?.GetSortedPersons(people,nameof(Person.PersonName),SortOrderOptions.Ascending);

            //Assert
            Assert.NotNull(SortedPeople);
            Assert.NotNull(people);
            Assert.Equal(people[0].PersonName, SortedPeople[0].PersonName);


        }
        #endregion
    }
}
