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

        public static List<UserInfo> ToUserInfo(this IList<ApplicationUser> applicationUser)
        {
            if (applicationUser == null) throw new ArgumentNullException(nameof(applicationUser));

            var users = new List<UserInfo>();
            foreach (var user in applicationUser)
            {
                var userInfo = new UserInfo();
                userInfo.Email = user.Email;
                userInfo.FirstName = user.FirstName;
                userInfo.LastName = user.LastName;
                userInfo.Id = user.Id;
                users.Add(userInfo);
            }
            return users;
        }
    }
}