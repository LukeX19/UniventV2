namespace Univent.Infrastructure.Options
{
    public class AWSS3StorageSettings
    {
        public string BucketName { get; set; }
        public string Region { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string S3BaseUrl { get; set; }
    }
}
