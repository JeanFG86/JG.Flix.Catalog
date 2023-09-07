using JG.Flix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Jg.Flix.Catalog.Api.Configurations;

public static class ConnectionsConfig
{
    public static IServiceCollection AddAppConnections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbConnection(configuration);
        return services;
    }

    private static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CatalogDb");
        services.AddDbContext<FlixCatalogDbContext>(
                options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );
        return services;
    }
}
