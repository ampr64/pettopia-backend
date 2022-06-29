using Domain.Entities.Users;
using Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.Queries.GetPosts
{
    public record GetPostsQuery : IRequest<IReadOnlyList<PostPreviewDto>>
    {
        public int? PetSpecies { get; set; }

        public List<int> PetGender { get; set; } = new();

        public List<int> PetAge { get; set; } = new();

        public List<int> PostType { get; set; } = new();

        public List<int> NeuterStatus { get; set; } = new();
    }

    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IReadOnlyList<PostPreviewDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IBlobService _blobService;
        private readonly BlobSettings _blobSettings;

        public GetPostsQueryHandler(IApplicationDbContext dbContext, IBlobService blobService, BlobSettings blobSettings)
        {
            _dbContext = dbContext;
            _blobService = blobService;
            _blobSettings = blobSettings;
        }

        public async Task<IReadOnlyList<PostPreviewDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var postsTasks = (
                from post in _dbContext.Posts.AsNoTracking()
                join user in _dbContext.Members.AsNoTracking().IgnoreQueryFilters() on post.CreatedBy equals user.Id
                where post.Status == PostStatus.Open
                where !request.PetSpecies.HasValue || post.PetSpecies == request.PetSpecies
                where !request.PetGender.Any() || request.PetGender.Contains(post.PetGender)
                where !request.PetAge.Any() || request.PetAge.Contains(post.PetAge)
                where !request.PostType.Any() || request.PostType.Contains(post.Type)
                where !request.NeuterStatus.Any() || request.NeuterStatus.Contains(post.NeuterStatus)
                orderby post.CreatedAt descending
                select new
                {
                    post.Id,
                    AuthorId = post.CreatedBy,
                    AuthorName = user.Role == Role.Fosterer ? ((Fosterer)user).OrganizationName : user.FirstName,
                    post.PetGender,
                    post.PetName,
                    post.CreatedAt,
                    Blob = post.Images.OrderBy(i => i.Order).Select(i => i.Blob).First(),
                })
                .AsEnumerable()
                .Select(async p => new PostPreviewDto
                {
                    Id = p.Id,
                    AuthorId = p.AuthorId,
                    AuthorName = p.AuthorName,
                    PetName = p.PetName,
                    PetGender = p.PetGender,
                    CreatedAt = p.CreatedAt,
                    Thumbnail = await _blobService.GetBlobAsync(_blobSettings.Container, p.Blob, cancellationToken)
                });

            return await Task.WhenAll(postsTasks);
        }
    }
}