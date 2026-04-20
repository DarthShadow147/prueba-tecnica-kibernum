using Microsoft.EntityFrameworkCore;
using PruebaTecnicaKibernum.Domain.Entities;

namespace PruebaTecnicaKibernum.Infrastructure.DataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Character> Character => Set<Character>();
        public DbSet<HiringRequest> HiringRequest => Set<HiringRequest>();

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Character>()
                .HasIndex(c => c.ExternalId)
                .IsUnique();
        }
    }
}
