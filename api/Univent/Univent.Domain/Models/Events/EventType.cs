using Univent.Domain.Models.BasicEntities;

namespace Univent.Domain.Models.Events
{
    public class EventType : BaseEntity
    {
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
