namespace Application.Features.Posts.Queries.GetPostDetail
{
    public record PostDetailDto
    {
        public Guid Id { get; set; }

        public string PetName { get; set; } = string.Empty;

        public int PetGender { get; set; }

        public int NeuterStatus { get; set; }

        public string Description { get; set; } = string.Empty;

        public int PetAge { get; set; }

        public int PetSpecies { get; set; }

        public int PostType { get; set; }

        public string AuthorId { get; set; } = string.Empty;

        public string AuthorName { get; set; } = string.Empty;

        public IEnumerable<BlobData> Images { get; set; } = Enumerable.Empty<BlobData>();
    }
}