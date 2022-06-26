namespace Domain.Entities.Posts
{
    public class PostImage : ValueObject
    {
        public Guid PostId { get; private init; }

        public string Blob { get; private init; }

        public int Order { get; private init; }

        public PostImage(Guid postId, string blob, int order)
        {
            PostId = postId;
            Blob = blob;
            Order = order;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return PostId;
            yield return Blob;
        }
    }
}