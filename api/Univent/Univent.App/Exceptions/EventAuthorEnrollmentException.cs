namespace Univent.App.Exceptions
{
    public class EventAuthorEnrollmentException : Exception
    {
        private const string MessageTemplate = "Event author cannot enroll as a participant.";

        public EventAuthorEnrollmentException()
            : base(MessageTemplate) { }

        public EventAuthorEnrollmentException(Exception innerException)
            : base(MessageTemplate, innerException) { }
    }
}
