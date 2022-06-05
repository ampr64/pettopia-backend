using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.CreateBackOfficeUser
{
    public record CreateBackOfficeUserCommand : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
               
        public DateTime BirthDate { get; set; }
    }

    public class CreateBackOfficeUserCommandHandler : IRequestHandler<CreateBackOfficeUserCommand, string>
    {
        private readonly IIdentityService _identityService;
               
        public CreateBackOfficeUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<string> Handle(CreateBackOfficeUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateBackOfficeUserAsync(request.Email, request.Password, request.FirstName, request.LastName, request.BirthDate);

            if (!result.Succeeded) throw new UnprocessableEntityException("User could not be created.", result.Errors);

            return result.Data;
        }

    }
}
