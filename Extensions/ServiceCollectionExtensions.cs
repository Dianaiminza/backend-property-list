using backend_property_list.DatabaseSeeds.Abstractions;
using backend_property_list.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_property_list.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseContext(
    this IServiceCollection services,
    IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var enableLogging = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        services.AddDbContext<DatabaseContext>(
            dbContextOptions => dbContextOptions
                .UseSqlServer(connectionString)
                .EnableSensitiveDataLogging(enableLogging)
                .EnableDetailedErrors(enableLogging)
                .UseSqlServer(
                    connectionString,
                    options => options.EnableRetryOnFailure()));

        return services;
    }

    public static WebApplication MigrateDatabase(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        using var appContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        if (appContext.Database.GetPendingMigrations().Any())
        {
            appContext.Database.Migrate();
        }

        return webApplication;
    }
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });
    }
    internal static IApplicationBuilder InitializeDatabaseData(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        var initializers = serviceScope.ServiceProvider.GetServices<IDatabaseSeeder>();

        foreach (var initializer in initializers)
        {
            initializer.Initialize();
        }

        return app;
    }
    internal static IServiceCollection AddDatabaseSeeders(this IServiceCollection services)
    {
        services.AddScoped<IDatabaseSeeder, DataSeeder>();


        return services;
    }
   
}
