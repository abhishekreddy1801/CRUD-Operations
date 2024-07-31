using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public Guid? CountryId { get; set; }

        public string? Country { get; set; }

        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }

        public double? Age { get; set; }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                ReceiveNewsLetters = ReceiveNewsLetters,
                Address = Address,
                CountryId = CountryId,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true)
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            if(obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;
            
            return PersonId == person.PersonId && PersonName == person.PersonName && Email == person.Email && DateOfBirth == person.DateOfBirth && Gender == person.Gender && CountryId == person.CountryId && Address == person.Address && ReceiveNewsLetters == person.ReceiveNewsLetters;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class PersonExtension
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            PersonResponse personResponse = new PersonResponse() 
            { 
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
            };

            if(personResponse.DateOfBirth is not null)
            {
                personResponse.Age = Math.Round((DateTime.Now - personResponse.DateOfBirth.Value).TotalDays / 365.25);
            }
            else
            {
                personResponse.Age = null;
            }

            return personResponse;

        }
    }
}
