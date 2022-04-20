using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace UnitTests.Infrastructure.Identity
{
    public class IdentityServiceTests
    {
        [Test]
        [Ignore("Needs fix for dependency injection fixture")]
        public async Task GetUserInfo_ShouldReturnNull_IfUserDoesNotExist()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var email = "nonexistantuser@test.com";
            var userManagerMock = fixture.Freeze<Mock<UserManager<ApplicationUser>>>();
            userManagerMock
                .Setup(u => u.FindByEmailAsync(email))!
                .ReturnsAsync(value: null);

            var sut = fixture.Create<IdentityService>();

            var actual = await sut.GetUserInfoAsync(email);

            actual.Should().BeNull();
            userManagerMock.Verify(u => u.FindByEmailAsync(email));
        }

        [Test]
        [Ignore("Needs fix for dependency injection fixture")]
        public async Task Authenticate_ShouldReturnNull_IfUserDoesNotExist()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var email = "nonexistantuser@test.com";
            var userManagerMock = fixture.Freeze<Mock<UserManager<ApplicationUser>>>();
            userManagerMock
                .Setup(u => u.FindByEmailAsync(email))!
                .ReturnsAsync(value: null);

            var sut = fixture.Create<IdentityService>();

            var actual = await sut.GetUserInfoAsync(email);

            actual.Should().BeNull();
        }
    }
}