using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SigmaSoftware.Application.Common.Interfaces;
using SigmaSoftware.Domain.Entities;
using SigmaSoftware.Infrastructure.Common;
using SigmaSoftware.Infrastructure.Persistence.Interceptors;

namespace SigmaSoftware.Infrastructure.Persistence;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext

{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public DbContext(
        DbContextOptions<DbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<Candidate> Candidate => Set<Candidate>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
        builder.Entity<Candidate>().HasQueryFilter(e =>  e.IsDeleted == false);
    }
   
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);
        return await base.SaveChangesAsync(cancellationToken);
    }
    
    
}