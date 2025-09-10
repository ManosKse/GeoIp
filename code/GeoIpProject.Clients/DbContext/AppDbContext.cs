using GeoIpProject.Clients.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoIpProject.Clients
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Batch> Batches => Set<Batch>();
        public DbSet<IpLookupItem> IpLookupItem => Set<IpLookupItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batch>()
                        .HasMany(b => b.Items)
                        .WithOne(i => i.Batch!)
                        .HasForeignKey(i => i.BatchId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}