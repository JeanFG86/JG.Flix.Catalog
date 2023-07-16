using JG.Flix.Catalog.Domain.Entity;
using JG.Flix.Catalog.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace JG.Flix.Catalog.Infra.Data.EF;
public class FlixCatalogDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();

    public FlixCatalogDbContext(DbContextOptions<FlixCatalogDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}
