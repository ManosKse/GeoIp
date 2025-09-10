using GeoIpProject.Clients.Interfaces;
using GeoIpProject.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Services
{
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepo;
        private readonly IIpLookupRepository _ipRepo;
        private readonly IFreeGeoIpClient _geoClient;
        private readonly ILogger<BatchService> _logger;

        public BatchService(IBatchRepository batchRepo,
                            IIpLookupRepository ipRepo,
                            IFreeGeoIpClient geoClient,
                            ILogger<BatchService> logger)
        {
            _batchRepo = batchRepo ?? throw new ArgumentNullException(nameof(batchRepo));
            _ipRepo = ipRepo ?? throw new ArgumentNullException(nameof(ipRepo));
            _geoClient = geoClient ?? throw new ArgumentNullException(nameof(geoClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BatchModel> CreateBatchAsync(IEnumerable<string> ips, CancellationToken cancellationToken)
        {
            var list = ips.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList();
            var batch = new Batch
            {
                TotalCount = list.Count,
                Status = Clients.Interfaces.BatchStatus.Pending
            };

            foreach (var ip in list)
                batch.Items.Add(new IpLookupItem { Ip = ip, Status = ItemStatus.Pending });

            await _batchRepo.AddAsync(batch, cancellationToken);

            _logger.LogInformation("Batch {BatchId} created with {Count} items", batch.Id, batch.TotalCount);

            // Start local fire-and-forget processing for quick feedback (hosted worker will also pick up)
            _ = Task.Run(() => ProcessBatchAsync(batch.Id, cancellationToken));

            return new BatchModel { Id = batch.Id, TotalCount = batch.TotalCount };
        }

        public async Task<GeoIpProject.Services.Interfaces.BatchStatus> GetBatchStatusAsync(string batchId, CancellationToken cancellationToken)
        {
            var batch = await _batchRepo.GetAsync(batchId, cancellationToken);
            if (batch == null) return null;

            DateTime eta = default;
            if (batch.StartedAt.HasValue && batch.TotalCount > 0)
            {
                var done = Math.Max(1, batch.CompletedCount + batch.FailedCount);
                var elapsed = DateTime.UtcNow - batch.StartedAt.Value;
                var avgTicks = elapsed.Ticks / done;
                var remaining = Math.Max(0, batch.TotalCount - done);
                eta = DateTime.UtcNow + TimeSpan.FromTicks(avgTicks * remaining);
            }

            return new GeoIpProject.Services.Interfaces.BatchStatus
            {
                BatchId = batch.Id,
                Completed = batch.CompletedCount,
                Failed = batch.FailedCount,
                Total = batch.TotalCount,
                EstimatedCompletion = eta,
                Status = batch.Status.ToString()
            };
        }

        private async Task ProcessBatchAsync(string batchId, CancellationToken cancellationToken)
        {
            var batch = await _batchRepo.GetAsync(batchId, cancellationToken);
            if (batch == null) return;

            batch.Status = Clients.Interfaces.BatchStatus.Running;
            batch.StartedAt = DateTime.UtcNow;
            await _batchRepo.UpdateAsync(batch, cancellationToken);

            foreach (var item in batch.Items.Where(i => i.Status == ItemStatus.Pending))
            {
                try
                {
                    item.Status = ItemStatus.Running;
                    item.StartedAt = DateTime.UtcNow;
                    await _ipRepo.UpdateAsync(item, cancellationToken);

                    var res = await _geoClient.LookupAsync(item.Ip, cancellationToken);

                    item.CountryCode = res.Data.Location.Country.CountryCode;
                    item.CountryName = res.Data.Location.Country.CountryName;
                    item.TimeZone = res.Data.TimeZone.Id;
                    item.Latitude = res.Data.Location.Latitude;
                    item.Longitude = res.Data.Location.Longitude;
                    item.Status = ItemStatus.Succeeded;
                    item.CompletedAt = DateTime.UtcNow;
                    batch.CompletedCount++;
                }
                catch (Exception ex)
                {
                    item.Status = ItemStatus.Failed;
                    item.Error = ex.Message;
                    item.CompletedAt = DateTime.UtcNow;
                    batch.FailedCount++;
                }

                await _ipRepo.UpdateAsync(item, cancellationToken);
                await _batchRepo.UpdateAsync(batch, cancellationToken);
            }

            batch.Status = batch.FailedCount > 0 ? Clients.Interfaces.BatchStatus.Failed : Clients.Interfaces.BatchStatus.Completed;
            batch.CompletedAt = DateTime.UtcNow;
            await _batchRepo.UpdateAsync(batch, cancellationToken);
        }
    }
}
