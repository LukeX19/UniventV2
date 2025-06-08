namespace Univent.App.Exceptions
{
    public class FeedbackClosedException : Exception
    {
        private const string MessageTemplate = "Feedback for the event with id '{0}' is closed because the event did not start yet or has been cancelled.";

        public FeedbackClosedException()
            : base(MessageTemplate) { }

        public FeedbackClosedException(Guid eventId)
            : base(string.Format(MessageTemplate, eventId)) { }

        public FeedbackClosedException(Guid eventId, Exception innerException)
            : base(string.Format(MessageTemplate, eventId), innerException) { }
    }
}
