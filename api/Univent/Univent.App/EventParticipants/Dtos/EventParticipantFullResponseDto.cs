namespace Univent.App.EventParticipants.Dtos
{
    public class EventParticipantFullResponseDto
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PictureUrl { get; set; }
        public double Rating { get; set; }
        public bool HasCompletedFeedback { get; set; }
    }
}
