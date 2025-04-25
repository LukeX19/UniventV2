namespace Univent.App.Exceptions
{
    public class EventMaximumParticipantsReachedException : Exception
    {
        private const string MessageTemplate = "The event is full. No more participants can enroll!";

        public EventMaximumParticipantsReachedException()
            : base(MessageTemplate) { }

        public EventMaximumParticipantsReachedException(Exception innerException)
            : base(MessageTemplate, innerException) { }
    }
}
