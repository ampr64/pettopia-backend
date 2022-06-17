using System.ComponentModel.DataAnnotations;

namespace Application.Common.Settings
{
    public class SmtpSettings
    {
        public static readonly string Section = "Smtp";

        [Required]
        public string Host { get; set; } = null!;

        [Required]
        public int Port { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string From { get; set; } = null!;

        [Required]
        public bool EnableSsl { get; set; }

        [Required]
        public bool IsBodyHtml { get; set; }
    }
}