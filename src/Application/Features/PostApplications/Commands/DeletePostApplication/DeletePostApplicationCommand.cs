using Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PostApplications.Commands.DeleteApplication
{
    public record DeletePostApplicationCommand(Guid PostId, Guid Id) : IRequest<Unit>;

    public class DeletePostApplicationCommandHandler : IRequestHandler<DeletePostApplicationCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public DeletePostApplicationCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IDateTimeService dateTimeService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public async Task<Unit> Handle(DeletePostApplicationCommand request, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts
                .Include(p => p.Applications)
                .FirstOrDefaultAsync(p => p.Id == request.PostId
                    && p.Status == PostStatus.Open
                    && p.Applications.Any(a => a.Id == request.Id),
                    cancellationToken)
                ?? throw new NotFoundException();

            var application = post.Applications.Single(a => a.Id == request.Id);

            if (post.CreatedBy == _currentUserService.UserId)
            {
                post.RejectApplication(application, _dateTimeService.Now);
            }
            else if (application.ApplicantId == _currentUserService.UserId)
            {
                post.CancelApplication(application, _dateTimeService.Now);
            }
            else throw new ForbiddenAccessException();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}