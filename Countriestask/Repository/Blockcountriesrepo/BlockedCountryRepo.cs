using Countriestask.Entities;
using Countriestask.Repository.logAttemptRepo;
using System.Collections.Concurrent;

namespace Countriestask.Repository.Blockcountriesrepo
{
    public class BlockedCountryRepo : IBlockedCountryRepo
    {
        private readonly ConcurrentDictionary<string, DateTime?> _blockedCountries ;
        

        public BlockedCountryRepo()
        {
            _blockedCountries = new ConcurrentDictionary<string, DateTime?>();
            
        }
        public bool AddCountry(string countryCode, DateTime? expiresAt)
        {
            
            return _blockedCountries.TryAdd(countryCode.ToUpperInvariant(), expiresAt);
        }

        public bool RemoveCountry(string countryCode)
        {
            
            return _blockedCountries.TryRemove(countryCode.ToUpperInvariant(), out _);
        }


        public bool IsBlocked(string countryCode)
        {
            if (_blockedCountries.TryGetValue(countryCode, out var expiresAt))
            {
                
                if (expiresAt.HasValue && expiresAt <= DateTime.UtcNow)
                {
                    _blockedCountries.TryRemove(countryCode, out _);
                    return false;
                }
                return true; 
            }
            return false;
        }

        public List<string> GetBlockedCountries(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            return _blockedCountries.Keys
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public void CleanExpiredTemporalBlocks()
        {
            var expired = _blockedCountries
                .Where(kv => kv.Value.HasValue && kv.Value.Value <= DateTime.UtcNow)
                .Select(kv => kv.Key)
                .ToList();

            foreach (var key in expired)
                _blockedCountries.TryRemove(key, out _);
        }

      
    }
}

