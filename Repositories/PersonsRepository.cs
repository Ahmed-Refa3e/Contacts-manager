using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System.Linq.Expressions;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Person> AddPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonByPersonID(Guid personID)
        {
            Task<Person> matchingPerson = GetPersonByPersonID(personID)!;
            if (matchingPerson == null)
            {
                return false;
            }
            _context.Persons.Remove(await matchingPerson);
            int rowsDeleted = await _context.SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await _context.Persons.Include("Country").ToListAsync();
        }

        public Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return _context.Persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonID(Guid personID)
        {
            return await _context.Persons.Include("Country").FirstOrDefaultAsync(p => p.PersonID == personID);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            var matchingPerson = await GetPersonByPersonID(person.PersonID) ?? throw new Exception("Person not found");
            _context.Entry(matchingPerson).CurrentValues.SetValues(person);
            await _context.SaveChangesAsync();
            return matchingPerson;
        }
    }
}
