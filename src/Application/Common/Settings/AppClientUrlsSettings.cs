using System.ComponentModel.DataAnnotations;

namespace Application.Common.Settings
{
    public class AppClientUrlsSettings
    {
        public static string Section => "AppClientUrls";

        [Required]
        public string BaseUrl { get; set; }

        [Required]
        public string EmailConfirmationCallbackUri { get; set; }

        [Required]
        public string PostDetailUri { get; set; }
    }
}