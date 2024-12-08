using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SigmaSoftware.Infrastructure.Persistence;

public class DbContextInitializer
{
    private readonly ILogger<DbContextInitializer> _logger;
    private readonly Microsoft.EntityFrameworkCore.DbContext _context;

    public DbContextInitializer(ILogger<DbContextInitializer> logger, DbContext context, IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsNpgsql())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
    
    private static async Task TrySeedAsync()
    {
        
    }
}
