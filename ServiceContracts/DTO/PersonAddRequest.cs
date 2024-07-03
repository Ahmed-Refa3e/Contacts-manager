using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        public string? PersonName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public Gender? Gender { get; set; }
        public string? Address { get; set; }
        public Guid? CountryID { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// converts the request to a person object
        /// </summary>
        /// <returns>object of person class</returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                CountryID = CountryID,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                Email = Email,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
