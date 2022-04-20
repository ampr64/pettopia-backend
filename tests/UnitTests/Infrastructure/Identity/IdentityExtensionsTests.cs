using FluentAssertions;
using Infrastructure.Identity;
using Moq;
using NUnit.Framework;
namespace UnitTests.Infrastructure.Identity
{
    public class IdentityExtensionsTests
    {
        [Test]
        public void ToUserInfo_ShouldMapAllPropertiesCorrectly()
        {
            var role = "User";
            var applicationUserStub = new ApplicationUser("appuser@admin.com", "Test", "Last")
            {
                Id = "123",
            };

            var actual = applicationUserStub.ToUserInfo(role);

            actual.Email.Should().Be(applicationUserStub.Email);
            actual.FirstName.Should().Be(applicationUserStub.FirstName);
            actual.LastName.Should().Be(applicationUserStub.LastName);
            actual.Id.Should().Be(applicationUserStub.Id);
            actual.Role.Should().Be(role);
        }

        [Test]
        public void ToUserInfo_ShouldThrowArgumentNullException_WhenApplicationUserIsNull()
        {
            ApplicationUser? applicationUser = null;

            var toUserInfo = FluentActions.Invoking(() => applicationUser!.ToUserInfo(It.IsAny<string>()));

            toUserInfo.Should().Throw<ArgumentNullException>();
        }
    }
}