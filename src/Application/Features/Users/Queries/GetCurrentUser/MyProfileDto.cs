using System.Text.Json.Serialization;

namespace Application.Features.Users.Queries.GetCurrentUser
{
    public record MyProfileDto
    {
        public string Id { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? OrganizationName { get; set; }

        public BlobData? ProfilePicture { get; set; }

        public string? AboutMe { get; set; }

        public string? AddressProvince { get; set; }

        public string? AddressCity { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? AddressZipCode { get; set; }

        public IReadOnlyList<BlobData> Pictures { get; set; } = new List<BlobData>();

        public string? FacebookProfileUrl { get; set; }

        public string? InstagramProfileUrl { get; set; }

        public string? PhonePrefix { get; set; }

        public string? PhoneNumber { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string>? AdoptionRequirements { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegisteredAt { get; set; }
    }
}