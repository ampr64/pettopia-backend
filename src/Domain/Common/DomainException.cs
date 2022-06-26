namespace Domain.Common
{
    public class DomainException : Exception
    {
        public DomainException() : base("There has occurred an exception in the domain.")
        {
        }

        public DomainException(string message) : base(message)
        {
        }
    }
}