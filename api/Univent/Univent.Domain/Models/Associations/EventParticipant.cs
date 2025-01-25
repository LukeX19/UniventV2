using Univent.Domain.Models.Events;
using Univent.Domain.Models.Users;

namespace Univent.Domain.Models.Associations
{
    public class EventParticipant
    {
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public bool HasCompletedFeedback { get; set; }
    }
}
