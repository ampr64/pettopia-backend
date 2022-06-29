using Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.Queries.GetMyPosts
{
    public record GetMyPostsQuery : IRequest<IReadOnlyList<PostBriefDto>>;

    public class GetMyPostsQueryHandler : IRequestHandler<GetMyPostsQuery, IReadOnlyList<PostBriefDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBlobService _blobService;
        private readonly BlobSettings _blobSettings;

        public GetMyPostsQueryHandler(IApplicationDbContext dbContext,
            ICurrentUserService currentUserService,
            IBlobService blobService,
            BlobSettings blobSettings)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _blobService = blobService;
            _blobSettings = blobSettings;
        }

        public async Task<IReadOnlyList<PostBriefDto>> Handle(GetMyPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _dbContext.Posts
                .AsNoTracking()
                .Include(p => p.Applications)
                .Where(p => p.Status == PostStatus.Open && p.CreatedBy == _currentUserService.UserId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostBriefDto
                {
                    Id = p.Id,
                    PetName = p.PetName,
                    PetGender = p.PetGender,
                    PostType = p.Type,
                    CreatedAt = p.CreatedAt,
                    BlobName = p.Images
                        .OrderBy(i => i.Order)
                        .Select(i => i.Blob)
                        .First(),
                    Applications = p.Applications
                        .Where(a => a.Status == ApplicationStatus.Pending)
                        .OrderByDescending(a => a.SubmittedAt)
                        .Select(a => new ApplicationBriefDto
                        {
                            Id = a.Id,
                            ApplicantId = a.ApplicantId,
                            ApplicantEmail = a.ApplicantInfo.Email,
                            ApplicantName = a.ApplicantInfo.Name,
                            SubmittedAt = a.SubmittedAt
                        }).ToList(),
                }).ToListAsync(cancellationToken);

            var setThumbnailBlobsTasks = posts.Select(async p =>
            {
                p.Thumbnail = await _blobService.GetBlobAsync(_blobSettings.Container, p.BlobName, cancellationToken);
            });

            await Task.WhenAll(setThumbnailBlobsTasks);

            return posts;
        }
    }
}