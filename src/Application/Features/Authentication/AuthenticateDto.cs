using Domain.Entities.Users;

namespace Application.Features.Authentication
{
    public record AuthenticateDto
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? OrganizationName { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Role { get; set; } = null!;

        public bool IsProfileComplete { get; set; }

        public string Token { get; set; } = null!;
    }
}