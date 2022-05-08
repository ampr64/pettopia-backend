namespace Application.Features.Users
{
    public class UserDto
    {
        public string Id { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public DateTime RegisteredAt { get; set; }
    }
}
