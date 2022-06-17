using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IBlobService
    {
        Task<BlobData> GetBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default);

        Task<string> UploadBlobAsync(Stream content, string contentType, string containerName, string fileName, string? section = null, object? identifier = null, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(string containerName, string blobName, CancellationToken cancellation);
    }
}