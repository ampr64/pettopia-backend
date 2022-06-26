using Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.Commands.CompletePost
{
    public record CompletePostCommand(Guid Id, Guid ApplicationId) : IRequest;

    public class CompletePostCommandHandler : IRequestHandler<CompletePostCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public CompletePostCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IDateTimeService dateTimeService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public async Task<Unit> Handle(CompletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts
                .Include(p => p.Applications)
                .FirstOrDefaultAsync(p => p.Id == request.Id
                    && p.Status == PostStatus.Open
                    && p.Applications.Any(a => a.Id == request.ApplicationId),
                    cancellationToken)
                ?? throw new NotFoundException();

            if (post.CreatedBy != _currentUserService.UserId) throw new ForbiddenAccessException();

            var application = post.Applications.Single(a => a.Id == request.ApplicationId);

            post.Complete(application, _dateTimeService.Now);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}