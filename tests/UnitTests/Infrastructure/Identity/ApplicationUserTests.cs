using Infrastructure.Identity;

namespace UnitTests.Infrastructure.Identity
{
    public class CustomIdentityUserTests
    {
        [Theory, AutoMoqData]
        public void Constructor_ShouldSetPropertiesCorrectly(string email)
        {
            var user = new CustomIdentityUser(email);

            user.Email.Should().Be(email);
            user.UserName.Should().Be(email);
        }

        [Theory, AutoMoqData]
        public void Constructor_ShouldInitializeIdWithGuidString(string email)
        {
            var user = new CustomIdentityUser(email);
            var parseResult = Guid.TryParse(user.Id, out var userIdGuid);

            parseResult.Should().BeTrue();
            userIdGuid.Should().Be(user.Id);
        }
    }
}