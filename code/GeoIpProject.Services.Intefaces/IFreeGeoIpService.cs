using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Services.Interfaces
{
    public interface IFreeGeoIpService
    {
        Task<IpLookupResponse> IpLookupAsync(string ip, CancellationToken cancellationToken);
    }
}