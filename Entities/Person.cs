using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }
        [StringLength(40)]
        public string? PersonName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(40)]
        public string? Email { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        [StringLength(60)]
        public string? Address { get; set; }
        //unique identifier
        [ForeignKey("CountryID")]
        public Guid? CountryID { get; set; }
        public bool ReceiveNewsLetters { get; set; }
    }
}
