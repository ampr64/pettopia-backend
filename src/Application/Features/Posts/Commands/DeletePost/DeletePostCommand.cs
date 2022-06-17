using Application.Common.Exceptions;
using Application.Common.Interfaces;
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

        public DeletePostCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts
                .FirstOrDefaultAsync(p => p.Id == request.Id
                    && p.CreatedBy == _currentUserService.UserId
                    && p.PostStatus == PostStatus.Open, cancellationToken)
                ?? throw new NotFoundException();

            post.Close();

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}