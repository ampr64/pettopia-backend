namespace Application.Common.Exceptions
{
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException() : base("No matches found for the given credentials.")
        {
        }
    }
}