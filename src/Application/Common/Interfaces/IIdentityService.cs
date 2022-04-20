using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        /// <summary>
        /// Get the user info.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>The user info if it exists, else null.</returns>
        Task<UserInfo?> GetUserInfoAsync(string email);

        /// <summary>
        /// Authenticate the given username and password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>A serialized JSON Web Token if authentication succeeded, else null.</returns>
        Task<string?> AuthenticateAsync(string email, string password);
    }
}