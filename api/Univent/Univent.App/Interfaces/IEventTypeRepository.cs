using Univent.Domain.Models.Events;

namespace Univent.App.Interfaces
{
    public interface IEventTypeRepository : IBaseRepository<EventType>
    {
        Task<EventType?> GetByNameAsync(string name, CancellationToken ct = default);
        Task<ICollection<EventType>> GetAllActiveAsync(CancellationToken ct = default);
    }
}
