using JG.Flix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Jg.Flix.Catalog.Api.Configurations;

public static class ConnectionsConfig
{
    public static IServiceCollection AddAppConnections(this IServiceCollection services)
    {
        services.AddDbConnection();
        return services;
    }

    private static IServiceCollection AddDbConnection(this IServiceCollection services)
    {
        services.AddDbContext<FlixCatalogDbContext>(
                options => options.UseInMemoryDatabase("InMemory-DSV-Database")
            );
        return services;
    }
}
