namespace Univent.App.Exceptions
{
    public class StatusConflictException : Exception
    {
        private const string MessageTemplate = "The {0} with id '{1}' is already {2}.";

        public StatusConflictException()
            : base() { }

        public StatusConflictException(string entityType, Guid entityId, string status)
            : base(string.Format(MessageTemplate, entityType, entityId, status)) { }

        public StatusConflictException(string entityType, Guid entityId, string status, Exception innerException)
            : base(string.Format(MessageTemplate, entityType, entityId, status), innerException) { }
    }
}
