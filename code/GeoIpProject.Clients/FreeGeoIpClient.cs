using GeoIpProject.Clients.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Clients
{
    public class FreeGeoIpClient : IFreeGeoIpClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly FreeGeoIpClientConfig _config;
        private readonly HttpClient _httpClient;
        private readonly ILogger<FreeGeoIpClient> _logger;
        public const string HTTP_CLIENT_NAME = nameof(FreeGeoIpClient);

        public FreeGeoIpClient(IHttpClientFactory clientFactory, IOptions<FreeGeoIpClientConfig> config,
            ILogger<FreeGeoIpClient> logger)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _httpClient = _clientFactory.CreateClient(HTTP_CLIENT_NAME);
        }

        public async Task<GeoIpResponse> LookupAsync(string ip, CancellationToken cancellationToken)
        {
            _logger.LogInformation("FreeGeoIpClient.LookupAsync start");

            var resp = await _httpClient.GetFromJsonAsync<GeoIpResponse>($"{_config.ServicePath}?apikey=sgiPfh4j3aXFR3l2CnjWqdKQzxpqGn9pX5b3CUsz&ip={ip}", cancellationToken).ConfigureAwait(false)
                ?? throw new HttpRequestException("Empty response");

            _logger.LogInformation("FreeGeoIpClient.LookupAsync finish");
            return resp;
        }
    }
}
