using GeoIpProject.Clients.Interfaces;
using GeoIpProject.Services.Extensions;
using GeoIpProject.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Services
{
    public class FreeGeoIpService : IFreeGeoIpService
    {
        private readonly IFreeGeoIpClient _freeGeoIpClient;
        private readonly ILogger<FreeGeoIpService> _logger;

        public FreeGeoIpService(IFreeGeoIpClient freeGeoIpClient, ILogger<FreeGeoIpService> logger)
        {
            _freeGeoIpClient = freeGeoIpClient ?? throw new ArgumentNullException(nameof(freeGeoIpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IpLookupResponse> IpLookupAsync(string ip, CancellationToken cancellationToken)
        {
            _logger.LogInformation("FreeGeoIpService.LookupAsync start");

            if (!System.Net.IPAddress.TryParse(ip, out _))
                throw new InvalidCastException("Invalid IP address");

            var resp = await _freeGeoIpClient.LookupAsync(ip, cancellationToken);

            _logger.LogInformation("FreeGeoIpService.LookupAsync finish");
            return resp.ToIpLookupResponse();
        }
    }
}
