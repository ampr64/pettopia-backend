using Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authentication.Commands.Authenticate
{
    public record AuthenticateCommand : IRequest<AuthenticateDto>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;

        public AuthenticateCommandHandler(IApplicationDbContext dbContext, IIdentityService identityService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
        }

        public async Task<AuthenticateDto> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var token = await _identityService.AuthenticateAsync(request.Email, request.Password) ?? throw new AuthenticationFailedException();
            var user = await _dbContext.Members
                .FirstOrDefaultAsync(m => m.Email == request.Email, cancellationToken)
                ?? throw new ForbiddenAccessException();

            return new AuthenticateDto
            {
                Id = user.Id,
                Email = user.Email,
                OrganizationName = (user as Fosterer)?.OrganizationName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsProfileComplete = user.IsComplete,
                Role = user.Role.Name,
                Token = token
            };
        }
    }
}