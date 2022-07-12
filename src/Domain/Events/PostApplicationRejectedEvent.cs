namespace Domain.Events
{
    public sealed class PostApplicationRejectedEvent : DomainEvent
    {
        public PostApplication Application { get; }

        public PostApplicationRejectedEvent(PostApplication application)
        {
            Application = application;
        }
    }
}