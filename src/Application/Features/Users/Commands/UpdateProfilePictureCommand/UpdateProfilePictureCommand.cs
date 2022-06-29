using Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.UpdateProfilePictureCommand
{
    public record UpdateProfilePictureCommand(string Id, IFormFile Picture) : IRequest;

    public class UpdateProfilePictureCommandHandler : IRequestHandler<UpdateProfilePictureCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IBlobService _blobService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;
        private readonly BlobSettings _blobSettings;

        public UpdateProfilePictureCommandHandler(IApplicationDbContext dbContext,
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

        public async Task<Unit> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId != request.Id) throw new ForbiddenAccessException();

            var user = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            var blob = await _blobService.UploadBlobAsync(request.Picture.OpenReadStream(),
                request.Picture.ContentType,
                _blobSettings.Container,
                request.Picture.FileName,
                user!.Id,
                cancellationToken);

            var profilePicture = new ProfilePicture(blob);
            user.ChangeProfilePicture(_dateTimeService.Now, profilePicture);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}