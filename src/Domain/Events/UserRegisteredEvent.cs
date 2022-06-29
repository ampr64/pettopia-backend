namespace Domain.Events
{
    public class UserRegisteredEvent : DomainEvent
    {
        public Member Member { get; }

        public UserRegisteredEvent(Member member)
        {
            Member = member;
        }
    }
}