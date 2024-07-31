using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly List<Country> _countries; 

        public CountryService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.AddRange(new List<Country>()
                {
                new Country() { CountryID = Guid.Parse("48A575FC-936C-4CF9-9B0A-28A0F8AA2BF8"), CountryName = "Bharat" },
                new Country() { CountryID = Guid.Parse("B9334C94-EE78-4BC8-A041-321C65F4219C"), CountryName = "USA" },
                new Country() { CountryID = Guid.Parse("2EFDD32E-1818-4D4D-A1F6-69FC19187652"), CountryName = "Russia" },
                new Country() { CountryID = Guid.Parse("966FF0AC-3658-47C0-80AD-EAC122BFEF0E"), CountryName = "England" },
                new Country() { CountryID = Guid.Parse("90A7EA19-C4BC-4487-A871-88FE9D1DC7B6"), CountryName = "Japan" },
                });
        }
        }

        public CountryResponse AddCountry(CountryAddRequest? countryaddRequest)
        {
            if (countryaddRequest is null) 
            { 
                throw new ArgumentNullException(nameof(countryaddRequest));
            }

            if (countryaddRequest.CountryName is null)
            {
                throw new ArgumentException(nameof(countryaddRequest));
            }

            if (_countries.Where(country => country.CountryName == countryaddRequest.CountryName).Count()>0)
            {
                throw new ArgumentNullException(nameof(countryaddRequest));
            }

            Country country = countryaddRequest.ToCountry();  

            country.CountryID = Guid.NewGuid();

            _countries.Add(country);

            return country.ToCountryResponse();

        }

        public List<CountryResponse> GetAllCountries()
        {
            //return _countries.Select(country => country.ToCountryResponse()).ToList();
            //                                     or

            IEnumerable<CountryResponse> countryResponses = _countries.Select(country => country.ToCountryResponse());
            List<CountryResponse> countryResponseList = countryResponses.ToList();
            return countryResponseList;
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryId)
        {
            if (countryId is null) return null;

            Country? country = _countries.FirstOrDefault(country => country.CountryID == countryId);

            return country?.ToCountryResponse();
        }
    }
}
