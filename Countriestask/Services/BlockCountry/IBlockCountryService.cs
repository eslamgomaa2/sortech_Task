using Countriestask.Entities;

namespace Countriestask.Services.BlockCountry
{
    public interface IBlockCountryService
    {


        string BlockCountry(string code, int? durationMinutes);
        string RemoveCountry(string code);
        List<string> GetBlockedCountries(int page, int pageSize);
        Task<bool> CheckIpBlockedAsync(string? ipAddress = null);
        List<BlockedAttemptLog> GetBlockedAttempts(int page, int pageSize);

        Task<string> GetCountryCodeAsync(string? ipAddress);
    }
}
