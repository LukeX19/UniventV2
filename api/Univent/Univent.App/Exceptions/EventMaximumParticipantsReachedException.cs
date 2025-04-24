namespace Univent.App.Exceptions
{
    public class EventMaximumParticipantsReachedException : Exception
    {
        private const string MessageTemplate = "The event with id {0} is full. No more participants can enroll!";

        public EventMaximumParticipantsReachedException()
            : base() { }

        public EventMaximumParticipantsReachedException(Guid eventId)
            : base(string.Format(MessageTemplate, eventId)) { }

        public EventMaximumParticipantsReachedException(Guid eventId, Exception innerException)
            : base(string.Format(MessageTemplate, eventId), innerException) { }
    }
}
