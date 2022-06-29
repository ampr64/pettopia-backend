using System.Text.Json.Serialization;

namespace Application.Features.Posts.Queries.GetMyPosts
{
    public record PostBriefDto
    {
        public Guid Id { get; set; }

        public string PetName { get; set; }

        public int PostType { get; set; }

        public int PetGender { get; set; }

        public DateTime CreatedAt { get; set; }

        public BlobData Thumbnail { get; set; } = null!;

        [JsonIgnore]
        public string BlobName { get; set; } = null!;

        public IReadOnlyList<ApplicationBriefDto> Applications { get; set; } = new List<ApplicationBriefDto>();
    }
}