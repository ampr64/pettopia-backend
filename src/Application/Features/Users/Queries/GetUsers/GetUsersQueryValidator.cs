using Domain.Enumerations;
using FluentValidation;

namespace Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleForEach(q => q.Role)
                .Must(BeValidRole)
                .When(q => q.Role is { Count: > 0 })
                .WithMessage("{PropertyValue} is not a valid {PropertyName}.");
        }

        private bool BeValidRole(string role)
        {
            return Role.TryFromName(role, true, out _);
        }
    }
}