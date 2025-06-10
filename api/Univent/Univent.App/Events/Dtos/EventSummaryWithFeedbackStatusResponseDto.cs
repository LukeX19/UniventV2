namespace Univent.App.Events.Dtos
{
    public class EventSummaryWithFeedbackStatusResponseDto
    {
        public EventSummaryResponseDto Event { get; set; }
        public bool HasLoggedUserProvidedFeedback { get; set; }
    }
}
