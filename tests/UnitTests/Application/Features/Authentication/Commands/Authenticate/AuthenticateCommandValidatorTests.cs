using Application.Features.Authentication.Commands.Authenticate;
using AutoFixture;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System.Linq.Expressions;

namespace UnitTests.Application.Features.Authentication.Commands.Authenticate
{
    public class AuthenticateCommandValidatorTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void Validate_ShouldHaveErrorWhenPasswordIsEmpty(string password)
        {
            SetUpTestForValidationError(c => c.Password, password);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Validate_ShouldHaveErrorWhenEmailIsEmpty(string email)
        {
            SetUpTestForValidationError(c => c.Email, email);
        }

        [Test]
        [TestCase(".com")]
        [TestCase("test.com")]
        [TestCase("test@.com@")]
        [TestCase("test.com@")]
        [TestCase("@.com")]
        [TestCase("@")]
        [TestCase("a@")]
        public void Validate_ShouldHaveErrorForEmail_WhenAtSignIsAtBeginningOrEndOrIsNotPresent(string email)
        {
            SetUpTestForValidationError(c => c.Email, email);
        }

        private static void SetUpTestForValidationError(Expression<Func<AuthenticateCommand, string>> propertySelector, string value)
        {
            var command = new Fixture()
                .Build<AuthenticateCommand>()
                .With(propertySelector, value)
                .Create();
            var validator = new AuthenticateCommandValidator();

            var actual = validator.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(propertySelector);
        }
    }
}