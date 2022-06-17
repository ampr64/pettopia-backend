namespace Domain.Events
{
    public sealed class PostCreatedEvent : DomainEvent
    {
        public Post Post { get; }

        public PostCreatedEvent(Post post)
        {
            Post = post;
        }
    }
}