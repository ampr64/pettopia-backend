using Application.Common.Interfaces;
using Application.Common.Exceptions;
using MediatR;
using Domain.Enumerations;

namespace Application.Features.Users.Commands.DeleteUser
{
    public class DeleteBackOfficeUserCommand: IRequest<Unit>
    {
        public string Id { get; set; } = null!;

        public class DeleteBackOfficeUserCommandHandler: IRequestHandler<DeleteBackOfficeUserCommand, Unit>
        {
            private readonly IIdentityService _identityService;

            public DeleteBackOfficeUserCommandHandler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<Unit> Handle(DeleteBackOfficeUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _identityService.GetUserInfoByIdAsync(request.Id) ?? throw new NotFoundException();
                bool isInRole = await _identityService.ValidateUserRoleAsync(user.Id, Role.BackOfficeUser.Name);

                if (!isInRole)
                {
                    throw new ForbiddenAccessException();
                }

                await _identityService.DeleteUserAsync(user.Id);

                return Unit.Value;
            }
        }
    }
}
