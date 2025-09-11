using GeoIpProject.Clients.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Clients
{
    public class IpLookupWorker : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<IpLookupWorker> _logger;
        private readonly WorkerOptions _options;

        public IpLookupWorker(IServiceProvider services, ILogger<IpLookupWorker> logger, IOptions<WorkerOptions> options)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("IpLookupWorker started, polling every {Interval}s", _options.PollIntervalSeconds);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var geo = scope.ServiceProvider.GetRequiredService<IFreeGeoIpClient>();

                    var pending = await db.IpLookupItem
                                          .Include(i => i.Batch)
                                          .Where(i => i.Status == ItemStatus.Pending)
                                          .Take(_options.MaxBatchItems)
                                          .ToListAsync(cancellationToken);

                    foreach (var item in pending)
                    {
                        if (cancellationToken.IsCancellationRequested) break;

                        try
                        {
                            item.Status = ItemStatus.Running;
                            item.StartedAt = DateTime.UtcNow;
                            await db.SaveChangesAsync(cancellationToken);

                            var res = await geo.LookupAsync(item.Ip, cancellationToken);

                            item.CountryCode = res.Data.Location.Country.CountryCode;
                            item.CountryName = res.Data.Location.Country.CountryName;
                            item.TimeZone = res.Data.TimeZone.Id;
                            item.Latitude = res.Data.Location.Latitude;
                            item.Longitude = res.Data.Location.Longitude;
                            item.Status = ItemStatus.Succeeded;
                            item.CompletedAt = DateTime.UtcNow;
                            item.Batch!.CompletedCount++;
                        }
                        catch (Exception ex)
                        {
                            item.Status = ItemStatus.Failed;
                            item.Error = ex.Message;
                            item.CompletedAt = DateTime.UtcNow;
                            item.Batch!.FailedCount++;
                        }

                        if (item.Batch!.CompletedCount + item.Batch.FailedCount >= item.Batch.TotalCount)
                        {
                            item.Batch.Status = item.Batch.FailedCount > 0 ? BatchStatus.Failed : BatchStatus.Completed;
                            item.Batch.CompletedAt = DateTime.UtcNow;
                        }
                        else
                        {
                            item.Batch.Status = BatchStatus.Running;
                            item.Batch.StartedAt ??= DateTime.UtcNow;
                        }

                        await db.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during background processing");
                }

                await Task.Delay(TimeSpan.FromSeconds(_options.PollIntervalSeconds), cancellationToken);
            }
        }
    }
}
