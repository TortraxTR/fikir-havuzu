namespace api.Services.Storage
{
    public interface IFileStorageService
    {
        // Uploads content under a new, service-generated key and returns that key.
        Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken = default);

        Task<Stream> DownloadAsync(string storageKey, CancellationToken cancellationToken = default);

        Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default);
    }
}
