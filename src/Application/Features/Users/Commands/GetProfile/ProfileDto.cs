using System.Text.Json.Serialization;

namespace Application.Features.Users.Commands.GetProfile
{
    public record ProfileDto
    {
        public string Id { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? OrganizationName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? AboutMe { get; set; }

        public DateTime RegisteredAt { get; set; }

        public string? AddressProvince { get; set; }

        public string? AddressCity { get; set; }

        public BlobData? ProfilePicture { get; set; }

        public IReadOnlyList<BlobData> Pictures { get; set; } = new List<BlobData>();

        public string? InstagramProfileUrl { get; set; }

        public string? FacebookProfileUrl { get; set; }
    }
}