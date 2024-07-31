using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage ="Enter a valid email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage="Date of birth can't be blank")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Country can not be blank")]
        public GenderOptions? Gender { get; set; }

        [Required(ErrorMessage ="Gender can not be blank")]
        public Guid? CountryId { get; set; }

        [Required(ErrorMessage ="Address can't be blank")]
        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }


        public Person ToPerson()
        {
            return new Person() 
            { 
                PersonName = PersonName, Email = Email, DateOfBirth = DateOfBirth, Gender = Gender.ToString(), CountryId = CountryId, Address = Address, ReceiveNewsLetters = ReceiveNewsLetters 
            };
        }
    }
}
