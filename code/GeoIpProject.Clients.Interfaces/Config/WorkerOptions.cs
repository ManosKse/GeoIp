namespace GeoIpProject.Clients.Interfaces
{
    public class WorkerOptions
    {
        public const string Name = "WorkerOptions";

        public int PollIntervalSeconds { get; set; }

        public int MaxBatchItems { get; set; }
    }
}
