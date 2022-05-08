using FluentValidation;

namespace Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQueryValidator: AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(q => q.Role)
                .NotEmpty();
        }
    }
}
