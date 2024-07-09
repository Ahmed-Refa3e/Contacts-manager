using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.Buffers.Text;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;

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
                return AllPeople;
            }

            //typeof(PersonResponse): Gets the type object for the PersonResponse class.
            //GetProperty: Retrieves the PropertyInfo object for the property specified by sortBy.
            //BindingFlags.IgnoreCase: Ensures that the property name comparison is case-insensitive.
            //BindingFlags.Public: Ensures that only public properties are considered.
            //BindingFlags.Instance: Ensures that only instance properties are considered(not static properties).

            PropertyInfo? propertyInfo = typeof(PersonResponse).GetProperty(SearchBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
            {
                return AllPeople;
            }
            foreach (PersonResponse person in AllPeople)
            {
                //Retrieves the value of the specified property(SearchBy) from the current person object and converts it to a string.
                string? value = propertyInfo.GetValue(person, null)?.ToString();

                //value.ToLower(CultureInfo.InvariantCulture).Contains(SearchValue.ToLower(CultureInfo.InvariantCulture)):
                //Converts both value and SearchValue to lowercase(using invariant culture for consistency) and checks if value contains SearchValue.
                if (value != null && value.ToLower(CultureInfo.InvariantCulture).Contains(SearchValue.ToLower(CultureInfo.InvariantCulture)))
                {
                    //If the condition is true, the person object is added to the filteredPeople list.
                    filteredPeople.Add(person);
                }
            }
            return filteredPeople;
        }
        public List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortBy, SortOrderOptions sortOrder)
        {
            //using reflection to sort the list of persons based on the property name
            if (string.IsNullOrEmpty(sortBy))
                return persons;

            //typeof(PersonResponse): Gets the type object for the PersonResponse class.
            //GetProperty: Retrieves the PropertyInfo object for the property specified by sortBy.
            //BindingFlags.IgnoreCase: Ensures that the property name comparison is case-insensitive.
            //BindingFlags.Public: Ensures that only public properties are considered.
            //BindingFlags.Instance: Ensures that only instance properties are considered(not static properties).

            PropertyInfo? propertyInfo = typeof(PersonResponse).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
                return persons;

            //Checks if the sort order is ascending Orders the list in ascending order based on the value of the property retrieved
            //if the sort order is descending Orders the list in descending order based on the value of the property retrieved

            return sortOrder == SortOrderOptions.Ascending
                ? persons.OrderBy(p => propertyInfo.GetValue(p, null)).ToList()
                : persons.OrderByDescending(p => propertyInfo.GetValue(p, null)).ToList();
        }
    }
}
