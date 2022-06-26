namespace Application.Features.Posts.Queries.GetPosts
{
    public record PostPreviewDto
    {
        public Guid Id { get; set; }

        public string PetName { get; set; } = null!;

        public int PetGender { get; set; }

        public BlobData Thumbnail { get; set; } = null!;

        public string AuthorId { get; set; } = null!;

        public string AuthorName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}