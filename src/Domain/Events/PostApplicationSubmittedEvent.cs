namespace Domain.Events
{
    public sealed class PostApplicationSubmittedEvent : DomainEvent
    {
        public PostApplication Application { get; }

        public PostApplicationSubmittedEvent(PostApplication application)
        {
            Application = application;
        }
    }
}