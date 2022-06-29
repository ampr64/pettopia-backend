using Application.Features.Users.Commands.CreateUser;

namespace UnitTests.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandValidatorTests
    {
        [Theory]
        [AutoMoqInlineData(null!)]
        [AutoMoqInlineData("")]
        [AutoMoqInlineData("   ")]
        public void Validate_ShouldHaveError_WhenEmailIsEmpty(string? email,
            CreateUserCommandValidator sut,
            CreateUserCommand command)
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
            CreateUserCommandValidator sut,
            CreateUserCommand command)
        {
            command.Email = email;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorCode("EmailValidator");
        }

        [Theory(Skip = "TODO Refactor")]
        [AutoMoqInlineData(null!)]
        [AutoMoqInlineData("")]
        [AutoMoqInlineData("   ")]
        public void Validate_ShouldHaveError_WhenRoleIsEmpty(string? role,
            CreateUserCommandValidator sut,
            CreateUserCommand command)
        {
            command.Role = role!;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Role)
                .WithErrorCode("NotEmptyValidator");
        }

        [Theory, AutoMoqData]
        public void Validate_ShouldHaveError_WhenRoleIsInvalid(CreateUserCommandValidator sut,
            CreateUserCommand command)
        {
            command.Role = "rolethatdoesnotexist";
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Role);
        }
    }
}