using Microsoft.EntityFrameworkCore;
using Univent.App.Interfaces;
using Univent.App.Pagination.Dtos;
using Univent.Domain.Models.Events;
using Univent.Infrastructure.Exceptions;
using Univent.Infrastructure.Repositories.BasicRepositories;

namespace Univent.Infrastructure.Repositories
{
    public class EventRepository : BaseRepositoryEF<Event>, IEventRepository
    {
        public EventRepository(AppDbContext context) : base(context) { }

        public async Task<PaginationResponseDto<Event>> GetAvailableEventsSummariesAsync(PaginationRequestDto pagination,
            string? search = null, ICollection<Guid>? types = null, CancellationToken ct = default)
        {
            var query = _context.Events
                .AsNoTracking()
                .AsSplitQuery()
                .Where(e => e.StartTime > DateTime.UtcNow)
                .Where(e => e.IsCancelled == false)
                .Include(e => e.Author)
                .Include(e => e.Type)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                query = query.Where(e =>
                    e.Name.ToLower().Contains(lowerSearch) ||
                    e.Description.ToLower().Contains(lowerSearch));
            }

            if (types != null && types.Count > 0)
            {
                query = query.Where(e => types.Contains(e.TypeId.Value));
            }

            query = query.OrderByDescending(e => e.CreatedAt);

            int totalEvents = await query.CountAsync(ct);

            var events = await query
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(ct);

            int totalPages = (int)Math.Ceiling((double)totalEvents / pagination.PageSize);

            return new PaginationResponseDto<Event>(events, pagination.PageIndex, totalPages, totalEvents);
        }

        public async Task<PaginationResponseDto<Event>> GetCreatedEventsSummariesByUserIdAsync(Guid userId, PaginationRequestDto pagination, CancellationToken ct = default)
        {
            var query = _context.Events
                .AsNoTracking()
                .AsSplitQuery()
                .Where(e => e.AuthorId == userId)
                .Include(e => e.Author)
                .Include(e => e.Type)
                .OrderByDescending(e => e.CreatedAt);

            int totalEvents = await query.CountAsync(ct);

            var events = await query
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(ct);

            int totalPages = (int)Math.Ceiling((double)totalEvents / pagination.PageSize);

            return new PaginationResponseDto<Event>(events, pagination.PageIndex, totalPages, totalEvents);
        }

        public async Task<int> GetCreatedEventsCountByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Events
                .AsNoTracking()
                .AsSplitQuery()
                .Where(e => e.AuthorId == userId)
                .CountAsync(ct);
        }

        public async Task<PaginationResponseDto<Event>> GetParticipatedEventsSummariesByUserIdAsync(Guid userId, PaginationRequestDto pagination, CancellationToken ct = default)
        {
            var query = _context.Events
                .AsNoTracking()
                .AsSplitQuery()
                .Where(e => e.Participants.Any(ep => ep.UserId == userId))
                .Include(e => e.Author)
                .Include(e => e.Type)
                .OrderByDescending(e => e.CreatedAt);

            int totalEvents = await query.CountAsync(ct);

            var events = await query
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(ct);

            int totalPages = (int)Math.Ceiling((double)totalEvents / pagination.PageSize);

            return new PaginationResponseDto<Event>(events, pagination.PageIndex, totalPages, totalEvents);
        }

        public async Task<int> GetParticipatedEventsCountByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Events
                .AsNoTracking()
                .AsSplitQuery()
                .Where(e => e.Participants.Any(ep => ep.UserId == userId))
                .CountAsync(ct);
        }

        public async Task<Event> GetEventByIdAsync(Guid id, CancellationToken ct = default)
        {
            var eventEntity = await _context.Events
                .AsNoTracking()
                .AsSplitQuery()
                .Include(e => e.Author)
                .Include(e => e.Type)
                .FirstOrDefaultAsync(e => e.Id == id, ct);

            return eventEntity ?? throw new EntityNotFoundException(nameof(Event), id);
        }

        public async Task<Dictionary<Guid, int>> GetEventParticipantsCountAsync(ICollection<Guid> eventIds, CancellationToken ct = default)
        {
            return await _context.EventParticipants
                .Where(ep => eventIds.Contains(ep.EventId))
                .GroupBy(ep => ep.EventId)
                .Select(g => new { EventId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.EventId, x => x.Count, ct);
        }

        public async Task<ICollection<Event>> GetAllPotentialEventsAsync(Guid userId, CancellationToken ct = default)
        {
            var currentTime = DateTime.UtcNow;

            var userEnrolledEventIds = await _context.EventParticipants
                .Where(ep => ep.UserId == userId)
                .Select(ep => ep.EventId)
                .ToListAsync(ct);

            var events = await _context.Events
                .AsNoTracking()
                .Include(e => e.Type)
                .Where(e =>
                    !e.IsCancelled &&
                    e.StartTime > currentTime.AddHours(2) &&
                    e.AuthorId != userId &&
                    !userEnrolledEventIds.Contains(e.Id))
                .ToListAsync(ct);

            var participantCounts = await _context.EventParticipants
                .Where(ep => events.Select(e => e.Id).Contains(ep.EventId))
                .GroupBy(ep => ep.EventId)
                .Select(g => new { EventId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.EventId, g => g.Count, ct);

            return events
                .Where(e => !participantCounts.ContainsKey(e.Id) || participantCounts[e.Id] < e.MaximumParticipants)
                .OrderBy(e => e.StartTime)
                .ToList();
        }
    }
}
