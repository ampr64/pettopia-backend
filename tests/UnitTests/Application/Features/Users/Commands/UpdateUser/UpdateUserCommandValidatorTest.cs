using Application.Features.Users.Commands.UpdateUser;

namespace UnitTests.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidatorTest
    {
        [Theory]
        [AutoMoqInlineData(null!)]
        [AutoMoqInlineData("")]
        [AutoMoqInlineData("   ")]
        public void Validate_ShouldHaveError_WhenIdIsEmpty(string? id, UpdateUserCommandValidator sut, UpdateUserCommand command)
        {
            command.Id = id!;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Id)
                .WithErrorCode("NotEmptyValidator");
        }

        [Theory]
        [AutoMoqInlineData(null!)]
        [AutoMoqInlineData("")]
        [AutoMoqInlineData("   ")]
        public void Validate_ShouldHaveError_WhenEmailIsEmpty(string? email, UpdateUserCommandValidator sut, UpdateUserCommand command)
        {
            command.Email = email!;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorCode("NotEmptyValidator");
        }

        [Theory]
        [AutoMoqInlineData(null!)]
        [AutoMoqInlineData("")]
        [AutoMoqInlineData("   ")]
        public void Validate_ShouldHaveError_WhenFirstNameIsEmpty(string? firstname, UpdateUserCommandValidator sut, UpdateUserCommand command)
        {
            command.FirstName = firstname!;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.FirstName)
                .WithErrorCode("NotEmptyValidator");
        }

        [Theory]
        [AutoMoqInlineData(null!)]
        [AutoMoqInlineData("")]
        [AutoMoqInlineData("   ")]
        public void Validate_ShouldHaveError_WhenLastNameIsEmpty(string? lastname, UpdateUserCommandValidator sut, UpdateUserCommand command)
        {
            command.LastName = lastname!;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.LastName)
                .WithErrorCode("NotEmptyValidator");
        }

        [Theory]
        [AutoMoqInlineData(null!)]
        [AutoMoqInlineData("")]
        [AutoMoqInlineData("   ")]
        public void Validate_ShouldHaveError_WhenBirthDateIsEmpty(DateTime birthdate, UpdateUserCommandValidator sut, UpdateUserCommand command)
        {
            command.BirthDate = birthdate;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.BirthDate)
                .WithErrorCode("NotEmptyValidator");
        }
    }
}
