using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDTest
{
    public class PersonsServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;
        public PersonsServiceTest()
        {
            _personService = new PersonService();
            _countryService = new CountryService(false);
        }

        #region AddPerson

        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                PersonResponse? personResponse = _personService.AddPerson(personAddRequest);

            });

        }

        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                PersonResponse? personResponse = _personService.AddPerson(personAddRequest);

            });

        }

        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "Jack", Email = "Jack@Oggy.com", Address = "Cartoon World", CountryId = Guid.NewGuid(), Gender = ServiceContracts.Enums.GenderOptions.Male, DateOfBirth = DateTime.Parse("2000-01-18"), ReceiveNewsLetters = true };

            //Act
            PersonResponse? personResponse = _personService.AddPerson(personAddRequest);
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            //Assert
            Assert.True(personResponse.PersonId != Guid.Empty);
            Assert.Contains(personResponse, allPersons);

        }

        #endregion

        #region GetPersonByPersonId

        [Fact]
        public void GetPersonByPersonId_NullPersonId()
        {
            //Arrange
            Guid? personId = null;

            //Act
            PersonResponse? personResponse = _personService.GetPersonByPersonId(personId);


            //Assert
            Assert.Null(personResponse);

        }

        [Fact]
        public void GetPersonByPersonId_WithPersonId()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Bharat" };
            CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);

            //Act
            PersonAddRequest personAddRequest = new PersonAddRequest() { PersonName = "Sigma", Address = "Karnataka", DateOfBirth = DateTime.Parse("2001-06-01"), Gender = GenderOptions.Male, Email = "sigma@alpha.com", CountryId = countryResponse.CountryId, ReceiveNewsLetters = false };
            PersonResponse expectedPersonResponse = _personService.AddPerson(personAddRequest);

            PersonResponse? actualPerson = _personService.GetPersonByPersonId(expectedPersonResponse.PersonId);

            //Assert
            Assert.Equal(expectedPersonResponse, actualPerson);

        }

        #endregion

        #region GetAllPersons

        [Fact]
        public void GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> personResponses = _personService.GetAllPersons();

            //Assert
            Assert.Empty(personResponses);

        }

        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest countryAddRequestOne = new CountryAddRequest() { CountryName = "Bharat" };
            CountryAddRequest countryAddRequestTwo = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse countryResponseOne = _countryService.AddCountry(countryAddRequestOne);
            CountryResponse countryResponseTwo = _countryService.AddCountry(countryAddRequestTwo);

            PersonAddRequest personAddRequestOne = new PersonAddRequest() { PersonName = "alpha", Address = "alpha city", CountryId = countryResponseOne.CountryId, ReceiveNewsLetters = false, DateOfBirth = DateTime.Parse("2002-08-12"), Email = "alpha@sigma.com", Gender = GenderOptions.Male };
            PersonAddRequest personAddRequestTwo = new PersonAddRequest() { PersonName = "beta", Address = "beta city", CountryId = countryResponseTwo.CountryId, ReceiveNewsLetters = true, DateOfBirth = DateTime.Parse("2005-09-12"), Email = "beta@sigma.com", Gender = GenderOptions.Female };

            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>() { personAddRequestOne, personAddRequestTwo };
            List<PersonResponse> expectedPersonResponses = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse = _personService.AddPerson(personAddRequest);
                expectedPersonResponses.Add(personResponse);
            }

            //Act
            List<PersonResponse> actualPersonResponsees = _personService.GetAllPersons();


            //Assert
            foreach (PersonResponse expectedPerson in expectedPersonResponses)
            {
                Assert.Contains(expectedPerson, actualPersonResponsees);
            }

        }

        #endregion

        #region GetFilteredPerson

        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryAddRequest countryAddRequestOne = new CountryAddRequest() { CountryName = "Bharat" };
            CountryAddRequest countryAddRequestTwo = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse countryResponseOne = _countryService.AddCountry(countryAddRequestOne);
            CountryResponse countryResponseTwo = _countryService.AddCountry(countryAddRequestTwo);

            PersonAddRequest personAddRequestOne = new PersonAddRequest() { PersonName = "alpha", Address = "alpha city", CountryId = countryResponseOne.CountryId, ReceiveNewsLetters = false, DateOfBirth = DateTime.Parse("2002-08-12"), Email = "alpha@sigma.com", Gender = GenderOptions.Male };
            PersonAddRequest personAddRequestTwo = new PersonAddRequest() { PersonName = "beta", Address = "beta city", CountryId = countryResponseTwo.CountryId, ReceiveNewsLetters = true, DateOfBirth = DateTime.Parse("2005-09-12"), Email = "beta@sigma.com", Gender = GenderOptions.Female };

            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>() { personAddRequestOne, personAddRequestTwo };
            List<PersonResponse> expectedPersonResponses = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse = _personService.AddPerson(personAddRequest);
                expectedPersonResponses.Add(personResponse);
            }

            //Act
            List<PersonResponse> actualPersonResponsees = _personService.GetFilteredPersons(nameof(Person.PersonName), "");


            //Assert
            foreach (PersonResponse expectedPerson in expectedPersonResponses)
            {
                Assert.Contains(expectedPerson, actualPersonResponsees);
            }

        }

        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest countryAddRequestOne = new CountryAddRequest() { CountryName = "Bharat" };
            CountryAddRequest countryAddRequestTwo = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse countryResponseOne = _countryService.AddCountry(countryAddRequestOne);
            CountryResponse countryResponseTwo = _countryService.AddCountry(countryAddRequestTwo);

            PersonAddRequest personAddRequestOne = new PersonAddRequest() { PersonName = "alpha", Address = "alpha city", CountryId = countryResponseOne.CountryId, ReceiveNewsLetters = false, DateOfBirth = DateTime.Parse("2002-08-12"), Email = "alpha@sigma.com", Gender = GenderOptions.Male };
            PersonAddRequest personAddRequestTwo = new PersonAddRequest() { PersonName = "beta", Address = "beta city", CountryId = countryResponseTwo.CountryId, ReceiveNewsLetters = true, DateOfBirth = DateTime.Parse("2005-09-12"), Email = "beta@sigma.com", Gender = GenderOptions.Female };

            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>() { personAddRequestOne, personAddRequestTwo };
            List<PersonResponse> expectedPersonResponse = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse = _personService.AddPerson(personAddRequest);
                expectedPersonResponse.Add(personResponse);
            }

            //Act
            List<PersonResponse> actualPersonResponse = _personService.GetFilteredPersons(nameof(Person.PersonName), "be");


            //Assert
            foreach (PersonResponse expectedPerson in expectedPersonResponse)
            {
                if (expectedPerson.PersonName.Contains("be", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(expectedPerson, actualPersonResponse);
                }
            }

        }


        #endregion

        #region GetSortedPersons

        [Fact]
        public void GetSortedPersons()
        {
            //Arrange
            CountryAddRequest countryAddRequestOne = new CountryAddRequest() { CountryName = "Bharat" };
            CountryAddRequest countryAddRequestTwo = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse countryResponseOne = _countryService.AddCountry(countryAddRequestOne);
            CountryResponse countryResponseTwo = _countryService.AddCountry(countryAddRequestTwo);

            PersonAddRequest personAddRequestOne = new PersonAddRequest() { PersonName = "alpha", Address = "alpha city", CountryId = countryResponseOne.CountryId, ReceiveNewsLetters = false, DateOfBirth = DateTime.Parse("2002-08-12"), Email = "alpha@sigma.com", Gender = GenderOptions.Male };
            PersonAddRequest personAddRequestTwo = new PersonAddRequest() { PersonName = "beta", Address = "beta city", CountryId = countryResponseTwo.CountryId, ReceiveNewsLetters = true, DateOfBirth = DateTime.Parse("2005-09-12"), Email = "beta@sigma.com", Gender = GenderOptions.Female };

            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>() { personAddRequestOne, personAddRequestTwo };
            List<PersonResponse> expectedPersonResponse = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse = _personService.AddPerson(personAddRequest);
                expectedPersonResponse.Add(personResponse);
            }

            //Act
            List<PersonResponse> allPersons = _personService.GetAllPersons();
            List<PersonResponse> actualPersonResponse = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);
            expectedPersonResponse = expectedPersonResponse.OrderByDescending(person => person.PersonName).ToList();

            //Assert
            for (int i = 0; i < expectedPersonResponse.Count; i++)
            {
                Assert.Equal(expectedPersonResponse[i], actualPersonResponse[i]);
            }
        }

        #endregion

        #region Update person

        [Fact]
        public void UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(()=>
            {
                //Act
                PersonResponse personResponse = _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() { PersonId = Guid.NewGuid()};

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_NullPersonName()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Bharat" };
            CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest() { PersonName = "Abhi", CountryId = countryResponse.CountryId, Email = "abhi@bing.com", Gender = GenderOptions.Male };
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName=null;

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_PersonFullDetails()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Bharat" };
            CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest() { PersonName = "Abhi", CountryId = countryResponse.CountryId, Address = "Karnataka", DateOfBirth = DateTime.Parse("2000-03-12"), Email ="a@b.com", Gender= GenderOptions.Male, ReceiveNewsLetters = true };
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "Abhishek";
            personUpdateRequest.Email = "abhi@bing.com";

            //Act
            PersonResponse expectedPersonResponse = _personService.UpdatePerson(personUpdateRequest);

            PersonResponse? actualPersonResponse = _personService.GetPersonByPersonId(expectedPersonResponse.PersonId);

            //Assert
            Assert.Equal(expectedPersonResponse, actualPersonResponse);
        }

        #endregion

        #region Delete_Person

        [Fact]
        public void DeletePerson_NullPersonId()
        {
            //Arrange
            Guid? personId = null;  

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.DeletePerson(personId);
            });
        }

        [Fact]
        public void DeletePerson_InvalidPersonId()
        {
            //Arrange
            Guid inValidPersonId = Guid.NewGuid();            

            //Act
            bool isPersonDeleted = _personService.DeletePerson(inValidPersonId);

            //Assert
            Assert.False(isPersonDeleted);
        }

        [Fact]
        public void DeletePerson_ValidPersonId()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Bharat" };
            CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest() { PersonName = "Abhi", CountryId = countryResponse.CountryId, Address = "Karnataka", DateOfBirth = DateTime.Parse("2000-01-23"), Email = "a@b.c", Gender = GenderOptions.Male, ReceiveNewsLetters = false };
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            //Act
            bool isPersonDeleted = _personService.DeletePerson(personResponse.PersonId);

            //Assert
            Assert.True(isPersonDeleted);
        }

        #endregion
    }
}
