using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SigmaSoftware.Application.Common.Interfaces;
using SigmaSoftware.Infrastructure.Persistence;
using SigmaSoftware.Infrastructure.Services;
using DbContext = SigmaSoftware.Infrastructure.Persistence.DbContext;

namespace SigmaSoftware.Infrastructure.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        #region Service and Connection

        services.AddDbContext<DbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(DbContext).Assembly.FullName));
                
                if (environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }
            }
        );


        var connectionString = configuration.GetConnectionString("DefaultConnection");

        #endregion
        #region Injected Services

        services.AddScoped<IDbContext>(provider => provider.GetRequiredService<DbContext>());
        services.AddScoped<DbContextInitializer>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));


        services.AddHttpClient();

        #endregion
       
        return services;
    }
}