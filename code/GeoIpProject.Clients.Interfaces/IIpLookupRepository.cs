using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Clients.Interfaces
{
    public interface IIpLookupRepository
    {
        Task UpdateAsync(IpLookupItem item, CancellationToken cancellationToken);
    }
}
