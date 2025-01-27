using Univent.App.Interfaces;
using Univent.Domain.Models.Events;
using Univent.Infrastructure.Repositories.BasicRepositories;

namespace Univent.Infrastructure.Repositories
{
    public class EventTypeRepository : BaseRepositoryEF<EventType>, IEventTypeRepository
    {
        public EventTypeRepository(AppDbContext context) : base(context) { }
    }
}
