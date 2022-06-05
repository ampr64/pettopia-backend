using Application.Features.Users.Commands.CreateBackOfficeUser;

namespace UnitTests.Application.Features.Users.Commands.CreateBackOfficeUser
{
    public class CreateBackOfficeUserCommandValidatorTest
    {
        [Theory]
        [AutoMoqInlineData(null!)]
        [AutoMoqInlineData("")]
        [AutoMoqInlineData("   ")]
        public void Validate_ShouldHaveError_WhenEmailIsEmpty(string? email,
               CreateBackOfficeUserCommandValidator sut,
               CreateBackOfficeUserCommand command)
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
        public void Validate_ShouldHaveErrorForEmail_WhenAtSignIsAtBeginningOrEndOrIsNotPresent(string email,
               CreateBackOfficeUserCommandValidator sut,
               CreateBackOfficeUserCommand command)
        {
            command.Email = email;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorCode("EmailValidator");
        }

    }
}
