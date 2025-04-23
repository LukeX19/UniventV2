using Univent.Domain.Models.Associations;

namespace Univent.App.Interfaces
{
    public interface IEventParticipantRepository
    {
        Task<(Guid EventId, Guid UserId)> CreateEventParticipantAsync(EventParticipant entity, CancellationToken ct = default);
        Task<ICollection<EventParticipant>> GetEventParticipantsByEventIdAsync(Guid eventId, CancellationToken ct = default);
        Task UpdateEventParticipantAsync(EventParticipant updatedEntity, CancellationToken ct = default);
        Task DeleteEventParticipantAsync(Guid eventId, Guid userId, CancellationToken ct = default);
    }
}
