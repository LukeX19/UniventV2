namespace Univent.Infrastructure.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        private const string MessageTemplate = "A {0} with the id '{1}' already exists.";

        public EntityAlreadyExistsException()
            : base() { }

        public EntityAlreadyExistsException(string entityType, Guid entityId)
            : base(string.Format(MessageTemplate, entityType, entityId)) { }

        public EntityAlreadyExistsException(string entityType, Guid entityId, Exception innerException)
            : base(string.Format(MessageTemplate, entityType, entityId), innerException) { }
    }
}
