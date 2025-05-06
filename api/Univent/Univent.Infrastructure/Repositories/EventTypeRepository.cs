using Microsoft.EntityFrameworkCore;
using Univent.App.Interfaces;
using Univent.App.Pagination.Dtos;
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

        public async Task<ICollection<EventType>> GetAllActiveAsync(CancellationToken ct = default)
        {
            return await _context.EventTypes
                .Where(et => !et.IsDeleted)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<PaginationResponseDto<EventType>> GetAllAsync(PaginationRequestDto pagination, CancellationToken ct = default)
        {
            var query = _context.EventTypes
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(et => et.Name)
                .AsQueryable();

            int totalEventTypes = await query.CountAsync(ct);

            var eventTypes = await query
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(ct);

            int totalPages = (int)Math.Ceiling((double)totalEventTypes / pagination.PageSize);

            return new PaginationResponseDto<EventType>(eventTypes, pagination.PageIndex, totalPages, totalEventTypes);
        }
    }
}
