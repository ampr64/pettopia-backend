namespace Domain.Events
{
    public sealed class UserRegisteredEvent : DomainEvent
    {
        public Member Member { get; }

        public UserRegisteredEvent(Member member)
        {
            Member = member;
        }
    }
}