﻿using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage ="person name cannot be blank")]
        public string? PersonName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
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
