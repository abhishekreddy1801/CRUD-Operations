using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        private List<Person> _persons;
        private ICountryService _countryService;

        public PersonService(bool initialize = true) 
        {
            _persons = new List<Person>();
            _countryService = new CountryService();

            if(initialize) 
            { 
                _persons.AddRange(new List<Person>()
                {
                    new Person() {PersonId = Guid.Parse("88794828-12FB-42D9-BDC9-16A1C59CC710"), PersonName = "Abhi", Email ="abhi@bing.com", Address="kalaburgi", CountryId =Guid.Parse("48A575FC-936C-4CF9-9B0A-28A0F8AA2BF8"), DateOfBirth = DateTime.Parse("2000-02-03"), Gender="Male", ReceiveNewsLetters=true },
                    new Person() {PersonId = Guid.Parse("CE766300-CB7E-4914-B16B-9660601FD514"), PersonName = "Narayan", Email ="Narayan@bing.com", Address="Banglore", CountryId =Guid.Parse("48A575FC-936C-4CF9-9B0A-28A0F8AA2BF8"), DateOfBirth = DateTime.Parse("1990-02-05"), Gender="Male", ReceiveNewsLetters=false },
                    new Person() {PersonId = Guid.Parse("B5F7FAC6-E10D-45F9-9553-C2DDAC85B472"), PersonName = "Naruto", Email ="Naruto@bing.com", Address="Mysore", CountryId =Guid.Parse("B9334C94-EE78-4BC8-A041-321C65F4219C"), DateOfBirth = DateTime.Parse("2000-05-08"), Gender="Male", ReceiveNewsLetters=true },
                    new Person() {PersonId = Guid.Parse("EE703313-13CF-47CE-BA68-665BD0FF256A"), PersonName = "Hinita", Email ="Hinita@bing.com", Address="Belgav", CountryId =Guid.Parse("2EFDD32E-1818-4D4D-A1F6-69FC19187652"), DateOfBirth = DateTime.Parse("2001-03-02"), Gender="Female", ReceiveNewsLetters=false },
                    new Person() {PersonId = Guid.Parse("9603E4E2-B3FE-4623-9723-37FF2CBBE45E"), PersonName = "Kakashi", Email ="Kakashi@bing.com", Address="Hubli", CountryId =Guid.Parse("966FF0AC-3658-47C0-80AD-EAC122BFEF0E"), DateOfBirth = DateTime.Parse("1995-02-03"), Gender="Male", ReceiveNewsLetters=false },
                    new Person() {PersonId = Guid.Parse("B22CEB36-3FF0-4A59-BD2D-F5B09D6C92A6"), PersonName = "Sakura", Email ="Sakura@bing.com", Address="Dharwad", CountryId =Guid.Parse("90A7EA19-C4BC-4487-A871-88FE9D1DC7B6"), DateOfBirth = DateTime.Parse("2000-08-08"), Gender="Female", ReceiveNewsLetters=true },
                });
            }
        }
            
        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();

            personResponse.Country = _countryService.GetCountryByCountryId(person.CountryId)?.CountryName;

            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if(personAddRequest is null) throw new ArgumentNullException(nameof(personAddRequest));

            //if (string.IsNullOrEmpty(personAddRequest.PersonName)) throw new ArgumentException("Person name can't be blank");
            //                              or
            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();

            _persons.Add(person);

            PersonResponse personResponse = ConvertPersonToPersonResponse(person);

            return personResponse;
        }

        public List<PersonResponse> GetAllPersons()
        {
            //return _persons.Select(person => person.ToPersonResponse()).ToList();
            //                     or

            List<PersonResponse> personResponses = _persons.Select(person => ConvertPersonToPersonResponse(person)).ToList();
            return personResponses;
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if(personId is null)  return null;

            Person? person = _persons.FirstOrDefault(person => person.PersonId == personId);

            if(person is null) return null;

            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if(string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchBy)) return matchingPersons;

            switch(searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.PersonName) ? person.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.Email) ? person.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(person => (person.DateOfBirth is not null) ? person.DateOfBirth.Value.ToString("dd MM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.Gender) ? person.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.CountryId):
                    matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.Country) ? person.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.Address) ? person.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: 
                    matchingPersons = allPersons;
                    break;
            }

            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy)) return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.ReceiveNewsLetters).ToList(),

                _ => allPersons
            } ;

            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest is null) throw new ArgumentNullException(nameof(personUpdateRequest));

            ValidationHelper.ModelValidation(personUpdateRequest);

            Person? matchingPerson = _persons.FirstOrDefault(person => person.PersonId == personUpdateRequest.PersonId); 

            if(matchingPerson is null) throw new ArgumentException(nameof(matchingPerson));

            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();

            return ConvertPersonToPersonResponse(matchingPerson);

        }

        public bool DeletePerson(Guid? personId)
        {
            if (personId is null) throw new ArgumentNullException();

            Person? person = _persons.FirstOrDefault(person => person.PersonId == personId);

            if (person is null) return false;

            _persons.RemoveAll(person => person.PersonId == personId);

            return true;

        }
    }
}
