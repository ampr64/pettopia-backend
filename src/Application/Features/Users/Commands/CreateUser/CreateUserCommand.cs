using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Enumerations;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IIdentityService _identityService;

        public static string[] ForbiddenRoles => new[] { Role.Admin.Name, Role.BackOfficeUser.Name };

        public CreateUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (IsForbiddenRole(request.Role))
            {
                throw new ForbiddenAccessException();
            }

            var result = await _identityService.CreateUserAsync(request.Email, request.Password, request.FirstName, request.LastName, request.BirthDate, request.Role);

            if (!result.Succeeded) throw new UnprocessableEntityException("User could not be created.", result.Errors);

            return result.Data;
        }

        private static bool IsForbiddenRole(string role)
        {
            return ForbiddenRoles.Any(r => r.Equals(role, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}