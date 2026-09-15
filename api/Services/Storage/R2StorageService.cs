using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace api.Services.Storage
{
    // Cloudflare R2 is S3-compatible, so this talks to it through the AWS S3 SDK
    // pointed at the account's R2 endpoint instead of AWS.
    public sealed class R2StorageService : IFileStorageService
    {
        private readonly IAmazonS3 _client;
        private readonly string _bucketName;

        public R2StorageService(IAmazonS3 client, IOptions<R2Options> options)
        {
            _client = client;
            _bucketName = options.Value.BucketName;
        }

        public async Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken = default)
        {
            var key = $"proposal-files/{Guid.NewGuid()}/{fileName}";

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = content,
                ContentType = contentType,
                AutoCloseStream = false,
                // R2 doesn't implement the SDK's chunked/signed streaming payload upload
                // (fails with "STREAMING-AWS4-HMAC-SHA256-PAYLOAD not implemented"); fall
                // back to an unsigned payload, which R2 does support.
                DisablePayloadSigning = true
            };

            await _client.PutObjectAsync(request, cancellationToken);
            return key;
        }

        public async Task<Stream> DownloadAsync(string storageKey, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetObjectAsync(_bucketName, storageKey, cancellationToken);
            return response.ResponseStream;
        }

        public async Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
        {
            await _client.DeleteObjectAsync(_bucketName, storageKey, cancellationToken);
        }
    }
}
