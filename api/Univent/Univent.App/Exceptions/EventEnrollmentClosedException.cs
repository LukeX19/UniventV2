namespace Univent.App.Exceptions
{
    public class EventEnrollmentClosedException : Exception
    {
        private const string MessageTemplate = "Enrollment for the event with id '{0}' is closed. You can no longer join this event.";

        public EventEnrollmentClosedException()
            : base(MessageTemplate) { }

        public EventEnrollmentClosedException(Guid eventId)
            : base(string.Format(MessageTemplate, eventId)) { }

        public EventEnrollmentClosedException(Guid eventId, Exception innerException)
            : base(string.Format(MessageTemplate, eventId), innerException) { }
    }
}
