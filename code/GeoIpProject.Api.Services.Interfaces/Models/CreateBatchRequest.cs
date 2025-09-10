using System.Collections.Generic;

namespace GeoIpProject.Api.Services.Interfaces.Models
{
    public class CreateBatchRequest
    {
        public IEnumerable<string> Ips { get; set; }
    }
}
