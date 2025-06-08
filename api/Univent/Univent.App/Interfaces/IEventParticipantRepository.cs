using Univent.Domain.Models.Associations;

namespace Univent.App.Interfaces
{
    public interface IEventParticipantRepository
    {
        Task<(Guid EventId, Guid UserId)> CreateEventParticipantAsync(EventParticipant entity, CancellationToken ct = default);
        Task<ICollection<EventParticipant>> GetEventParticipantsByEventIdAsync(Guid eventId, CancellationToken ct = default);
        Task<EventParticipant> GetEventParticipantByIdPairAsync(Guid eventId, Guid userId, CancellationToken ct = default);
        Task<Dictionary<Guid, bool>> GetFeedbackStatusesAsync(Guid userId, List<Guid> eventIds, CancellationToken ct = default);
        Task UpdateEventParticipantWithoutSavingAsync(EventParticipant updatedEntity, CancellationToken ct = default);
        Task DeleteEventParticipantAsync(Guid eventId, Guid userId, CancellationToken ct = default);
        Task<int> CountEventParticipantsByEventIdAsync(Guid eventId, CancellationToken ct = default);
    }
}
