using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Clients.Interfaces
{
    public interface IBatchRepository
    {
        Task AddAsync(Batch batch, CancellationToken cancellationToken);

        Task<Batch> GetAsync(string id, CancellationToken cancellationToken);

        Task UpdateAsync(Batch batch, CancellationToken cancellationToken);
    }
}


