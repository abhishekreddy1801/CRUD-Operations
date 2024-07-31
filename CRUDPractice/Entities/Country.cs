namespace Entities
{
    /// <summary>
    /// Domain model for stroing country model
    /// </summary>
    public class Country
    {
        public Guid CountryID { get; set; }

        public string? CountryName { get; set; }
    }
}
