using Countriestask.Entities;
using static System.Reflection.Metadata.BlobBuilder;

namespace Countriestask.Repository.logAttemptRepo
{
    public class LogRepo : ILogRepo
    {
        private readonly List<BlockedAttemptLog> _logs = new();
       

        public LogRepo(List<BlockedAttemptLog> logs, object logLock)
        {
            _logs = logs;
          
        }

        public void AddLog(BlockedAttemptLog log)
        {
                _logs.Add(log);
            
        }

        public List<BlockedAttemptLog> GetLogs(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

                return _logs
                    .OrderByDescending(l => l.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            
        }
    }
}
