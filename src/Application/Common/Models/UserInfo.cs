using Domain.ValueObjects;

namespace Application.Common.Models
{
    public class UserInfo
    {
        public string Id { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? ProfilePictureUrl { get; set; }

        public string? PhoneNumber { get; set; }

        public string UserName { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public DateTime RegisteredAt { get; set; }

        public Address? Address { get; set; }
    }
}