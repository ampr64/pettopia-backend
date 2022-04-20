using Application.Common.Models;

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
                Role = roleName
            };
        }
    }
}