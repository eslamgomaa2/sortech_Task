namespace Countriestask.Entities
{
    public class BlockedAttemptLog
    {
        public string IPAddress { get; set; }
        public DateTime Timestamp { get; set; }
        public string CountryCode { get; set; }
        public bool IsBlocked { get; set; }
        public string UserAgent { get; set; }
    }
}
