using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTest
{
    public class CountriesServiceTest
    {
        private readonly ICountryService _countryService;

        public CountriesServiceTest()
        {
            _countryService = new CountryService(false);
        }

        #region AddCountry

        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countryService.AddCountry(request);
            });

        }

        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryService.AddCountry(request);
            });

        }

        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countryService.AddCountry(request1);
                _countryService.AddCountry(request1);

            });

        }

        [Fact]
        public void AddCountry_ProperCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "Bharat" };

            //Act
            CountryResponse countryResponse = _countryService.AddCountry(request1);
            List<CountryResponse> countryResponseList = _countryService.GetAllCountries();

            //Assert
            Assert.True(countryResponse.CountryId != Guid.Empty);
            Assert.Contains(countryResponse, countryResponseList);

        }

        #endregion

        #region GetAllCountries

        [Fact]
        public void GetAllCountries_EmptyList()
        {
            //Act
            List<CountryResponse> allCountryList = _countryService.GetAllCountries();

            //Assert
            Assert.Empty(allCountryList);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> allCountryList = new List<CountryAddRequest>()
            {
                new CountryAddRequest() {CountryName = "USA"},
                new CountryAddRequest() {CountryName = "UAE"}
            };

            List<CountryResponse> expectedCountryResponsesList = new List<CountryResponse>();

            foreach (CountryAddRequest countryAddRequest in allCountryList)
            {
                expectedCountryResponsesList.Add(_countryService.AddCountry(countryAddRequest));
            }

            //Act
            List<CountryResponse> actualCountryResponsesList = _countryService.GetAllCountries();

            //Assert
            foreach(CountryResponse countryResponse in expectedCountryResponsesList)
            {
                Assert.Contains(countryResponse, actualCountryResponsesList);
            }
            
        }

        #endregion

        #region GetCountryByCountryId

        [Fact]
        public void GetCountryByCountryId_NullCountryId()
        {
            //Arrange
            Guid? countryId = null;

            //Act
             CountryResponse? country = _countryService.GetCountryByCountryId(countryId);

            //Assert
            Assert.Null(country);
        }

        [Fact]
        public void GetCountryByCountryId_ValidCountryId()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Bharat"};
            CountryResponse expectedCountryResponse = _countryService.AddCountry(countryAddRequest);

            //Act
            CountryResponse? actualCountryResponse = _countryService.GetCountryByCountryId(expectedCountryResponse.CountryId);

            //Assert
            Assert.Equal(expectedCountryResponse, actualCountryResponse);
        }

        #endregion

    }
}