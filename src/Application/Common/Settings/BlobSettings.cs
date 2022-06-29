using System.ComponentModel.DataAnnotations;

namespace Application.Common.Settings
{
    public class BlobSettings
    {
        public static readonly string Section = "AzureBlobStorage";

        [Required]
        public string ConnectionString { get; set; } = null!;

        [Required]
        public string Container { get; set; } = null!;

        [Required]
        public string PostsSection { get; set; } = null!;

        [Required]
        public string UsersSection { get; set; } = null!;
    }
}