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
                .MaximumLength(256);

            RuleFor(_ => _.Password)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}