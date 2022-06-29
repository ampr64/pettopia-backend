using Domain.Entities.Users;
using MediatR;

namespace Application.Features.Users.Queries.GetCurrentUser
{
    public record GetCurrentUserQuery : IRequest<MyProfileDto>;

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, MyProfileDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IBlobService _blobService;
        private readonly BlobSettings _blobSettings;

        public GetCurrentUserQueryHandler(IIdentityService identityService,
            IBlobService blobService,
            BlobSettings blobSettings)
        {
            _identityService = identityService;
            _blobService = blobService;
            _blobSettings = blobSettings;
        }

        public async Task<MyProfileDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetCurrentUserAsync(false, cancellationToken);
            
            var fosterer = user as Fosterer;
            var pictures = new List<BlobData>();

            if (fosterer is not null)
            {
                foreach (var picture in fosterer.Pictures)
                {
                    var blob = await _blobService.GetBlobAsync(_blobSettings.Container, picture.Blob, cancellationToken);
                    pictures.Add(blob);
                }
            }

            return new MyProfileDto
            {
                Id = user!.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OrganizationName = fosterer?.OrganizationName,
                Role = user.Role.Name,
                AboutMe = user.AboutMe,
                ProfilePicture = user.ProfilePicture is null ? null : await _blobService.GetBlobAsync(_blobSettings.Container, user.ProfilePicture.Blob, cancellationToken),
                AddressProvince = user.Address?.Province,
                AddressCity = user.Address?.City,
                AddressLine1 = user.Address?.Line1,
                AddressLine2 = user.Address?.Line2,
                AddressZipCode = user.Address?.ZipCode,
                PhoneNumber = user.PhoneNumber?.Number,
                PhonePrefix = user.PhoneNumber?.Prefix,
                FacebookProfileUrl = user.FacebookProfileUrl,
                InstagramProfileUrl = user.InstagramProfileUrl,
                AdoptionRequirements = fosterer?.ApplicationForm?.Requirements.Select(r => r.ToString()),
                Pictures = pictures,
                BirthDate = user.BirthDate,
                RegisteredAt = user.RegisteredAt
            };
        }
    }
}