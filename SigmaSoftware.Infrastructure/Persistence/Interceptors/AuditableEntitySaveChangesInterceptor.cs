using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SigmaSoftware.Application.Common.Interfaces;
using SigmaSoftware.Domain.Common.Interfaces;

namespace SigmaSoftware.Infrastructure.Persistence.Interceptors;

public class AuditableEntitySaveChangesInterceptor(
    ICurrentUserService currentUserService,
    IDateTime dateTime)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;
        Guid userId = default;
        if (currentUserService.UserId != null)
        {
            userId = new Guid(currentUserService.UserId);
        }

        foreach (var entry in context.ChangeTracker.Entries<IBaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatorUserId = userId;
                entry.Entity.CreationTime = dateTime.Now;
            } 

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifierUser = userId;
                entry.Entity.LastModificationTime = dateTime.Now;
            }
            
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Unchanged;
                entry.Entity.DeleterUserId = userId;
                entry.Entity.DeletionTime = dateTime.Now;
                entry.Entity.IsDeleted = true;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
