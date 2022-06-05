using Application.Common.Interfaces;
using Domain.Enumerations;
using FluentValidation;

namespace Application.Features.Users.Commands.CreateBackOfficeUser
{
    public class CreateBackOfficeUserCommandValidator : AbstractValidator<CreateBackOfficeUserCommand>
    {
        public CreateBackOfficeUserCommandValidator(IDateTimeService dateTimeService)
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

        }

    }
}
