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
        private readonly List<string> _supportedFormats = new() { "png", "jpg", "jpeg" };

        public FileService(IOptions<AWSS3StorageSettings> settings)
        {
            _settings = settings.Value;
            _s3Client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, RegionEndpoint.GetBySystemName(_settings.Region));
        }

        public async Task<string> UploadAsync(Stream stream, string fileName, CancellationToken ct = default)
        {
            var fileExtension = Path.GetExtension(fileName).TrimStart('.').ToLower();
            if (!_supportedFormats.Contains(fileExtension))
            {
                throw new InvalidFileFormatException();
            }

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = _settings.BucketName,
                ContentType = $"image/{fileExtension}"
            };

            using var transferUtility = new TransferUtility(_s3Client);
            await transferUtility.UploadAsync(uploadRequest, ct);

            return GetFileUrl(fileName);
        }

        public async Task DeleteAsync(string fileName, CancellationToken ct = default)
        {
            try
            {
                await _s3Client.GetObjectMetadataAsync(_settings.BucketName, fileName, ct);
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new AWSFileNotFoundException(fileName);
            }

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = fileName
            };

            await _s3Client.DeleteObjectAsync(deleteRequest, ct);
        }

        private string GetFileUrl(string fileName)
        {
            return $"{_settings.S3BaseUrl}/{fileName}";
        }
    }
}
