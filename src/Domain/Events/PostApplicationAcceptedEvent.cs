namespace Domain.Events
{
    public sealed class PostApplicationAcceptedEvent : DomainEvent
    {
        public PostApplication Application { get; }

        public PostApplicationAcceptedEvent(PostApplication application)
        {
            Application = application;
        }
    }
}