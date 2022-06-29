using Domain.Entities.Users;
using Domain.Enumerations;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? FostererName { get; set; }

        public string Role { get; set; }

        public DateTime BirthDate { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IDateTimeService _dateTimeService;

        public CreateUserCommandHandler(IApplicationDbContext dbContext, IIdentityService identityService, IDateTimeService dateTimeService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _dateTimeService = dateTimeService;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var role = Role.FromName(request.Role, true);
            var currentUser = await _identityService.GetCurrentUserAsync(false, cancellationToken);

            if (role == Role.Admin || (role == Role.Admin && currentUser is not Administrator)) throw new ForbiddenAccessException();

            var result = await _identityService.CreateUserAsync(request.Email, request.Password, role, cancellationToken);

            if (!result.Succeeded) throw new UnprocessableEntityException("User could not be created.", result.Errors);

            Member user = CreateMember(request, role, result.Data);

            await _dbContext.Members.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result.Data;
        }

        private Member CreateMember(CreateUserCommand request, Role role, string id)
        {
            Member user;
            if (role == Role.BackOfficeUser)
            {
                user = new BackOfficeUser(id, request.FirstName, request.LastName, request.Email, request.BirthDate, _dateTimeService.Now);
            }
            else if (role == Role.Fosterer)
            {
                user = new Fosterer(id, request.FostererName!, request.FirstName, request.LastName, request.Email, request.BirthDate, _dateTimeService.Now);
            }
            else
            {
                user = new EndUser(id, request.FirstName, request.LastName, request.Email, request.BirthDate, _dateTimeService.Now);
            }

            return user;
        }
    }
}