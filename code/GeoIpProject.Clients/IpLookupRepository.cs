using GeoIpProject.Clients.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Clients
{
    public class IpLookupRepository : IIpLookupRepository
    {
        private readonly AppDbContext _dbContext;

        public IpLookupRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task UpdateAsync(IpLookupItem item, CancellationToken cancellationToken)
        {
            _dbContext.IpLookupItem.Update(item);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}