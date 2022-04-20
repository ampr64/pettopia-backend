using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public ApplicationUser(string email,
            string firstName,
            string lastName) : base(email)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}