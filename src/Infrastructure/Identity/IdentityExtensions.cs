using Application.Common.Models;
using Application.Features.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public static class IdentityExtensions
    {
        public static UserInfo ToUserInfo(this UserDto userDto)
        {
            if (userDto == null) throw new ArgumentNullException(nameof(userDto));

            return new UserInfo
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName!,
                LastName = userDto.LastName!,
                Id = userDto.Id,
                BirthDate = userDto.BirthDate,
                RegisteredAt = userDto.RegisteredAt
            };
        }

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
                Address = applicationUser.Address,
                ProfilePictureUrl = applicationUser.ProfilePictureUrl,
                PhoneNumber = applicationUser.PhoneNumber,
                UserName = applicationUser.UserName
            };
        }

        public static UserInfo ToUserInfo(this ApplicationUser applicationUser)
        {
            if (applicationUser == null) throw new ArgumentNullException(nameof(applicationUser));

            return new UserInfo
            {
                Email = applicationUser.Email,
                FirstName = applicationUser.FirstName!,
                LastName = applicationUser.LastName!,
                Id = applicationUser.Id,
                BirthDate = applicationUser.BirthDate,
                RegisteredAt = applicationUser.RegisteredAt,
                Address = applicationUser.Address,
                ProfilePictureUrl = applicationUser.ProfilePictureUrl,
                PhoneNumber = applicationUser.PhoneNumber,
                UserName = applicationUser.UserName
            };
        }

        public static Result<string?> ToResultObject(this IdentityResult result, string userId)
        {
            return result.Succeeded
                ? Result<string?>.Success(userId)
                : Result<string?>.Failure(result.Errors.Select(e => e.Description));
        }

        public static List<UserInfo> ToUserInfo(this IList<ApplicationUser> applicationUser, string role)
        {
            if (applicationUser == null) throw new ArgumentNullException(nameof(applicationUser));

            var users = new List<UserInfo>();
            foreach (var user in applicationUser)
            {
                var userInfo = new UserInfo();
                userInfo.Email = user.Email;
                userInfo.FirstName = user.FirstName;
                userInfo.LastName = user.LastName;
                userInfo.RegisteredAt = user.RegisteredAt;
                userInfo.BirthDate = user.BirthDate;
                userInfo.Id = user.Id;
                userInfo.Address = user.Address;
                userInfo.ProfilePictureUrl = user.ProfilePictureUrl;
                userInfo.PhoneNumber = user.PhoneNumber;
                userInfo.UserName = user.UserName;
                userInfo.Role = role;

                users.Add(userInfo);
            }
            return users;
        }

    }
}