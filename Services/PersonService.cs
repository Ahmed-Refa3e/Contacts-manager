using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.Globalization;
using System.Reflection;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly ApplicationDbContext _DBcontext;


        //constructor
        public PersonService(ApplicationDbContext personDbContext)
        {
            _DBcontext = personDbContext;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? request)
        {
            ArgumentNullException.ThrowIfNull(request);
            //Model validations
            ValidationHelper.ModelValidation(request);
            Person person = request.ToPerson();
            person.PersonID = Guid.NewGuid();
            _DBcontext.Persons.Add(person);
            await _DBcontext.SaveChangesAsync();
            return person.ToPersonResponse();
        }
        public async Task<PersonResponse?> GetPersonByPersonID(Guid? PersonID)
        {
            if (PersonID == null)
            {
                return null;
            }
            Person? person = await _DBcontext.Persons.Include("Country").FirstOrDefaultAsync(p => p.PersonID == PersonID);
            if (person == null)
            {
                return null;
            }
            return person.ToPersonResponse();
        }
        public async Task<List<PersonResponse>> GetAllPersons()
        {
            var persons = await _DBcontext.Persons.Include("Country").ToListAsync();
            return persons.Select(p => p.ToPersonResponse()).ToList();
        }
        public async Task<List<PersonResponse>> GetFilteredPersons(string SearchBy, string SearchValue)
        {
            List<PersonResponse> filteredPeople = [];
            List<PersonResponse> AllPeople = await GetAllPersons();
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
        public  List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortBy, SortOrderOptions sortOrder)
        {
            //using reflection to sort the list of persons based on the property name specified by sortBy
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

            return sortOrder == SortOrderOptions.ASC
                ? persons.OrderBy(p => propertyInfo.GetValue(p, null)).ToList()
                : persons.OrderByDescending(p => propertyInfo.GetValue(p, null)).ToList();
        }
        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? request)
        {
            ArgumentNullException.ThrowIfNull(request);
            //Model validations
            ValidationHelper.ModelValidation(request);
            Person? person = await _DBcontext.Persons.FirstOrDefaultAsync(p => p.PersonID == request.PersonID) ?? throw new ArgumentException("PersonID doesn't exist");
            //updating the person object
            person.PersonName = request.PersonName;
            person.DateOfBirth = request.DateOfBirth;
            person.Email = request.Email;
            person.ReceiveNewsLetters = request.ReceiveNewsLetters;
            person.Address = request.Address;
            person.CountryID = request.CountryID;
            person.Gender = request.Gender.ToString();

            await _DBcontext.SaveChangesAsync();

            return person.ToPersonResponse();
        }
        public async Task<bool> DeletePerson(Guid? PersonID)
        {
            if (PersonID == null)
            {
                throw new ArgumentNullException(nameof(PersonID));
            }
            Person? person = await _DBcontext.Persons.FirstOrDefaultAsync(p => p.PersonID == PersonID);
            if (person == null)
            {
                return false;
            }
            _DBcontext.Persons.Remove(person);
            await _DBcontext.SaveChangesAsync();
            return true;
        }
    }
}
