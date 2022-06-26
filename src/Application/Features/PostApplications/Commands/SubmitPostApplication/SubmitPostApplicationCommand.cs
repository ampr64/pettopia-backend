using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PostApplications.Commands.SubmitPostApplication
{
    public record SubmitPostApplicationCommand(Guid PostId) : IRequest<Guid>;

    public class SubmitPostApplicationCommandHandler : IRequestHandler<SubmitPostApplicationCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IDateTimeService _dateTimeService;

        public SubmitPostApplicationCommandHandler(IApplicationDbContext dbContext, IIdentityService identityService, IDateTimeService dateTimeService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _dateTimeService = dateTimeService;
        }

        public async Task<Guid> Handle(SubmitPostApplicationCommand request, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts
                .Include(p => p.Applications)
                .FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken)
                ?? throw new NotFoundException();

            var user = await _identityService.GetCurrentUserAsync(cancellationToken);

            var applicationId = post.AddApplication(_dateTimeService.Now, user!.Id, user.FirstName, user.Email, user.PhoneNumber);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return applicationId;
        }
    }
}