﻿using Jg.Flix.Catalog.Api.Configurations.Policies;
using Jg.Flix.Catalog.Api.Filters;

namespace Jg.Flix.Catalog.Api.Configurations;

public static class ControllersConfig
{
    public static IServiceCollection AddAndConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(options => 
            options.Filters.Add(typeof(ApiGlobalExceptionFilter))
            ).AddJsonOptions(jsonOpt =>
            {
                jsonOpt.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCasePolicy();
            });
        services.AddDocumentation();

        return services;
    }

    private static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static WebApplication UseDocumentation(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        return app;
    }
}
