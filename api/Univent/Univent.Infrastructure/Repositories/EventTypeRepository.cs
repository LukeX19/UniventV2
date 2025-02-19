using Microsoft.EntityFrameworkCore;
using Univent.App.Interfaces;
using Univent.Domain.Models.Events;
using Univent.Infrastructure.Repositories.BasicRepositories;

namespace Univent.Infrastructure.Repositories
{
    public class EventTypeRepository : BaseRepositoryEF<EventType>, IEventTypeRepository
    {
        public EventTypeRepository(AppDbContext context) : base(context) { }

        public async Task<EventType?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            return await _context.EventTypes.FirstOrDefaultAsync(et => et.Name.ToLower() == name.ToLower(), ct);
        }
    }
}
