namespace Univent.Infrastructure.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        private const string MessageTemplate = "The {0} with id '{1}' already exists.";
        private const string MessageTemplateWithMultipleIds = "The {0} with ids '{1}' and '{2}' already exists.";

        public EntityAlreadyExistsException()
            : base() { }

        public EntityAlreadyExistsException(string entityType, Guid entityId)
            : base(string.Format(MessageTemplate, entityType, entityId)) { }

        public EntityAlreadyExistsException(string entityType, Guid entityId, Exception innerException)
            : base(string.Format(MessageTemplate, entityType, entityId), innerException) { }

        public EntityAlreadyExistsException(string entityType, Guid entityId1, Guid entityId2)
            : base(string.Format(MessageTemplateWithMultipleIds, entityType, entityId1, entityId2)) { }

        public EntityAlreadyExistsException(string entityType, Guid entityId1, Guid entityId2, Exception innerException)
            : base(string.Format(MessageTemplateWithMultipleIds, entityType, entityId1, entityId2), innerException) { }
    }
}
