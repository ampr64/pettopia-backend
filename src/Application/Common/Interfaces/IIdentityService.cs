using Domain.Entities.Users;
using Domain.Enumerations;

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
        /// <param name="role">The role.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result{string}"/> of the operation.</returns>
        Task<Result<string?>> CreateUserAsync(string email, string password, Role role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a user specified by id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing succed or failed message.</returns>
        Task<bool> DeleteUserAsync(string userId);

        /// <summary>
        /// Generates an email confirmation token for the specified user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, an email confirmation token.</returns>
        Task<string?> GetEmailConfirmationToken(string userId);

        /// <summary>
        /// Validates that an email confirmation token matches the specified user.
        /// </summary>
        /// <param name="email">The user's email to validate the token against.</param>
        /// <param name="token">The email confirmation token to validate.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the result of the operation.</returns>
        Task<bool> ConfirmEmailAsync(string email, string token);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="email">The email.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, if succed true, else false.</returns>
        Task<bool> ChangeEmailAsync(string userId, string email);

        /// <summary>
        /// Returns a flag indicating whether the user has elevated access.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing a flag indicating whether the user with the given id has elevated access.</returns>
        Task<bool> IsElevatedAccessUser(string userId);

        /// <summary>
        /// Gets the user info of the authenticated user.
        /// </summary>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="UserInfo"/> if authenticated, else null.</returns>
        Task<Member?> GetCurrentUserAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    }
}