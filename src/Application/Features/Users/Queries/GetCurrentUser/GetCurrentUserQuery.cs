using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries.GetCurrentUser
{
    public record GetCurrentUserQuery : IRequest<UserDto> { }

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public GetCurrentUserQueryHandler(ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var principal = _currentUserService.Principal ?? throw new UnauthorizedAccessException();

            var userInfo = await _identityService.GetUserInfoAsync(principal) ?? throw new UnauthorizedAccessException();

            return new UserDto
            {
                Id = userInfo.Id,
                Email = userInfo.Email,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Role = userInfo.Role,
                BirthDate = userInfo.BirthDate,
                RegisteredAt = userInfo.RegisteredAt
            };
        }
    }
}