using GeoIpProject.Api.Services.Interfaces.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Api.Services.Interfaces
{
    public interface IGeoIpProjectService
    {
        Task<GetIpInfoResponse> GetIpInfoAsync(string ip, CancellationToken cancellationToken);

        Task<CreateBatchResponse> CreateIpBatchesAsync(CreateBatchRequest request, CancellationToken cancellationToken);

        Task<GetBatchProcessingStatusResponse> GetBatchProcessingStatusAsync(string id, CancellationToken cancellationToken);
    }
}
