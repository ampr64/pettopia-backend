using Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(string Id) : IRequest<Unit>;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IDateTimeService _dateTimeService;

        public DeleteUserCommandHandler(IApplicationDbContext dbContext, IIdentityService identityService, IDateTimeService dateTimeService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _dateTimeService = dateTimeService;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var targetUser = await _dbContext.Members
                .Include(m => m.Posts)
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException();

            if (targetUser.Role == Role.Admin) throw new ForbiddenAccessException();

            var currentUser = await _identityService.GetCurrentUserAsync(false, cancellationToken);
            var now = _dateTimeService.Now;

            if (currentUser!.Id == request.Id)
            {
                targetUser.MarkAsDeactivated(now);
            }
            else if (currentUser.Role == Role.Admin || targetUser.Role != Role.BackOfficeUser)
            {
                targetUser.MarkAsBanned(now);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}