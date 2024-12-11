using SigmaSoftware.Domain.Common;

namespace SigmaSoftware.Domain.Events.CandidateEvents
{
    public class CandidateCreatedEvent(string email) : BaseEvent
    {
        private string _email = email;
    }
    
    public class CandidateUpdatedEvent(string email) : BaseEvent
    {
        private string _email = email;
    }
}
