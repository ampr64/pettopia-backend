using Domain.Entities.Users;
using Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Stats.Queries.GetStats
{
    public record GetStatsQuery : IRequest<StatsVm>;

    public class GetStatsQueryHandler : IRequestHandler<GetStatsQuery, StatsVm>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetStatsQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<StatsVm> Handle(GetStatsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new StatsVm
            {
                ActiveMembersCount = _dbContext.Members
                    .Where(m => m.Role != Role.Admin && m.Role != Role.BackOfficeUser)
                    .Count(),
                InactiveMembersCount = _dbContext.Members
                    .IgnoreQueryFilters()
                    .Where(m => m.Status == AccountStatus.Banned || m.Status == AccountStatus.Deactivated)
                    .Count(),
                FosterersCount = _dbContext.Fosterers.Count(),
                TotalPostsCount = _dbContext.Posts.Count(),
                CompletedPostsCount = _dbContext.Posts
                    .Where(p => p.Status == PostStatus.Completed)
                    .Count(),
                AdoptionsCount = _dbContext.Posts.Where(p => p.Type == PostType.Adoption).Count(),
                FoundCount = _dbContext.Posts.Where(p => p.Type == PostType.Found).Count(),
                MissingCount = _dbContext.Posts.Where(p => p.Type == PostType.Missing).Count(),
            });
        }
    }
}