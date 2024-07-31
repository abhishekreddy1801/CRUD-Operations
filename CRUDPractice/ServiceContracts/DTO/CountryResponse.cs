using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class to return country obj
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            if (obj.GetType() != typeof(CountryResponse)) return false;

            CountryResponse countryResponse = (CountryResponse) obj;

            return this.CountryId == countryResponse.CountryId && this.CountryName == countryResponse.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExtension 
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse() { CountryId = country.CountryID, CountryName = country.CountryName };
        }
    }

}
