namespace Domain.Entities
{
    public class PostImage
    {
        public Guid PostId { get; set; }

        public string Blob { get; set; } = null!;

        public int Order { get; set; }
    }
}