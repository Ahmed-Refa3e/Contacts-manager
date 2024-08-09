using Entities;
using Microsoft.EntityFrameworkCore;
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
            _countriesService = new CountriesService(new ContactsDbContext(new DbContextOptionsBuilder<ContactsDbContext>().Options));

            _personService = new PersonService(new ContactsDbContext(new DbContextOptionsBuilder<ContactsDbContext>().Options));
        }
        #region AddPerson
        //if we supply null request, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Act
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personService!.AddPerson(personAddRequest);
            });
        }
        //when we supply person name as null, it should throw ArgumentException
        [Fact]
        public async Task AddPerson_NullPersonName()
        {
            //Arrange
            PersonAddRequest request = new()
            {
                PersonName = null
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _personService!.AddPerson(request);
            });
        }
        //when we supply proper request, it should add the person to the list of persons
        [Fact]
        public async Task AddPerson_ProperRequest()
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = await _countriesService!.AddCountry(countryRequest);
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
            PersonResponse? response = await _personService!.AddPerson(request);
            List<PersonResponse>? list = await _personService!.GetAllPersons();

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
        public async Task GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Act
            PersonResponse? response = await _personService!.GetPersonByPersonID(personID);

            //Assert
            Assert.Null(response);
        }
        //if we supply a person id that does not exist, it should return null
        [Fact]
        public async Task GetPersonByPersonID_PersonIDDoesNotExist()
        {
            //Arrange
            Guid personID = Guid.NewGuid();

            //Act
            PersonResponse? response = await _personService!.GetPersonByPersonID(personID);

            //Assert
            Assert.Null(response);
        }
        //if we supply a person id that exists, it should return the person
        [Fact]
        public async Task GetPersonByPersonID_PersonIDExists()
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = await _countriesService!.AddCountry(countryRequest);
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
            PersonResponse? addedPerson = await _personService!.AddPerson(request);
            PersonResponse? response = await _personService!.GetPersonByPersonID(addedPerson?.PersonID);
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
        public async Task GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse>? persons = await _personService!.GetAllPersons();

            //Assert
            Assert.NotNull(persons);
            Assert.Empty(persons);
        }
        //if there are persons, it should return a list of persons
        [Fact]
        public async Task GetAllPersons_NonEmptyList()
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = await _countriesService!.AddCountry(countryRequest);
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
            PersonResponse? response1 = await _personService!.AddPerson(request1);
            PersonResponse? response2 = await _personService!.AddPerson(request2);
            List<PersonResponse>? people = await _personService!.GetAllPersons();
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
        public async Task GetFilteredPersons_NullSearchByAndNullSearchValue()
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = await _countriesService!.AddCountry(countryRequest);
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
            List<PersonResponse>? people1 = await _personService!.GetAllPersons();
            List<PersonResponse>? people2 = await _personService.GetFilteredPersons(null!, null!);
            //Assert
            Assert.NotNull(people2);
            Assert.True(people2.Count == 2);
            Assert.Equal(people1?[0].PersonName, people2[0].PersonName);
            Assert.Equal(people1?[1].PersonName, people2[1].PersonName);
        }
        //if we supply search by as person name and search value as "ah", it should return a list of persons that contain "ah" in their name regardless of the case
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = await _countriesService!.AddCountry(countryRequest);
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
            List<PersonResponse>? people1 = await _personService!.GetFilteredPersons(nameof(Person.PersonName), "ah");
            List<PersonResponse>? people2 = await _personService!.GetAllPersons();

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
        public async Task GetSortedPersons_properInput() 
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = await _countriesService!.AddCountry(countryRequest);
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
            List<PersonResponse>? people = await _personService!.GetAllPersons();
            List<PersonResponse>? SortedPeople = _personService?.GetSortedPersons(people,nameof(Person.PersonName),SortOrderOptions.ASC);

            //Assert
            Assert.NotNull(SortedPeople);
            Assert.NotNull(people);
            Assert.Equal(people[0].PersonName, SortedPeople[0].PersonName);


        }
        #endregion

        #region UpdatePerson
        //if we supply null request, it should throw ArgumentNullException
        [Fact]
        public void UpdatePerson_NullRequest()
        {
            //Arrange
            PersonUpdateRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService?.UpdatePerson(request);
            });
        }
        //if we supply a person id that does not exist, it should return argument exception
        [Fact]
        public void UpdatePerson_PersonIDDoesNotExist()
        {
            //Arrange
            PersonUpdateRequest request = new()
            {
                PersonID = Guid.NewGuid()
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService?.UpdatePerson(request);
            });
        }
        //if we supply proper request, it should update the person
        [Fact]
        public async Task UpdatePerson_ProperRequest()
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = await _countriesService!.AddCountry(countryRequest);
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

            //Act
            PersonResponse? addedPerson1 = await _personService!.AddPerson(request1);
            PersonUpdateRequest UpdateRequest = new()
            {
                PersonID = addedPerson1.PersonID,
                PersonName = "Ali",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "Ali@example.com",
                Gender = Gender.male,
                Address = "Cairo",
                CountryID = country_response?.CountryID,
                ReceiveNewsLetters = true
            };
            _personService?.UpdatePerson(UpdateRequest);
            PersonResponse? person = await _personService!.GetPersonByPersonID(addedPerson1.PersonID);

            //Assert
            Assert.NotNull(person);
            Assert.Equal(UpdateRequest.PersonName, person.PersonName);
            Assert.Equal(UpdateRequest.Email, person.Email);
        }
        #endregion

        #region DeletePerson
        //if we supply null person id, it should throw ArgumentNullException
        [Fact]
        public void DeletePerson_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService?.DeletePerson(personID);
            });
        }
        //if we supply a person id that does not exist, it should return false
        [Fact]
        public async Task DeletePerson_PersonIDDoesNotExist()
        {
            //Arrange
            Guid personID = Guid.NewGuid();

            //Act
            var isDeleted = await _personService!.DeletePerson(personID);

            //Assert
            Assert.False(isDeleted);
        }
        //if we supply a person id that exists, it should return true
        [Fact]
        public async Task DeletePerson_PersonIDExists()
        {
            //Arrange
            CountryAddRequest countryRequest = new()
            {
                CountryName = "Egypt"
            };
            CountryResponse? country_response = await _countriesService!.AddCountry(countryRequest);
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

            //Act
            PersonResponse? addedPerson = await _personService!.AddPerson(request1);
            bool isDeleted = await _personService!.DeletePerson(addedPerson?.PersonID);
            PersonResponse? person = await _personService!.GetPersonByPersonID(addedPerson?.PersonID);
            List<PersonResponse>? people = await _personService!.GetAllPersons();

            //Assert
            Assert.True(isDeleted);
            Assert.Null(person);
            Assert.True(people?.Count == 0);
        }
        #endregion
    }
}
