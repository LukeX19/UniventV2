namespace Univent.Infrastructure.Exceptions
{
    public class DeleteFailedException : Exception
    {
        private const string MessageTemplate = "Failed to delete {0} with id '{1}'.";

        public DeleteFailedException()
            : base(MessageTemplate) { }
        public DeleteFailedException(string entityType, Guid entityId)
            : base(string.Format(MessageTemplate, entityType, entityId)) { }

        public DeleteFailedException(string entityType, Guid entityId, Exception innerException)
            : base(string.Format(MessageTemplate, entityType, entityId), innerException) { }
    }
}
