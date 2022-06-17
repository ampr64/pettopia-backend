using Application.Common.Interfaces;
using Application.Common.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Text;

namespace Infrastructure.Services
{
    public class AzureBlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<BlobData> GetBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var downloadResult = await blobClient.DownloadContentAsync(cancellationToken);

            return new BlobData(
                blobClient.Name,
                GetFileName(blobName),
                downloadResult.Value.Content.ToArray(),
                downloadResult.Value.Details.ContentLength,
                downloadResult.Value.Details.ContentType);
        }

        public async Task<string> UploadBlobAsync(Stream content,
            string contentType,
            string containerName,
            string fileName,
            string? section = null,
            object? identifier = null,
            CancellationToken cancellationToken = default)
        {
            var blobName = GenerateBlobName(section, identifier, fileName);

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };

            await blobClient.UploadAsync(content, new BlobUploadOptions { HttpHeaders = blobHttpHeader }, cancellationToken);

            return blobClient.Name;
        }

        public async Task<bool> DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var response = await blobClient.DeleteAsync(cancellationToken: cancellationToken);
            
            return !response.IsError;
        }

        private static string GenerateBlobName(string? section, object? identifier, string fileName)
        {
            return GetPath(section, identifier) + fileName;
        }

        private static string GetPath(string? section, object? identifier)
        {
            return new StringBuilder(section)
                .Append(identifier is null ? string.Empty : $"{identifier}/")
                .ToString();
        }

        private static string GetFileName(string blob)
        {
            return blob.Split('/') switch
            {
                { Length: 2 } path => path[1],
                { Length: 3 } path => path[2],
                _ => blob
            };
        }
    }
}