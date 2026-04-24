using Countriestask.Repository.Blockcountriesrepo;

namespace Countriestask.Services.BackgroundServices
{
    public class TemporalBlockCountryCleanupService : BackgroundService
    {
        private readonly IBlockedCountryRepo _store;

        public TemporalBlockCountryCleanupService(IBlockedCountryRepo store)
        {
            _store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _store.CleanExpiredTemporalBlocks();

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
