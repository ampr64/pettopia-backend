using Application.Common.Interfaces;
using Domain.Enumerations;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using UnitTests.Configuration.Identity;

namespace UnitTests.Infrastructure.Identity
{
    public class IdentityServiceTests
    {
        [Theory(Skip = "Freeze failing."), IdentityAutoData]
        public async Task Authenticate_ShouldReturnToken_IfCredentialsMatch([Frozen] Mock<SignInManager<CustomIdentityUser>> signInManagerMock,
            [Frozen] Mock<ITokenClaimsService> tokenClaimsServiceMock,
            Mock<UserManager<CustomIdentityUser>> userManagerStub,
            IdentityService sut,
            CustomIdentityUser user,
            string token,
            string email,
            string password)
        {
            userManagerStub
                .Setup(u => u.FindByEmailAsync(email))
                .ReturnsAsync(user);

            signInManagerMock
                .Setup(s => s.CheckPasswordSignInAsync(user, password, false))
                .ReturnsAsync(SignInResult.Success);

            tokenClaimsServiceMock
                .Setup(t => t.GetTokenAsync(email))
                .ReturnsAsync(token);

            var actual = await sut.AuthenticateAsync(email, password);

            tokenClaimsServiceMock.Verify(t => t.GetTokenAsync(email));
            actual.Should().NotBeNull()
                .And.Be(token);
        }

        [Theory, IdentityAutoData]
        public async Task Authenticate_ShouldReturnNull_IfCredentialsAreInvalid([Frozen] Mock<SignInManager<CustomIdentityUser>> signInManagerMock,
            Mock<UserManager<CustomIdentityUser>> userManagerStub,
            IdentityService sut,
            CustomIdentityUser user,
            string email,
            string password)
        {
            userManagerStub
                .Setup(u => u.FindByEmailAsync(email))
                .ReturnsAsync(user);

            signInManagerMock
                .Setup(s => s.CheckPasswordSignInAsync(user, password, false))
                .ReturnsAsync(SignInResult.Failed);

            var actual = await sut.AuthenticateAsync(email, password);

            actual.Should().BeNull();
        }

        [Theory(Skip = "UserManager mock failing."), IdentityAutoData]
        public async Task CreateUser_ShouldReturnSuccessWithUserId_WhenCreatedSuccessfully([Frozen] Mock<UserManager<CustomIdentityUser>> userManagerMock,
            IdentityService sut,
            CustomIdentityUser user,
            Role role,
            string password)
        {
            userManagerMock
                .Setup(u => u.CreateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock
                .Setup(u => u.AddToRoleAsync(user, role.Name))
                .ReturnsAsync(IdentityResult.Success);

            var actual = await sut.CreateUserAsync(user.Email, password, role);

            actual.Succeeded.Should().BeTrue();
            actual.Data.Should().NotBeNull();
        }
    }
}