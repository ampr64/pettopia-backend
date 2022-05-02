namespace Application.Common.Exceptions
{
    public class UnprocessableEntityException : Exception
    {
        public IEnumerable<string> Errors { get; } = new List<string>();

        public UnprocessableEntityException() : base()
        {
        }

        public UnprocessableEntityException(string message) : base(message)
        {
        }

        public UnprocessableEntityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public UnprocessableEntityException(string message, IEnumerable<string> errors)
            : this(message)
        {
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }
    }
}