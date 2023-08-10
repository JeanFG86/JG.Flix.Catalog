using JG.Flix.Catalog.Application.UseCases.Category.CreateCategory;

namespace Jg.Flix.Catalog.Api.Configurations
{
    public static class UseCasesConfig
    {
        public static IServiceCollection AddUseCases(
        this IServiceCollection services
    )
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateCategory).Assembly));
            return services;
        }
    }
}
