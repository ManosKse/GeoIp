using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Services.Interfaces
{
    public interface IBatchService
    {
        Task<BatchModel> CreateBatchAsync(IEnumerable<string> ips, CancellationToken cancellationToken);

        Task<BatchStatus> GetBatchStatusAsync(string batchId, CancellationToken cancellationToken);
    }
}