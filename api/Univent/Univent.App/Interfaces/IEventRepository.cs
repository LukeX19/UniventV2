using Univent.App.Pagination.Dtos;
using Univent.Domain.Models.Events;

namespace Univent.App.Interfaces
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task<PaginationResponseDto<Event>> GetAllEventsSummariesAsync(PaginationRequestDto pagination, CancellationToken ct = default);
        Task<Event> GetEventByIdAsync(Guid id, CancellationToken ct = default);
        Task<Dictionary<Guid, int>> GetEventParticipantsCountAsync(ICollection<Guid> eventIds, CancellationToken ct = default);
    }
}
