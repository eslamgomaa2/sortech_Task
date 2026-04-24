using Countriestask.Entities;
using Countriestask.Repository;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace Countriestask.Services.BlockCountry
{

    public class BlockCountryService : IBlockCountryService
    {
        private readonly IBlockedCountryRepo _blockStore;
        private readonly ILogRepo _logStore;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public BlockCountryService(IBlockedCountryRepo blockStore, IConfiguration configuration, ILogRepo logStore, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _blockStore = blockStore;
            _logStore = logStore;
            _httpContextAccessor = httpContextAccessor;
            _httpClient= httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress= new Uri("https://api.ipgeolocation.io/");
        }

        public string BlockCountry(string code, int? durationMinutes)
        {
            if (code is null )
                return "invalid_code";

            var upper = code.ToUpperInvariant();

            if (_blockStore.IsBlocked(upper))
                return "country blockebefore";

            DateTime? expiresAt = durationMinutes.HasValue
                ? DateTime.UtcNow.AddMinutes(durationMinutes.Value)
                : null;

            bool success = _blockStore.AddCountry(upper, expiresAt);

            return success ? "Add successfuly" : "Fail to add country";
        }

        public string RemoveCountry(string code) 
        {
            if (!_blockStore.RemoveCountry(code.ToUpperInvariant()))
                return "Country doesnt exist";
            return "Country successfully unblocked.";


        }


        public async Task<string> GetCountryCodeAsync(string? ipAddress)
        {
            var apiKey = _configuration["IPGeolocationSettings:ApiKey"];
            if (apiKey is null)
            {
                return " add api key";
            }
                if (string.IsNullOrWhiteSpace(ipAddress))
            {
                ipAddress = _httpContextAccessor.HttpContext? .Connection? .RemoteIpAddress?  .ToString();
            }


            var response = await _httpClient.GetAsync($"ipgeo?apiKey={apiKey}&ip={ipAddress}");

            if (!response.IsSuccessStatusCode)
                return "Unknown";

            var json = await response.Content.ReadAsStringAsync();
            
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("Country_code2", out var countryProp))
                return "Unknown";

            return countryProp.GetString() ?? "Unknown";
        }

        public List<string> GetBlockedCountries(int page, int pageSize)
        {
            var items = _blockStore.GetBlockedCountries(page, pageSize);
            return items;
        }

        public async Task<bool> CheckIpBlockedAsync(string? ipAddress = null)
        {
            string ip = ipAddress ?? _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()!;
            var countrycode = await GetCountryCodeAsync(ip);

            var isblocked = _blockStore.IsBlocked(countrycode);
            if (isblocked)
            {

                var log = new BlockedAttemptLog
                {
                    IPAddress = ip,
                    Timestamp = DateTime.UtcNow,
                    CountryCode = countrycode,
                    IsBlocked = _blockStore.IsBlocked(countrycode),
                    UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString(),


                };

                _logStore.AddLog(log);
                return true;
            }
            return false;
        }

        public List<BlockedAttemptLog> GetBlockedAttempts(int page, int pageSize)
        {
            var items = _logStore.GetLogs(page, pageSize);
            return items;
        }


    }
}
