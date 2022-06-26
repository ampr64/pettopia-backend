namespace Application.Features.Posts.Queries.GetMyPosts
{
    public record ApplicationBriefDto
    {
        public Guid Id { get; set; }

        public string ApplicantId { get; set; } = null!;

        public string ApplicantName { get; set; } = null!;

        public string ApplicantEmail { get; set; } = null!;

        public DateTime SubmittedAt { get; set; }
    }
}