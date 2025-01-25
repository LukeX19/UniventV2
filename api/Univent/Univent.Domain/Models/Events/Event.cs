using Univent.Domain.Models.BasicEntities;
using Univent.Domain.Models.Users;

namespace Univent.Domain.Models.Events
{
    public class Event : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaximumParticipants { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double LocationLat { get; set; }
        public double LocationLong { get; set; }
        public string PictureUrl { get; set; }
        public bool IsCancelled { get; set; }
        public EventType Type { get; set; }
        public Guid AuthorId { get; set; }
        public AppUser Author { get; set; }
        public ICollection<AppUser> Participants { get; set; }
    }
}
