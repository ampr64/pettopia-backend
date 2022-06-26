using Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.Commands.DeletePost
{
    public record DeletePostCommand(Guid Id) : IRequest<Unit>;

    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public DeletePostCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IDateTimeService dateTimeService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts
                .Include(p => p.Applications)
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Status == PostStatus.Open, cancellationToken)
                ?? throw new NotFoundException();

            if (post.CreatedBy != _currentUserService.UserId) throw new ForbiddenAccessException();

            post.Close(_dateTimeService.Now);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}