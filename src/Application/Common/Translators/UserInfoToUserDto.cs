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
                userDto.Id = user.Id;
                users.Add(userDto);
            }
            return users;
        }
    }
}
