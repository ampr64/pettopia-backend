using Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand : IRequest<Unit>
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime BirthDate { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IDateTimeService _dateTimeService;

        public UpdateUserCommandHandler(IApplicationDbContext dbContext, IIdentityService identityService, IDateTimeService dateTimeService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _dateTimeService = dateTimeService;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var targetUser = await _dbContext.Members
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException();

            var currentUser = await _identityService.GetCurrentUserAsync(false, cancellationToken);
            var now = _dateTimeService.Now;

            if (currentUser!.Id == request.Id)
            {
                targetUser.UpdateDetails(now, request.FirstName, request.LastName, request.BirthDate);
            }

            if (!await _identityService.IsElevatedAccessUser(currentUser.Id) || targetUser.Role == Role.Admin) throw new ForbiddenAccessException();

            targetUser.UpdateDetails(now, request.FirstName, request.LastName, request.BirthDate);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}