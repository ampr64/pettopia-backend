using Application.Common.Models;
using Application.Features.Users;

namespace Application.Common.Translators
{
    public static class UserInfoToUserDto
    {
        public static List<UserDto> ToUserDto(IReadOnlyList<UserInfo?> userInfos)
        {
            if (userInfos == null) throw new ArgumentNullException(nameof(userInfos));

            var users = new List<UserDto>();
            foreach (var user in userInfos)
            {
                var userDto = new UserDto();
                userDto.Email = user.Email;
                userDto.FirstName = user.FirstName;
                userDto.LastName = user.LastName;
                userDto.RegisteredAt = user.RegisteredAt;
                userDto.BirthDate = user.BirthDate;
                userDto.Id = user.Id;
                userDto.Role = user.Role;
                users.Add(userDto);
            }
            return users;
        }

        public static UserDto ToUserDto(UserInfo? userInfo)
        {
            if (userInfo == null) throw new ArgumentNullException(nameof(userInfo));

            return new UserDto
            {
                Email = userInfo.Email,
                FirstName = userInfo.FirstName!,
                LastName = userInfo.LastName!,
                Id = userInfo.Id,
                BirthDate = userInfo.BirthDate,
                RegisteredAt = userInfo.RegisteredAt
            };
        }
    }
}
