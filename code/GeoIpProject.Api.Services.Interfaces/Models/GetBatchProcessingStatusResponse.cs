using System;

namespace GeoIpProject.Api.Services.Interfaces.Models
{
    public class GetBatchProcessingStatusResponse
    {
        public string Progress { get; set; }
        public DateTime EstimatedCompletion { get; set; }
    }
}
