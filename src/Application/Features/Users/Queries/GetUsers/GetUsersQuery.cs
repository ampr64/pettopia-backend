using Application.Common.Interfaces;
using Application.Common.Translators;
using MediatR;

namespace Application.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<IReadOnlyList<UserDto>>
    {
        public string Role { get; set; } = null!;
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyList<UserDto>>
    {
        private readonly IIdentityService _identityService;

        public GetUsersQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<IReadOnlyList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var usersInfo = await _identityService.GetUsersByRole(request.Role);
            var result = UserInfoToUserDto.ToUserDto(usersInfo);

            return result;
        }
    }
}
