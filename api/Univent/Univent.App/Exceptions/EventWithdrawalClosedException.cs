namespace Univent.App.Exceptions
{
    public class EventWithdrawalClosedException : Exception
    {
        private const string MessageTemplate = "Leaving the event with id '{0}' is no longer allowed because the event is about to start.";

        public EventWithdrawalClosedException()
            : base(MessageTemplate) { }

        public EventWithdrawalClosedException(Guid eventId)
            : base(string.Format(MessageTemplate, eventId)) { }

        public EventWithdrawalClosedException(Guid eventId, Exception innerException)
            : base(string.Format(MessageTemplate, eventId), innerException) { }
    }
}
