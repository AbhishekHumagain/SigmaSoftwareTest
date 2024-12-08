using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SigmaSoftware.Application.Common.Interfaces;
using SigmaSoftware.Infrastructure.Common;
using SigmaSoftware.Infrastructure.Persistence.Interceptors;

namespace SigmaSoftware.Infrastructure.Persistence;

public class DbContext(
    DbContextOptions<DbContext> options,
    IMediator mediator,
    AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
    : Microsoft.EntityFrameworkCore.DbContext(options), IDbContext

{
    // public DbSet<UserInfo> UserInfos => Set<UserInfo>();
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
        // builder.Entity<UserInfo>().HasQueryFilter(e =>  e.IsDeleted == false);
    }
   
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await mediator.DispatchDomainEvents(this);
        return await base.SaveChangesAsync(cancellationToken);
    }
    
    
}