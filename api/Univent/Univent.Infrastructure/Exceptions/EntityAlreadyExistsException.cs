namespace Univent.Infrastructure.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        private const string MessageTemplate = "The {0} with id {1} already exists.";

        public EntityAlreadyExistsException()
            : base() { }

        public EntityAlreadyExistsException(string entityType, Guid entityId)
            : base(string.Format(MessageTemplate, entityType, entityId)) { }

        public EntityAlreadyExistsException(string entityType, Guid entityId, Exception innerException)
            : base(string.Format(MessageTemplate, entityType, entityId), innerException) { }
    }
}
