namespace Domain.Events
{
    public sealed class PostApplicationCanceledEvent : DomainEvent
    {
        public PostApplication Application { get; }

        public PostApplicationCanceledEvent(PostApplication application)
        {
            Application = application;
        }
    }
}