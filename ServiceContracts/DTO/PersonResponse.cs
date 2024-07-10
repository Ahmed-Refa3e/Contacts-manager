using Entities;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public int? Age { get; set; }
        public bool ReceiveNewsLetters { get; set; }
    }

    public static class PersonExtensions
    {
        /// <summary>
        /// extension method to convert person object to person response object
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                DateOfBirth = person.DateOfBirth,
                Email = person.Email,
                Gender = person.Gender,
                Address = person.Address,
                CountryID = person.CountryID,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = person.DateOfBirth.HasValue ? DateTime.Now.Year - person.DateOfBirth.Value.Year : null
            };
        }
    }
}
