using System.ComponentModel.DataAnnotations;

namespace Application.Common.Settings
{
    public class JwtSettings
    {
        public static string Section => "Jwt";

        [Required]
        public string AppSecret { get; set; }

        public bool RequireSignedTokens { get; set; } = true;

        public bool RequireExpirationTime { get; set; } = true;

        public bool ValidateAudience { get; set; } = false;

        public bool ValidateIssuer { get; set; } = true;

        public bool ValidateIssuerSigningKey { get; set; } = true;

        public bool ValidateLifetime { get; set; } = true;
    }
}