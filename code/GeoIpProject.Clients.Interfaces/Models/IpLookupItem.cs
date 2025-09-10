using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoIpProject.Clients.Interfaces
{
    public class IpLookupItem
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string BatchId { get; set; }

        [ForeignKey(nameof(BatchId))]
        public Batch Batch { get; set; }

        [Required, MaxLength(64)]
        public string Ip { get; set; } = default!;

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string TimeZone { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public ItemStatus Status { get; set; } = ItemStatus.Pending;

        public string Error { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime CompletedAt { get; set; }
    }
}
