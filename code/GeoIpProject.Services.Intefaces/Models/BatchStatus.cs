using System;

namespace GeoIpProject.Services.Interfaces
{
    public class BatchStatus
    {
        public string BatchId { get; set; }
        public int Completed { get; set; }
        public int Failed { get; set; }
        public int Total { get; set; }
        public string Progress => $"{Completed + Failed}/{Total}";
        public DateTime EstimatedCompletion { get; set; }
        public string Status { get; set; } = default!;
    }
}
