namespace Univent.App.Exceptions
{
    public class StatusConflictException : Exception
    {
        private const string MessageTemplate = "The {0} with id '{1}' is already cancelled.";

        public StatusConflictException()
            : base() { }

        public StatusConflictException(string entityType, Guid entityId)
            : base(string.Format(MessageTemplate, entityType, entityId)) { }

        public StatusConflictException(string entityType, Guid entityId, Exception innerException)
            : base(string.Format(MessageTemplate, entityType, entityId), innerException) { }
    }
}
