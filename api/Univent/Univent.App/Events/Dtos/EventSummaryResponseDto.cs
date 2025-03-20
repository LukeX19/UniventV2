namespace Univent.App.Events.Dtos
{
    public class EventSummaryResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int EnrolledParticipants { get; set; }
        public int MaximumParticipants { get; set; }
        public DateTime StartTime { get; set; }
        public string LocationAddress { get; set; }
        public string PictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string TypeName { get; set; }
        public EventAuthorResponseDto Author { get; set; }
    }
}
