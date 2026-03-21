using Cashly.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cashly.Infrastructure.Data.Context
{
    public class CashlyDbContext : DbContext
    {
        public CashlyDbContext(DbContextOptions<CashlyDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CashlyDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
