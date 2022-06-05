using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime BirthDate { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IIdentityService _identityService;

        public UpdateUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserInfoByIdAsync(request.Id);

            if (user is null) throw new NotFoundException();

            if (!await _identityService.UpdateUserAsync(request.Id, request.Email, request.FirstName, request.LastName, request.BirthDate))
            {
                throw new UnprocessableEntityException();
            }

            return Unit.Value;
        }
    }

}
