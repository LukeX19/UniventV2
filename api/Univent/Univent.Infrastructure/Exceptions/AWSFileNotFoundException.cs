namespace Univent.Infrastructure.Exceptions
{
    public class AWSFileNotFoundException : Exception
    {
        private const string MessageTemplate = "The File '{0}' was not found in AWS bucket.";

        public AWSFileNotFoundException()
            : base() { }

        public AWSFileNotFoundException(string blobName)
            : base(string.Format(MessageTemplate, blobName)) { }

        public AWSFileNotFoundException(string blobName, Exception innerException)
            : base(string.Format(MessageTemplate, blobName), innerException) { }
    }
}
