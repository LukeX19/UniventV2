namespace Univent.Infrastructure.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private const string MessageTemplate = "The {0} with the id '{1}' was not found.";

        public EntityNotFoundException()
            : base() { }

        public EntityNotFoundException(string entityType, Guid entityId)
            : base(string.Format(MessageTemplate, entityType, entityId)) { }

        public EntityNotFoundException(string entityType, Guid entityId, Exception innerException)
            : base(string.Format(MessageTemplate, entityType, entityId), innerException) { }
    }
}
