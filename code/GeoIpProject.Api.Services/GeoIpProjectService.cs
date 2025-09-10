using GeoIpProject.Api.Services.Interfaces;
using GeoIpProject.Api.Services.Interfaces.Models;
using GeoIpProject.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Api.Services
{
    public class GeoIpProjectService : IGeoIpProjectService
    {
        private readonly IFreeGeoIpService _geoIpService;
        private readonly IBatchService _batchService;
        private readonly ILogger<GeoIpProjectService> _logger;

        public GeoIpProjectService(IFreeGeoIpService geoIpService, ILogger<GeoIpProjectService> logger,
            IBatchService batchService)
        {
            _geoIpService = geoIpService ?? throw new ArgumentNullException(nameof(geoIpService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _batchService = batchService ?? throw new ArgumentNullException(nameof(batchService));
        }

        public async Task<GetIpInfoResponse> GetIpInfoAsync(string ip, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Ip Info start");

            var resp = await _geoIpService.IpLookupAsync(ip, cancellationToken);

            _logger.LogInformation("Get Ip Info finish");
            return resp.ToGetIpInfoResponse();
        }

        public async Task<CreateBatchResponse> CreateIpBatchesAsync(CreateBatchRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Create Ip Batches start");

            var resp = await _batchService.CreateBatchAsync(request.Ips, cancellationToken);

            _logger.LogInformation("Create Ip Batches finish");
            return new CreateBatchResponse { BatchId = resp.Id };
        }

        public async Task<GetBatchProcessingStatusResponse> GetBatchProcessingStatusAsync(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Batch Processing Status start");

            var resp = await _batchService.GetBatchStatusAsync(id, cancellationToken);

            _logger.LogInformation("Get Batch Processing Status finish");
            return new GetBatchProcessingStatusResponse { EstimatedCompletion = resp.EstimatedCompletion, Progress = resp.Progress };
        }
    }
}
