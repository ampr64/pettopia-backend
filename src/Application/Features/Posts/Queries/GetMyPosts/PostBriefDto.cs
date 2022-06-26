using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public string BlobName { get; set; } = null!;

        public IReadOnlyList<ApplicationBriefDto> Applications { get; set; } = new List<ApplicationBriefDto>();
    }
}