using Microsoft.EntityFrameworkCore;
using Univent.App.Interfaces;
using Univent.Domain.Models.Events;
using Univent.Infrastructure.Repositories.BasicRepositories;

namespace Univent.Infrastructure.Repositories
{
    public class EventRepository : BaseRepositoryEF<Event>, IEventRepository
    {
        public EventRepository(AppDbContext context) : base(context) { }

        public async Task<ICollection<Event>> GetAllEventsSummariesAsync(CancellationToken ct = default)
        {
            return await _context.Events
                .AsNoTracking()
                .AsSplitQuery()
                .Include(e => e.Author)
                .Include(e => e.Type)
                .ToListAsync(ct);
        }

        public async Task<Dictionary<Guid, int>> GetEventParticipantsCountAsync(IEnumerable<Guid> eventIds, CancellationToken ct = default)
        {
            return await _context.EventParticipants
                .Where(ep => eventIds.Contains(ep.EventId))
                .GroupBy(ep => ep.EventId)
                .Select(g => new { EventId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.EventId, x => x.Count, ct);
        }
    }
}
