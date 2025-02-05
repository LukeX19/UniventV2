namespace Univent.Infrastructure.Exceptions
{
    public class InvalidFileFormatException : Exception
    {
        private const string MessageTemplate = "The provided file extension is not supported. Please use '.png', '.jpg' or '.jpeg' files instead.";

        public InvalidFileFormatException()
            : base(string.Format(MessageTemplate)) { }

        public InvalidFileFormatException(Exception innerException)
            : base(string.Format(MessageTemplate), innerException) { }
    }
}
