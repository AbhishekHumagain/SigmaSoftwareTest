using MediatR;
using Microsoft.EntityFrameworkCore;
using SigmaSoftware.Domain.Common;

namespace SigmaSoftware.Infrastructure.Common;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IMediator mediator, DbContext context) 
    {
        var entities = context.ChangeTracker
            .Entries<Events>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity);

        var eventsEnumerable = entities as Events[] ?? entities.ToArray();
        var domainEvents = eventsEnumerable
            .SelectMany(e => e.DomainEvents)
            .ToList();

        eventsEnumerable.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}
