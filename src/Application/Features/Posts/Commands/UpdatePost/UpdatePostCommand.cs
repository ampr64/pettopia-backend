using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Settings;
using Domain.Entities;
using Domain.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.Commands.UpdatePost
{
    public record UpdatePostCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }

        public int PostType { get; set; }

        public string PetName { get; set; } = null!;

        public int PetGender { get; set; }

        public int NeuterStatus { get; set; }

        public string Description { get; set; } = null!;

        public int PetAge { get; set; }

        public int PetSpecies { get; set; }

        public IFormFileCollection Images { get; set; } = null!;
    }

    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IBlobService _blobService;
        private readonly BlobSettings _blobSettings;

        public UpdatePostCommandHandler(IApplicationDbContext dbContext, IBlobService blobService, BlobSettings blobSettings)
        {
            _dbContext = dbContext;
            _blobService = blobService;
            _blobSettings = blobSettings;
        }

        public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.PostStatus == PostStatus.Open, cancellationToken)
                ?? throw new NotFoundException();

            post.PostType = (PostType)request.PostType;
            post.PetName = request.PetName;
            post.PetGender = (PetGender)request.PetGender;
            post.NeuterStatus = (NeuterStatus)request.NeuterStatus;
            post.Description = request.Description;
            post.PetSpecies = (PetSpecies)request.PetSpecies;

            await DeletePostImagesBlobsAsync(post, cancellationToken);
            var blobs = await UploadBlobsAsync(request.Images, post, cancellationToken);

            post.Images = GetPostImages(post, blobs);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
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

        private async Task DeletePostImagesBlobsAsync(Post post, CancellationToken cancellationToken)
        {
            foreach (var image in post.Images)
            {
                await _blobService.DeleteAsync(_blobSettings.Container, image.Blob, cancellationToken);
            }
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
    }
}