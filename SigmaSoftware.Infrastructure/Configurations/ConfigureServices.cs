using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SigmaSoftware.Application.Common.Interfaces;
using SigmaSoftware.Infrastructure.Persistence;
using SigmaSoftware.Infrastructure.Persistence.Interceptors;
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

        #endregion
        #region Injected Services
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<IDbContext>(provider =>
        {
            var options = provider.GetRequiredService<DbContextOptions<DbContext>>();
            var mediator = provider.GetRequiredService<IMediator>();
            var interceptor = provider.GetRequiredService<AuditableEntitySaveChangesInterceptor>();

            return new DbContext(options, mediator, interceptor);
        });
        services.AddScoped<DbContextInitializer>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));


        services.AddHttpClient();

        #endregion
       
        return services;
    }
}