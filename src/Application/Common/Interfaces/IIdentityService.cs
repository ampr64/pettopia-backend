﻿using Application.Common.Models;

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
        /// Gets a list of users with the specified role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the list of users.</returns>
        Task<IReadOnlyList<UserInfo?>> GetUsersByRole(string role);
    }
}