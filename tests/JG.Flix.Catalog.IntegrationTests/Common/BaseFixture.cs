using Bogus;
using JG.Flix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace JG.Flix.Catalog.IntegrationTests.Common;
public abstract class BaseFixture
{
    public Faker Faker { get; set; }

    protected BaseFixture() => Faker = new Faker("pt_BR");

    public FlixCatalogDbContext CreateDbContext(bool preserveData = false)
    {
        var context = new FlixCatalogDbContext(new DbContextOptionsBuilder<FlixCatalogDbContext>().UseInMemoryDatabase("integration-test-db").Options);

        if (preserveData == false)
            context.Database.EnsureDeleted();

        return context;
    }
}
