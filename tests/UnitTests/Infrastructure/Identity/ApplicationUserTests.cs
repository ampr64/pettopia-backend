using Application.Common.Interfaces;
using Infrastructure.Identity;

namespace UnitTests.Infrastructure.Identity
{
    public class ApplicationUserTests
    {
        [Theory, AutoMoqData]
        public void ConstructorForNewUser_ShouldSetRegisteredAtToCurrentDate([Frozen] Mock<IDateTimeService> dateTimeServiceMock,
            DateTime expectedDate,
            string email,
            string firstName,
            string lastName,
            DateTime birthDate)
        {
            dateTimeServiceMock
                .Setup(d => d.Now)
                .Returns(expectedDate);

            var newUser = new ApplicationUser(email, firstName, lastName, birthDate, dateTimeServiceMock.Object);

            dateTimeServiceMock.Verify(d => d.Now);
            newUser.RegisteredAt.Should().Be(expectedDate);
        }

        [Theory, AutoMoqData]
        public void Constructor_ShouldSetPropertiesCorrectly(string email,
            string firstName,
            string lastName,
            DateTime birthDate)
        {
            var user = new ApplicationUser(email, firstName, lastName, birthDate);

            user.Email.Should().Be(email);
            user.UserName.Should().Be(email);
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
            user.BirthDate.Should().Be(birthDate);
        }

        [Theory, AutoMoqData]
        public void Constructor_ShouldInitializeIdWithGuidString(string email,
            string firstName,
            string lastName,
            DateTime birthDate)
        {
            var user = new ApplicationUser(email, firstName, lastName, birthDate);
            var parseResult = Guid.TryParse(user.Id, out var userIdGuid);

            parseResult.Should().BeTrue();
            userIdGuid.Should().Be(user.Id);
        }
    }
}