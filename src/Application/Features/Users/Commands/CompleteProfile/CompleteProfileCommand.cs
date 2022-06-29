using Domain.Entities.Users;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.CompleteProfile
{
    public record CompleteProfileCommand : IRequest
    {
        public string Id { get; set; }

        public string AddressProvince { get; set; } = null!;

        public string AddressCity { get; set; } = null!;

        public string AddressLine1 { get; set; } = null!;

        public string? AddressLine2 { get; set; }

        public string AddressZipCode { get; set; } = null!;

        public string AboutMe { get; set; }

        public string PhonePrefix { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? InstagramUserName { get; set; }

        public string? FacebookUserName { get; set; }

        public List<string> AdoptionRequirements { get; set; } = new();

        public IFormFileCollection? Pictures { get; set; }
    }

    public class CompleteProfileCommandHandler : IRequestHandler<CompleteProfileCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IBlobService _blobService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;
        private readonly BlobSettings _blobSettings;

        public CompleteProfileCommandHandler(IApplicationDbContext dbContext,
            IBlobService blobService,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService,
            BlobSettings blobSettings)
        {
            _dbContext = dbContext;
            _blobService = blobService;
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
            _blobSettings = blobSettings;
        }

        public async Task<Unit> Handle(CompleteProfileCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId != request.Id) throw new ForbiddenAccessException();

            var user = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken) ?? throw new NotFoundException();
            var now = _dateTimeService.Now;
            var phoneNumber = new PhoneNumber(request.PhonePrefix, request.PhoneNumber);
            var address = new Address(request.AddressProvince, request.AddressCity, request.AddressLine1, request.AddressLine2, request.AddressZipCode);

            user.UpdateContactInfo(now,
                phoneNumber,
                GetInstagramUrl(request.InstagramUserName),
                GetFacebookUrl(request.FacebookUserName));

            user.SetAboutMe(request.AboutMe);
            user.SetAddress(now, address);

            if (user is Fosterer fosterer)
            {
                if (request.Pictures is { Count: > 0 })
                {
                    var blobs = await UploadBlobsAsync(request.Pictures, fosterer, cancellationToken);
                    var pictures = blobs.Select((b, i) => new FostererPicture(fosterer.Id, b, i + 1)).ToList();
                    
                    fosterer.SetPictures(now, pictures);
                }

                var applicationForm = new ApplicationForm(user.Id, request.AdoptionRequirements);

                fosterer.SetApplicationForm(now, applicationForm);
                user = fosterer;
            }

            user.SetAsComplete(now);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task<string[]> UploadBlobsAsync(IFormFileCollection images, Fosterer fosterer, CancellationToken cancellationToken)
        {
            var uploadImagesTasks = images.Select(async (image, index) =>
            {
                using var stream = image.OpenReadStream();
                return await _blobService.UploadBlobAsync(stream, image.ContentType, _blobSettings.Container, image.FileName, _blobSettings.UsersSection, fosterer.Id, cancellationToken);
            });

            return await Task.WhenAll(uploadImagesTasks);
        }

        private static string? GetInstagramUrl(string? instagramProfile)
        {
            const string instagramUrl = "https://www.instagram.com/";

            return instagramProfile is null ? null : instagramUrl + instagramProfile;
        }

        private static string? GetFacebookUrl(string? facebookProfile)
        {
            const string facebook = "https://www.facebook.com/";

            return facebookProfile is null ? null : facebook + facebookProfile;
        }
    }
}