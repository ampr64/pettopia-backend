using FluentValidation;

namespace Application.Features.Authentication.Commands.Authenticate
{
    public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        public AuthenticateCommandValidator()
        {
            RuleFor(_ => _.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(_ => _.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(100);
        }
    }
}