namespace Countriestask.Repository
{
    public interface IBlockedCountryRepo
    {

        bool AddCountry(string countryCode, DateTime? expiresAt = null);
        bool RemoveCountry(string countryCode);
        bool IsBlocked(string countryCode);
        List<string> GetBlockedCountries(int page, int pageSize);
        void CleanExpiredTemporalBlocks();
    }
}
