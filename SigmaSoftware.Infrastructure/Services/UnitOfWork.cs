using MediatR;
using Microsoft.EntityFrameworkCore;
using SigmaSoftware.Application.Common.Interfaces;
using SigmaSoftware.Infrastructure.Persistence;

namespace SigmaSoftware.Infrastructure.Services;

public sealed class UnitOfWork(SigmaSigmaDbContext sigmaSigmaDbContext, IMediator mediator) : IUnitOfWork, IDisposable
{
    private bool _disposed;
    private readonly IMediator _mediator = mediator;

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await sigmaSigmaDbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            Rollback();
            throw;
        }
    }
    public void Rollback()
    {
        foreach (var entry in sigmaSigmaDbContext.ChangeTracker.Entries())
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
            }  
        }
    }
    public IGenericRepository<T> GenericRepository<T>() where T : class
    {
        return new GenericRepository<T>(sigmaSigmaDbContext);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                sigmaSigmaDbContext.DisposeAsync();
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