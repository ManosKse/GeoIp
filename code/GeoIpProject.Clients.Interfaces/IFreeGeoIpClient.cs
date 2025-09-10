using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Clients.Interfaces
{
    public interface IFreeGeoIpClient
    {
        Task<GeoIpResponse> LookupAsync(string ip, CancellationToken cancellationToken);
    }
}
