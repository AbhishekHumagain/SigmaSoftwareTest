using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SigmaSoftware.Application.Common.Interfaces;
using SigmaSoftware.Infrastructure.Persistence;
using SigmaSoftware.Infrastructure.Persistence.Interceptors;
using SigmaSoftware.Infrastructure.Services;

namespace SigmaSoftware.Infrastructure.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        #region Service and Connection

        // Register the AuditableEntitySaveChangesInterceptor as Scoped
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<SigmaSigmaDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(SigmaSigmaDbContext).Assembly.FullName));

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });

        #endregion

        #region Injected Services
        services.AddScoped<ISigmaDbContext>(provider => provider.GetRequiredService<SigmaSigmaDbContext>());
        services.AddScoped<SigmaDbContextInitializer>();
        
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddHttpClient();

        #endregion

        return services;
    }

}