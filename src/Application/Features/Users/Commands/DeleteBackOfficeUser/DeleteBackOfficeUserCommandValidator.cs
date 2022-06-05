using FluentValidation;

namespace Application.Features.Users.Commands.DeleteUser
{
    public class DeleteBackOfficeUserCommandValidator: AbstractValidator<DeleteBackOfficeUserCommand>
    {
        public DeleteBackOfficeUserCommandValidator()
        {
            RuleFor(q => q.Id)
              .NotEmpty();
        }
    }
}
