using System.Text.Json.Serialization;

namespace Application.Features.Users.Queries.GetUsers
{
    public record UserDto
    {
        public string Id { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Role { get; set; }

        public string Email { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? OrganizationName { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public BlobData? ProfilePicture { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public DateTime RegisteredAt { get; set; }
    }
}