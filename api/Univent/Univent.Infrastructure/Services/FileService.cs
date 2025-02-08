using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using System.Net;
using Univent.App.Interfaces;
using Univent.Infrastructure.Exceptions;
using Univent.Infrastructure.Options;

namespace Univent.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly AWSS3StorageSettings _settings;
        private readonly AmazonS3Client _s3Client;
        private readonly HashSet<string> _allowedMimeTypes = new()
        {
            "image/png", "image/jpeg", "image/jpg", "image/gif", "image/webp", "image/bmp", "image/tiff", "image/svg+xml"
        };

        public FileService(IOptions<AWSS3StorageSettings> settings)
        {
            _settings = settings.Value;
            _s3Client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, RegionEndpoint.GetBySystemName(_settings.Region));
        }

        public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default)
        {
            // Ensure file's MIME type is a valid image type
            if (string.IsNullOrEmpty(contentType) || !_allowedMimeTypes.Contains(contentType.ToLower()))
            {
                throw new InvalidFileFormatException(contentType);
            }

            var fileExtension = Path.GetExtension(fileName).TrimStart('.').ToLower();

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = _settings.BucketName,
                ContentType = contentType
            };

            using var transferUtility = new TransferUtility(_s3Client);
            await transferUtility.UploadAsync(uploadRequest, ct);

            return $"{_settings.S3BaseUrl}/{fileName}";
        }

        public async Task DeleteAsync(string fileUrl, CancellationToken ct = default)
        {
            try
            {
                await _s3Client.GetObjectMetadataAsync(_settings.BucketName, fileUrl, ct);
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new AWSFileNotFoundException(fileUrl);
            }

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = fileUrl
            };

            await _s3Client.DeleteObjectAsync(deleteRequest, ct);
        }
    }
}
