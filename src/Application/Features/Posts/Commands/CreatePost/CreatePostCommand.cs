using Application.Common.Interfaces;
using Application.Common.Settings;
using Domain.Entities;
using Domain.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Posts.Commands.CreatePost
{
    public record CreatePostCommand : IRequest<Guid>
    {
        public int PostType { get; set; }

        public string PetName { get; set; } = null!;

        public int PetGender { get; set; }

        public int NeuterStatus { get; set; }

        public string Description { get; set; } = null!;

        public int PetAge { get; set; }

        public int PetSpecies { get; set; }

        public IFormFileCollection Images { get; set; } = null!;
    }

    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBlobService _blobService;
        private readonly IDateTimeService _dateTimeService;
        private readonly BlobSettings _blobSettings;

        public CreatePostCommandHandler(IApplicationDbContext dbContext,
            ICurrentUserService currentUserService,
            IBlobService storageService,
            IDateTimeService dateTimeService,
            BlobSettings blobSettings)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _blobService = storageService;
            _dateTimeService = dateTimeService;
            _blobSettings = blobSettings;
        }

        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post((PostType)request.PostType,
                request.PetName,
                request.Description,
                (PetGender)request.PetGender,
                (NeuterStatus)request.NeuterStatus,
                (PetAge)request.PetAge,
                (PetSpecies)request.PetSpecies,
                _currentUserService.UserId!,
                _dateTimeService.Now);

            var blobs = await UploadBlobsAsync(request.Images, post, cancellationToken);

            post.Images = GetPostImages(post, blobs);

            await _dbContext.Posts.AddAsync(post, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return post.Id;
        }

        private static List<PostImage> GetPostImages(Post post, IEnumerable<string> blobs)
        {
            return blobs.Select((blob, index) => new PostImage
            {
                PostId = post.Id,
                Order = index + 1,
                Blob = blob
            }).ToList();
        }

        private async Task<string[]> UploadBlobsAsync(IFormFileCollection images, Post post, CancellationToken cancellationToken)
        {
            var uploadImagesTasks = images.Select(async (image, index) =>
            {
                using var stream = image.OpenReadStream();
                return await _blobService.UploadBlobAsync(stream, image.ContentType, _blobSettings.Container, image.FileName, _blobSettings.PostsSection, post.Id, cancellationToken);
            });

            return await Task.WhenAll(uploadImagesTasks);
        }
    }
}