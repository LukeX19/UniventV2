namespace Univent.Infrastructure.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private const string MessageTemplate = "The {0} with id '{1}' was not found.";
        private const string MessageTemplateWithMultipleIds = "The {0} with ids '{1}' and '{2}' was not found.";

        public EntityNotFoundException()
            : base() { }

        public EntityNotFoundException(string entityType, Guid entityId)
            : base(string.Format(MessageTemplate, entityType, entityId)) { }

        public EntityNotFoundException(string entityType, Guid entityId, Exception innerException)
            : base(string.Format(MessageTemplate, entityType, entityId), innerException) { }

        public EntityNotFoundException(string entityType, Guid entityId1, Guid entityId2)
            : base(string.Format(MessageTemplateWithMultipleIds, entityType, entityId1, entityId2)) { }

        public EntityNotFoundException(string entityType, Guid entityId1, Guid entityId2, Exception innerException)
            : base(string.Format(MessageTemplateWithMultipleIds, entityType, entityId1, entityId2), innerException) { }
    }
}
