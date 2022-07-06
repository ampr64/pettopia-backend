namespace Domain.Events
{
    public sealed class ApplicationSubmittedEvent : DomainEvent
    {
        public PostApplication Application { get; }

        public ApplicationSubmittedEvent(PostApplication application)
        {
            Application = application;
        }
    }
}