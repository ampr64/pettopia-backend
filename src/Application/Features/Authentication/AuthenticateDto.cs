namespace Application.Features.Authentication
{
    public record AuthenticateDto
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Token { get; set; } = null!;
    }
}