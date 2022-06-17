namespace Application.Common.Models
{
    public record BlobData(string BlobName, string FileName, byte[] Content, long ContentLength, string ContentType);
}