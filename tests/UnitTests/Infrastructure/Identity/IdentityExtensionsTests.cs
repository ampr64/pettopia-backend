using Infrastructure.Identity;

namespace UnitTests.Infrastructure.Identity
{
    public class IdentityExtensionsTests
    {
        [Theory, AutoData]
        public void ToUserInfo_ShouldMapAllPropertiesCorrectly(ApplicationUser sut, string role)
        {
            var actual = sut.ToUserInfo(role);

            actual.Email.Should().Be(sut.Email);
            actual.FirstName.Should().Be(sut.FirstName);
            actual.LastName.Should().Be(sut.LastName);
            actual.Id.Should().Be(sut.Id);
            actual.Role.Should().Be(role);
        }

        [Fact]
        public void ToUserInfo_ShouldThrowArgumentNullException_WhenApplicationUserIsNull()
        {
            ApplicationUser? applicationUser = null;

            var toUserInfo = FluentActions.Invoking(() => applicationUser!.ToUserInfo(It.IsAny<string>()));

            toUserInfo.Should().Throw<ArgumentNullException>();
        }
    }
}