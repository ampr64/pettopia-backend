using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class CustomIdentityUser : IdentityUser
    {
        public CustomIdentityUser(string email) : base(email)
        {
            Email = email;
        }
    }
}