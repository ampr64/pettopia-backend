using Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public static class IdentityExtensions
    {
        public static UserInfo ToUserInfo(this ApplicationUser applicationUser, string roleName)
        {
            if (applicationUser == null) throw new ArgumentNullException(nameof(applicationUser));

            return new UserInfo
            {
                Email = applicationUser.Email,
                FirstName = applicationUser.FirstName!,
                LastName = applicationUser.LastName!,
                Id = applicationUser.Id,
                Role = roleName,
                BirthDate = applicationUser.BirthDate,
                RegisteredAt = applicationUser.RegisteredAt,
            };
        }

        public static Result<string?> ToResultObject(this IdentityResult result, string userId)
        {
            return result.Succeeded
                ? Result<string?>.Success(userId)
                : Result<string?>.Failure(result.Errors.Select(e => e.Description));
        }
    }
}