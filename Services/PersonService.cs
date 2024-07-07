using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _people;
        private readonly ICountriesService _countriesService;

        public PersonService()
        {
            _people = new();
            _countriesService = new CountriesService();
        }
        //private method to convert person object to person response object
        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            //Model validations
            ValidationHelper.ModelValidation(request);
            Person person = request.ToPerson();
            person.PersonID = Guid.NewGuid();
            _people.Add(person);
            return ConvertPersonToPersonResponse(person);
        }
        public PersonResponse? GetPersonByPersonID(Guid? PersonID)
        {
            if (PersonID == null)
            {
                return null;
            }
            Person? person = _people.FirstOrDefault(p => p.PersonID == PersonID);
            if (person == null)
            {
                return null;
            }
            return ConvertPersonToPersonResponse(person);
        }
        public List<PersonResponse> GetAllPersons()
        {
            return _people.Select(person => ConvertPersonToPersonResponse(person)).ToList();
        }
        public List<PersonResponse> GetFilteredPersons(string SearchBy, string SearchValue)
        {
            List<PersonResponse> filteredPeople = new();
            List<PersonResponse> AllPeople = GetAllPersons();

            if (string.IsNullOrEmpty(SearchBy) || string.IsNullOrEmpty(SearchValue))
            {
                return filteredPeople = AllPeople;
            }
            switch (SearchBy)
            {
                case nameof(Person.PersonName):
                    filteredPeople = AllPeople.Where(p => (p.PersonName != null) && p.PersonName.Contains(SearchValue, StringComparison.OrdinalIgnoreCase)).ToList(); break;
                case nameof(Person.Address):
                    filteredPeople = AllPeople.Where(p => (p.Address != null) && p.Address.Contains(SearchValue, StringComparison.OrdinalIgnoreCase)).ToList(); break;
                case nameof(Person.DateOfBirth):
                    filteredPeople = AllPeople.Where(p => (p.DateOfBirth != null) && p.DateOfBirth.Value.ToString().Contains(SearchValue, StringComparison.OrdinalIgnoreCase)).ToList();break;
                case nameof(Person.Email):
                    filteredPeople = AllPeople.Where(p => (p.Email != null) && p.Email.Contains(SearchValue, StringComparison.OrdinalIgnoreCase)).ToList(); break;
                case nameof(Person.Gender):
                    filteredPeople = AllPeople.Where(p => (p.Gender != null) && p.Gender.Contains(SearchValue, StringComparison.OrdinalIgnoreCase)).ToList(); break;
                case nameof(Person.CountryID):
                    filteredPeople = AllPeople.Where(p => (p.CountryID != null) && p.CountryID.Value.ToString().Contains(SearchValue, StringComparison.OrdinalIgnoreCase)).ToList(); break;
                default: filteredPeople = AllPeople; break;
            }
            return filteredPeople;
        } 
    }
}
