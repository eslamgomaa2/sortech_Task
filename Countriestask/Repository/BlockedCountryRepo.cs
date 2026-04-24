using Countriestask.Entities;
using System.Collections.Concurrent;

namespace Countriestask.Repository
{
    public class BlockedCountryRepo : IBlockedCountryRepo, ILogRepo
    {
        private readonly ConcurrentDictionary<string, DateTime?> _blockedCountries ;
        private readonly List<BlockedAttemptLog> _logs = new();
        private readonly object _logLock = new();

        public BlockedCountryRepo()
        {
            _blockedCountries = new ConcurrentDictionary<string, DateTime?>();
            _logs = new List<BlockedAttemptLog>();
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

        public void AddLog(BlockedAttemptLog log)
        {
            lock (_logLock)
            {
                _logs.Add(log);
            }
        }

        public List<BlockedAttemptLog> GetLogs(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            lock (_logLock)
            {
                return _logs
                    .OrderByDescending(l => l.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }
    }
}

