using FluentValidation;

namespace Application.Features.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty();

            RuleFor(c => c.Token)
                .NotEmpty();
        }
    }
}