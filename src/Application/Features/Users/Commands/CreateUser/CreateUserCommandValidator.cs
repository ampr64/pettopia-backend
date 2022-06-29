using Domain.Enumerations;
using FluentValidation;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IDateTimeService dateTimeService)
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(c => c.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(100);

            RuleFor(c => c.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.LastName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.BirthDate)
                .LessThan(dateTimeService.Now);

            RuleFor(c => c.FostererName)
                .NotEmpty()
                .When(c => c.Role.Equals(Role.Fosterer.Name))
                .MaximumLength(50);

            RuleFor(c => c.Role)
                .NotEmpty()
                .Must(BeValidRole)
                .WithMessage("Invalid role.");
        }

        private bool BeValidRole(string role)
        {
            return Role.TryFromName(role, true, out _);
        }
    }
}