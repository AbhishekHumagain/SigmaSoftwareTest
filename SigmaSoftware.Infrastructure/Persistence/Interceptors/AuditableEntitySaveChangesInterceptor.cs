using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SigmaSoftware.Application.Common.Interfaces;
using SigmaSoftware.Domain.Common.Interfaces;

namespace SigmaSoftware.Infrastructure.Persistence.Interceptors;

public abstract class AuditableEntitySaveChangesInterceptor(IDateTime dateTime)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(Microsoft.EntityFrameworkCore.DbContext? context)
    {
        if (context == null) return;

        //Since we do not have session managed, this unique identifier will be used to insert in db in place of CreatorUserId, if there would be session we can use session data using different service
        var userInfoId = Guid.Parse("8D916776-2686-43C5-9AD1-4C7263E4120F"); 
        
        foreach (var entry in context.ChangeTracker.Entries<IBaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatorUserId = userInfoId;
                entry.Entity.CreationTime = dateTime.Now;
            }

            if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifierUser = userInfoId;
                entry.Entity.LastModificationTime = dateTime.Now;
            }

            if (entry.State != EntityState.Deleted) continue;
            entry.State = EntityState.Unchanged;
            entry.Entity.DeleterUserId = userInfoId;
            entry.Entity.DeletionTime = dateTime.Now;
            entry.Entity.IsDeleted = true;
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}