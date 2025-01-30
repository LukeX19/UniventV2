namespace Univent.Infrastructure.Exceptions
{
    public class AccountAlreadyExistsException : Exception
    {
        private const string MessageTemplate = "A user account is already associated with the email '{0}'.";

        public AccountAlreadyExistsException()
            : base() { }

        public AccountAlreadyExistsException(string email)
            : base(string.Format(MessageTemplate, email)) { }

        public AccountAlreadyExistsException(string email, Exception innerException)
            : base(string.Format(MessageTemplate, email), innerException) { }
    }
}
