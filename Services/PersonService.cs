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
            return _people.Select(p => ConvertPersonToPersonResponse(p)).ToList();
        }
    }
}
