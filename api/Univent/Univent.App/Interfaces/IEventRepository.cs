using Univent.Domain.Models.Events;

namespace Univent.App.Interfaces
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task<ICollection<Event>> GetAllEventsSummariesAsync(CancellationToken ct = default);
        Task<Dictionary<Guid, int>> GetEventParticipantsCountAsync(IEnumerable<Guid> eventIds, CancellationToken ct = default);
    }
}
