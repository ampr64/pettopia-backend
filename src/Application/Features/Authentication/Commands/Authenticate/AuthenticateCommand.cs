using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Authentication.Commands.Authenticate
{
    public record AuthenticateCommand : IRequest<AuthenticateDto>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateDto>
    {
        private readonly IIdentityService _identityService;

        public AuthenticateCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthenticateDto> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var token = await _identityService.AuthenticateAsync(request.Email, request.Password) ?? throw new AuthenticationFailedException();
            var userInfo = await _identityService.GetUserInfoAsync(request.Email);

            return new AuthenticateDto
            {
                Id = userInfo!.Id,
                Email = userInfo!.Email,
                FirstName = userInfo!.FirstName,
                LastName = userInfo!.LastName,
                Role = userInfo!.Role,
                Token = token
            };
        }
    }
}