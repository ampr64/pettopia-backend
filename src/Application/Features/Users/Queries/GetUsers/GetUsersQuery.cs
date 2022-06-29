using Domain.Entities.Users;
using Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<IReadOnlyList<UserDto>>
    {
        public List<string> Role { get; set; } = new();
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyList<UserDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IBlobService _blobService;
        private readonly BlobSettings _blobSettings;

        public GetUsersQueryHandler(IApplicationDbContext dbContext,
            IIdentityService identityService,
            IBlobService blobService,
            BlobSettings blobSettings)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _blobService = blobService;
            _blobSettings = blobSettings;
        }

        public async Task<IReadOnlyList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var roles = request.Role.Select(r => Role.FromName(r, true)).ToList();
            var currentUser = await _identityService.GetCurrentUserAsync(false, cancellationToken);
            var allowedRoles = GetAllowedRoles(currentUser);

            if (!roles.All(r => allowedRoles.Contains(r))) throw new ForbiddenAccessException();

            var usersTasks = _dbContext.Members
                .Where(m => (!roles.Any() && allowedRoles.Contains(m.Role)) || roles.Contains(m.Role))
                .OrderByDescending(m => m.Role)
                .ThenBy(m => m.RegisteredAt)
                .Select(m => new
                {
                    m.Id,
                    m.BirthDate,
                    m.Email,
                    m.FirstName,
                    m.LastName,
                    OrganizationName = m.Role == Role.Fosterer ? ((Fosterer)m).OrganizationName : null,
                    m.RegisteredAt,
                    Blob = m.ProfilePicture == null ? null : m.ProfilePicture.Blob,
                    Role = m.Role.Name,
                })
                .AsEnumerable()
                .Select(async m => new UserDto
                {
                    Id = m.Id,
                    Role = currentUser is Administrator or BackOfficeUser ? m.Role : null,
                    BirthDate = m.BirthDate,
                    Email = m.Email,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    OrganizationName = m.OrganizationName,
                    RegisteredAt = m.RegisteredAt,
                    ProfilePicture = m.Blob == null ? null : await _blobService.GetBlobAsync(_blobSettings.Container, m.Blob, cancellationToken)
                });

            return await Task.WhenAll(usersTasks);
        }

        private static IReadOnlyCollection<Role> GetAllowedRoles(Member? user)
        {
            return user switch
            {
                Administrator => Role.List,
                BackOfficeUser => new[] { Role.User, Role.Fosterer, Role.BackOfficeUser },
                _ => new[] { Role.Fosterer }
            };
        }
    }
}