using Application.Features.Authentication.Commands.Authenticate;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;
using UnitTests.Configuration;
using Xunit;

namespace UnitTests.Application.Features.Authentication.Commands.Authenticate
{
    public class AuthenticateCommandValidatorTests
    {
        [Theory]
        [InlineAutoData(null)]
        [InlineAutoData("")]
        [InlineAutoData("  ")]
        [InlineAutoData("       ")]
        public void Validate_ShouldHaveError_WhenPasswordIsEmpty(string password, AuthenticateCommandValidator sut, AuthenticateCommand command)
        {
            command.Password = password;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Password)
                .WithErrorCode("NotEmptyValidator");
        }

        [Theory]
        [InlineAutoData("")]
        [InlineAutoData("a")]
        [InlineAutoData("aa")]
        [InlineAutoData("aaa")]
        public void Validate_ShouldHaveError_WhenPasswordLengthIsLessThanMinimum(string shortPassword, AuthenticateCommandValidator sut, AuthenticateCommand command)
        {
            command.Password = shortPassword;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Password)
                .WithErrorCode("MinimumLengthValidator");
        }

        [Theory, AutoMoqData]
        public void Validate_ShouldHaveError_WhenPasswordLengthIsGreaterThanMaximum(string longPassword, AuthenticateCommandValidator sut, AuthenticateCommand command)
        {
            command.Password = longPassword;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Password)
                .WithErrorCode("MaximumLengthValidator");
        }

        [Theory]
        [InlineAutoData(null)]
        [InlineAutoData("")]
        [InlineAutoData("   ")]
        public void Validate_ShouldHaveError_WhenEmailIsEmpty(string email, AuthenticateCommandValidator sut, AuthenticateCommand command)
        {
            command.Email = email;
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
        public void Validate_ShouldHaveErrorForEmail_WhenAtSignIsAtBeginningOrEndOrIsNotPresent(string email, AuthenticateCommandValidator sut, AuthenticateCommand command)
        {
            command.Email = email;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorCode("EmailValidator");
        }
    }
}