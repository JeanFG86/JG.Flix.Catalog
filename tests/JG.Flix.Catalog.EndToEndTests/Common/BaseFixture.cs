using Bogus;
using JG.Flix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace JG.Flix.Catalog.EndToEndTests.Common;
public abstract class BaseFixture
{
    public Faker Faker { get; set; }

    public ApiClient ApiClient { get; set; }

    protected BaseFixture() => Faker = new Faker("pt_BR");

    public FlixCatalogDbContext CreateDbContext()
    {
        var context = new FlixCatalogDbContext(new DbContextOptionsBuilder<FlixCatalogDbContext>().UseInMemoryDatabase("end2end-test-db").Options);

        return context;
    }
}
