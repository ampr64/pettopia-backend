using Domain.Entities.Users;
using Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.Queries.GetPostDetail
{
    public record GetPostDetailQuery(Guid Id) : IRequest<PostDetailDto>;

    public class GetPostDetailQueryHandler : IRequestHandler<GetPostDetailQuery, PostDetailDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IBlobService _blobStorageService;
        private readonly BlobSettings _blobSettings;

        public GetPostDetailQueryHandler(IApplicationDbContext dbContext, IBlobService blobStorageService, BlobSettings blobSettings)
        {
            _dbContext = dbContext;
            _blobStorageService = blobStorageService;
            _blobSettings = blobSettings;
        }

        public async Task<PostDetailDto> Handle(GetPostDetailQuery request, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Status == PostStatus.Open, cancellationToken)
                ?? throw new NotFoundException();

            var author = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == post.CreatedBy, cancellationToken);
            var fosterer = author as Fosterer;
            
            var blobs = new List<BlobData>();
            foreach (var image in post.Images)
            {
                var blob = await _blobStorageService.GetBlobAsync(_blobSettings.Container, image.Blob, cancellationToken);
                blobs.Add(blob);
            }

            return new PostDetailDto
            {
                Id = post.Id,
                AuthorId = post.CreatedBy,
                AuthorName = fosterer?.OrganizationName ?? author!.FirstName,
                Description = post.Description,
                NeuterStatus = post.NeuterStatus,
                PetAge = post.PetAge,
                PetSpecies = post.PetSpecies,
                PetGender = post.PetGender,
                PetName = post.PetName,
                PostType = post.Type,
                Images = blobs,
            };
        }
    }
}