using Application.Common.Models;
using System.Security.Claims;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        /// <summary>
        /// Authenticate the given email and password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the serialized JSON Web Token if authentication succeeded, else null.</returns>
        Task<string?> AuthenticateAsync(string email, string password);

        /// <summary>
        /// Creates a new user with the specified role.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <param name="password">The password.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="birthDate">The date of birth.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result{string}"/> of the operation.</returns>
        Task<Result<string?>> CreateUserAsync(string email, string password, string firstName, string lastName, DateTime birthDate, string role);

        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="UserInfo"/> if it exists, else null.</returns>
        Task<UserInfo?> GetUserInfoAsync(string email);

        /// <summary>
        /// Gets the user info for the given <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="UserInfo"/> if it exists, else null.</returns>
        Task<UserInfo?> GetUserInfoAsync(ClaimsPrincipal principal);

        /// <summary>
        /// Gets a list of users with the specified role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the list of users.</returns>
        Task<IReadOnlyList<UserInfo?>> GetUsersByRoleAsync(string role);

        /// <summary>
        /// Deletes a user specified by id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing succed or failed message.</returns>
        Task<bool> DeleteUserAsync(string userId);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="email">The email.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="birthDate">The date of birth.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, if succed true, else false.</returns>
        Task<bool> UpdateUserAsync(string userId, string email, string firstName, string lastName, DateTime birthDate);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="UserInfo"/> if it exists, else null.</returns>
        Task<UserInfo?> GetUserInfoByIdAsync(string userId);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="role">The user role.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, if succed true, else false.</returns>
        Task<bool> ValidateUserRoleAsync(string userId, string role);

        /// <summary>
        /// Creates a new user with the specified role.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <param name="password">The password.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="birthDate">The date of birth.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result{string}"/> of the operation.</returns>
        Task<Result<string?>> CreateBackOfficeUserAsync(string email, string password, string firstName, string lastName, DateTime birthDate);

        /// <summary>
        /// Gets the user info of the authenticated user.
        /// </summary>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="UserInfo"/> if authenticated, else null.</returns>
        Task<UserInfo?> GetCurrentUserAsync(CancellationToken cancellationToken = default);
    }
}