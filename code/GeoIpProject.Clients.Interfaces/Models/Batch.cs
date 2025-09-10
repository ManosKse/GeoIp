using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeoIpProject.Clients.Interfaces
{
    public class Batch
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int TotalCount { get; set; }
        public int CompletedCount { get; set; }
        public int FailedCount { get; set; }
        public BatchStatus Status { get; set; } = BatchStatus.Pending;
        public ICollection<IpLookupItem> Items { get; set; } = new List<IpLookupItem>();
    }
}
