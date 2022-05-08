using Application.Features.Users.Commands.CreateUser;
using FluentValidation.TestHelper;

namespace UnitTests.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandValidatorTests
    {
        [Theory]
        [AutoMoqInlineData(null!)]
        [AutoMoqInlineData("")]
        [AutoMoqInlineData("   ")]
        public void Validate_ShouldHaveError_WhenEmailIsEmpty(string? email, CreateUserCommandValidator sut, CreateUserCommand command)
        {
            command.Email = email!;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorCode("NotEmptyValidator");
        }

        [Theory]
        [AutoMoqInlineData(".com")]
        [AutoMoqInlineData("Theory.com")]
        [AutoMoqInlineData("Theory@.com@")]
        [AutoMoqInlineData("Theory.com@")]
        [AutoMoqInlineData("@.com")]
        [AutoMoqInlineData("@")]
        [AutoMoqInlineData("a@")]
        public void Validate_ShouldHaveErrorForEmail_WhenAtSignIsAtBeginningOrEndOrIsNotPresent(string email, CreateUserCommandValidator sut, CreateUserCommand command)
        {
            command.Email = email;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorCode("EmailValidator");
        }
    }
}