namespace Univent.Infrastructure.Exceptions
{
    public class InvalidFileFormatException : Exception
    {
        private const string MessageTemplate = "Unsupported file type: {0}";

        public InvalidFileFormatException(string contentType)
            : base(string.Format(MessageTemplate, contentType)) { }

        public InvalidFileFormatException(string contentType, Exception innerException)
            : base(string.Format(MessageTemplate, contentType), innerException) { }
    }
}
