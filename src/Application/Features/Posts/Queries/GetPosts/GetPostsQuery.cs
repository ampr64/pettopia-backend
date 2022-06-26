using Domain.Entities.Posts;
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
        private readonly IIdentityService _identityService;
        private readonly IBlobService _blobService;
        private readonly BlobSettings _blobSettings;

        public GetPostsQueryHandler(IApplicationDbContext dbContext, IIdentityService identityService, IBlobService blobService, BlobSettings blobSettings)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _blobService = blobService;
            _blobSettings = blobSettings;
        }

        public async Task<IReadOnlyList<PostPreviewDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Posts
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => p.Status == PostStatus.Open);

            query = FilterByPetSpecies(query, request.PetSpecies);
            query = FilterByGender(query, request.PetGender);
            query = FilterByPetAge(query, request.PetAge);
            query = FilterByPostType(query, request.PostType);
            query = FilterByNeuterStatus(query, request.NeuterStatus);

            var posts = await query.ToListAsync(cancellationToken);

            var projectPostsTasks = posts.Select(async p => new PostPreviewDto
            {
                Id = p.Id,
                AuthorId = p.CreatedBy,
                AuthorName = _identityService.GetUserInfoByIdAsync(p.CreatedBy)!.Result!.FirstName, // Sync hotfix, otherwise throws concurrency exception -- TODO: Add domain user so we can query DB side
                PetGender = p.PetGender,
                PetName = p.PetName,
                CreatedAt = p.CreatedAt,
                Thumbnail = await _blobService.GetBlobAsync(_blobSettings.Container, p.Images.OrderBy(i => i.Order).Select(i => i.Blob).First())
            });

            return await Task.WhenAll(projectPostsTasks);
        }

        private static IQueryable<Post> FilterByPetSpecies(IQueryable<Post> query, int? petSpecies)
        {
            return petSpecies is not null
                ? query.Where(p => p.PetSpecies == petSpecies)
                : query;
        }

        private static IQueryable<Post> FilterByGender(IQueryable<Post> query, List<int> genderFilters)
        {
            return genderFilters is { Count: > 0 }
                ? query.Where(p => genderFilters.Contains(p.PetGender))
                : query;
        }

        private static IQueryable<Post> FilterByPetAge(IQueryable<Post> query, List<int> ageFilters)
        {
            return ageFilters is { Count: > 0 }
                ? query.Where(p => ageFilters.Contains(p.PetAge))
                : query;
        }

        private static IQueryable<Post> FilterByPostType(IQueryable<Post> query, List<int> postTypeFilters)
        {
            return postTypeFilters is { Count: > 0 }
                ? query.Where(p => postTypeFilters.Contains(p.Type))
                : query;
        }

        private static IQueryable<Post> FilterByNeuterStatus(IQueryable<Post> query, List<int> neuterStatusFilters)
        {
            return neuterStatusFilters is { Count: > 0 }
                ? query.Where(p => neuterStatusFilters.Contains(p.NeuterStatus))
                : query;
        }
    }
}