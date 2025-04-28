using Univent.App.Pagination.Dtos;
using Univent.Domain.Models.Events;

namespace Univent.App.Interfaces
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task<PaginationResponseDto<Event>> GetAvailableEventsSummariesAsync(PaginationRequestDto pagination,
            string? search = null, ICollection<Guid>? types = null, CancellationToken ct = default);
        Task<PaginationResponseDto<Event>> GetCreatedEventsSummariesByUserIdAsync(Guid userId, PaginationRequestDto pagination, CancellationToken ct = default);
        Task<int> GetCreatedEventsCountByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<PaginationResponseDto<Event>> GetParticipatedEventsSummariesByUserIdAsync(Guid userId, PaginationRequestDto pagination, CancellationToken ct = default);
        Task<int> GetParticipatedEventsCountByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<Event> GetEventByIdAsync(Guid id, CancellationToken ct = default);
        Task<Dictionary<Guid, int>> GetEventParticipantsCountAsync(ICollection<Guid> eventIds, CancellationToken ct = default);
    }
}
