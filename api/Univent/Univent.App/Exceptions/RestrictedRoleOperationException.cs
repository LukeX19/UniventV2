namespace Univent.App.Exceptions
{
    public class RestrictedRoleOperationException : Exception
    {
        private const string MessageTemplate = "Operation not allowed. User with id '{0}' has a restricted role.";

        public RestrictedRoleOperationException()
            : base() { }

        public RestrictedRoleOperationException(Guid userId)
            : base(string.Format(MessageTemplate, userId)) { }

        public RestrictedRoleOperationException(Guid userId, Exception innerException)
            : base(string.Format(MessageTemplate, userId), innerException) { }
    }
}
