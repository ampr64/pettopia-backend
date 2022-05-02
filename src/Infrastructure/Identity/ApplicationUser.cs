using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public DateTime RegisteredAt { get; private set; }

        public string? ProfilePictureUrl { get; set; }

        public ApplicationUser(string email,
            string firstName,
            string lastName,
            DateTime birthDate) : base(email)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        // Creates new user
        public ApplicationUser(string email,
            string firstName,
            string lastName,
            DateTime birthDate,
            IDateTimeService dateTimeService) : this(email, firstName, lastName, birthDate)
        {
            RegisteredAt = dateTimeService.Now;
        }
    }
}