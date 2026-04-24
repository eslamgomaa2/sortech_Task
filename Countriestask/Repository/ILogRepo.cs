using Countriestask.Entities;

namespace Countriestask.Repository
{
    public interface ILogRepo
    {

        void AddLog(BlockedAttemptLog log);
        List<BlockedAttemptLog> GetLogs(int page, int pageSize);
    }
}
