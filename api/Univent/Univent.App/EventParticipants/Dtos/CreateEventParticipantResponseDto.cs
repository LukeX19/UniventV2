namespace Univent.App.EventParticipants.Dtos
{
    public class CreateEventParticipantResponseDto
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
    }
}
