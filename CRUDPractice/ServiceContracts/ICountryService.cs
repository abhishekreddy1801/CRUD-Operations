using Entities;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents logic to Entites business
    /// </summary>
    public interface ICountryService
    {
        CountryResponse AddCountry(CountryAddRequest? countryaddRequest);

        List<CountryResponse> GetAllCountries();

        CountryResponse? GetCountryByCountryId(Guid? countryId);

    }
}
