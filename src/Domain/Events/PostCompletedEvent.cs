using Domain.Entities.Posts;

namespace Domain.Events
{
    public sealed class PostCompletedEvent : DomainEvent
    {
        public Post Post { get; }

        public PostCompletedEvent(Post post)
        {
            Post = post;
        }
    }
}