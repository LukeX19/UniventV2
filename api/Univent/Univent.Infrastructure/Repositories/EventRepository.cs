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

        public async Task<PaginationResponseDto<Event>> GetAllEventsSummariesAsync(PaginationRequestDto pagination, CancellationToken ct = default)
        {
            var query = _context.Events
                .AsNoTracking()
                .AsSplitQuery()
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
    }
}
