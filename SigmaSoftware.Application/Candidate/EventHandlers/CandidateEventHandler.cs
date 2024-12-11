using MediatR;
using Microsoft.Extensions.Logging;
using SigmaSoftware.Domain.Events.CandidateEvents;

namespace SigmaSoftware.Application.Candidate.EventHandlers
{
    public class CandidateEventHandler(ILogger<CandidateEventHandler> logger)
        : INotificationHandler<CandidateCreatedEvent>
    {
        public Task Handle(CandidateCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Candidate Domain Event: {DomainEvent}", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
