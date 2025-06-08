namespace Univent.App.Exceptions
{
    public class FeedbackAlreadySubmittedException : Exception
    {
        private const string MessageTemplate = "User with id '{0}' has already submitted feedback for event with id '{1}'.";

        public FeedbackAlreadySubmittedException()
            : base(MessageTemplate) { }

        public FeedbackAlreadySubmittedException(Guid userId, Guid eventId)
            : base(string.Format(MessageTemplate, userId, eventId)) { }

        public FeedbackAlreadySubmittedException(Guid userId, Guid eventId, Exception innerException)
            : base(string.Format(MessageTemplate, userId, eventId), innerException) { }
    }
}
