using DigMap.Domain.Entities;
using DigMap.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DigMap.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<FindItem> FindItems { get; set; }
        public DbSet<Coin> Coins { get; set; }
        public DbSet<Artifact> Artifacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FindItem>()
                .UseTphMappingStrategy();
        }
    }
}