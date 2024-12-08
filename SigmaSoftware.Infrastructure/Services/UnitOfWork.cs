using MediatR;
using Microsoft.EntityFrameworkCore;
using SigmaSoftware.Application.Common.Interfaces;
using DbContext = SigmaSoftware.Infrastructure.Persistence.DbContext;

namespace SigmaSoftware.Infrastructure.Services;

public sealed class UnitOfWork(DbContext dbContext, IMediator mediator) : IUnitOfWork, IDisposable
{
    private bool _disposed = false;
    private readonly IMediator _mediator = mediator;

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            Rollback();
            throw;
        }
    }
    public void Rollback()
    {
        foreach (var entry in dbContext.ChangeTracker.Entries())
        {
            switch (entry.State)  
            {  
                case EntityState.Modified:  
                    entry.State = EntityState.Unchanged;  
                    break;  
                case EntityState.Added:  
                    entry.State = EntityState.Detached;  
                    break;  
                case EntityState.Deleted:  
                    entry.Reload();  
                    break;  
                default: break;  
            }  
        }
    }
    public IGenericRepository<T> GenericRepository<T>() where T : class
    {
        return new GenericRepository<T>(dbContext);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                dbContext.DisposeAsync();
            }
        }
        _disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


}