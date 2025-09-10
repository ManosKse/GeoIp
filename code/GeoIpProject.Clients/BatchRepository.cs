using GeoIpProject.Clients.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Clients
{
    public class BatchRepository : IBatchRepository
    {
        private readonly AppDbContext _dbContext;

        public BatchRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task AddAsync(Batch batch, CancellationToken cancellationToken)
        {
            _dbContext.Batches.Add(batch);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Batch> GetAsync(string id, CancellationToken cancellationToken)
        {
            // Use tracking because we will update entities later
            return await _dbContext.Batches.Include(b => b.Items).FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Batch batch, CancellationToken cancellationToken)
        {
            _dbContext.Batches.Update(batch);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}