namespace Univent.Infrastructure.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        private const string MessageTemplate = "The provided credentials are not valid.";

        public InvalidCredentialsException()
            : base(MessageTemplate) { }

        public InvalidCredentialsException(Exception innerException)
            : base(MessageTemplate, innerException) { }
    }
}
