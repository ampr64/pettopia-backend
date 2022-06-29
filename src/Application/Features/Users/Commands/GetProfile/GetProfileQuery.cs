using Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.GetProfile
{
    public record GetProfileQuery(string Id) : IRequest<ProfileDto>;

    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IBlobService _blobService;
        private readonly BlobSettings _blobSettings;

        public GetProfileQueryHandler(IApplicationDbContext dbContext,
            IBlobService blobService,
            BlobSettings blobSettings)
        {
            _dbContext = dbContext;
            _blobService = blobService;
            _blobSettings = blobSettings;
        }

        public async Task<ProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Members
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException();
            var profilePicture = user.ProfilePicture is null ? null : await _blobService.GetBlobAsync(_blobSettings.Container, user.ProfilePicture.Blob, cancellationToken);
            
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

            return new ProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OrganizationName = (user as Fosterer)?.OrganizationName,
                AboutMe = user.AboutMe,
                ProfilePicture = profilePicture,
                Pictures = pictures,
                AddressProvince = user.Address?.Province,
                AddressCity = user.Address?.City,
                RegisteredAt = user.RegisteredAt,
                FacebookProfileUrl = fosterer?.FacebookProfileUrl,
                InstagramProfileUrl = fosterer?.InstagramProfileUrl,
            };
        }
    }
}