using Bogus;
using JG.Flix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JG.Flix.Catalog.EndToEndTests.Common;
public abstract class BaseFixture
{
    public Faker Faker { get; set; }

    public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
    public HttpClient HttpClient { get; set; }
    public ApiClient ApiClient { get; set; }

    private readonly string _dbConnectionString;

    protected BaseFixture()
    {
        Faker = new Faker("pt_BR");
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebAppFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);
        var configuration = WebAppFactory.Services.GetService(typeof(IConfiguration));
        ArgumentNullException.ThrowIfNull(configuration);
        _dbConnectionString = ((IConfiguration)configuration).GetConnectionString("CatalogDb");
    }

    public FlixCatalogDbContext CreateDbContext()
    {
        var context = new FlixCatalogDbContext(new DbContextOptionsBuilder<FlixCatalogDbContext>().UseMySql(_dbConnectionString, ServerVersion.AutoDetect(_dbConnectionString)).Options);

        return context;
    }

    public void CleanPersistence()
    {
        var context = CreateDbContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}
