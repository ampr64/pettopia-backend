using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator(IDateTimeService dateTimeService)
        {
            RuleFor(c => c.Id)
                .NotEmpty();

            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress()
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
