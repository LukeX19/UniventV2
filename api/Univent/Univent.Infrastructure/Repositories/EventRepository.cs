using Univent.App.Interfaces;
using Univent.Domain.Models.Events;
using Univent.Infrastructure.Repositories.BasicRepositories;

namespace Univent.Infrastructure.Repositories
{
    public class EventRepository : BaseRepositoryEF<Event>, IEventRepository
    {
        public EventRepository(AppDbContext context) : base(context) { }
    }
}
