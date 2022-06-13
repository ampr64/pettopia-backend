namespace Application.Common.Interfaces
{
    public interface ITokenClaimsService
    {
        /// <summary>
        /// Gets a JSON Web Token for the user associated to the given email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the serialized JSON Web Token, or null if no user was found with the given email.</returns>
        Task<string?> GetTokenAsync(string email);
    }
}