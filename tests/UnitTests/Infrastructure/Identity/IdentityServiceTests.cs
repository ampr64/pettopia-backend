using Application.Common.Interfaces;
using Domain.Enumerations;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using UnitTests.Configuration.Identity;

namespace UnitTests.Infrastructure.Identity
{
    public class IdentityServiceTests
    {
        [Theory, IdentityAutoData]
        public async Task Authenticate_ShouldReturnNull_IfUserDoesNotExist([Frozen] Mock<UserManager<ApplicationUser>> userManagerMock,
            IdentityService sut,
            string email)
        {
            userManagerMock
                .Setup(u => u.FindByEmailAsync(email))!
                .ReturnsAsync(value: null);

            var actual = await sut.GetUserInfoAsync(email);

            userManagerMock.Verify(u => u.FindByEmailAsync(email));
            actual.Should().BeNull();
        }

        [Theory(Skip = "Freeze failing."), IdentityAutoData]
        public async Task Authenticate_ShouldReturnToken_IfCredentialsMatch([Frozen] Mock<SignInManager<ApplicationUser>> signInManagerMock,
            [Frozen] Mock<ITokenClaimsService> tokenClaimsServiceMock,
            Mock<UserManager<ApplicationUser>> userManagerStub,
            IdentityService sut,
            ApplicationUser user,
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
        public async Task Authenticate_ShouldReturnNull_IfCredentialsAreInvalid([Frozen] Mock<SignInManager<ApplicationUser>> signInManagerMock,
            Mock<UserManager<ApplicationUser>> userManagerStub,
            IdentityService sut,
            ApplicationUser user,
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
        public async Task CreateUser_ShouldReturnSuccessWithUserId_WhenCreatedSuccessfully([Frozen] Mock<UserManager<ApplicationUser>> userManagerMock,
            IdentityService sut,
            ApplicationUser user)
        {
            var expectedRole = Role.User.Name;
            var password = "User123!";

            userManagerMock
                .Setup(u => u.CreateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock
                .Setup(u => u.AddToRoleAsync(user, expectedRole))
                .ReturnsAsync(IdentityResult.Success);

            var actual = await sut.CreateUserAsync(user.Email, password, user.FirstName, user.LastName, user.BirthDate);

            actual.Succeeded.Should().BeTrue();
            actual.Data.Should().NotBeNull();
        }

        [Theory, IdentityAutoData]
        public async Task GetUserInfo_ShouldReturnNull_IfUserDoesNotExist([Frozen] Mock<UserManager<ApplicationUser>> userManagerMock,
            IdentityService sut,
            string email)
        {
            userManagerMock
                .Setup(u => u.FindByEmailAsync(email))!
                .ReturnsAsync(value: null);

            var actual = await sut.GetUserInfoAsync(email);

            userManagerMock.Verify(u => u.FindByEmailAsync(email));
            actual.Should().BeNull();
        }

        [Theory, IdentityAutoData]
        public async Task GetUserInfo_ShouldThrowInvalidOperationException_IfUserHasMultipleRoles(Mock<UserManager<ApplicationUser>> userManagerStub,
            IdentityService sut,
            ApplicationUser user,
            IList<string> roles,
            string email)
        {
            userManagerStub
                .Setup(u => u.FindByEmailAsync(email))
                .ReturnsAsync(user);

            userManagerStub
                .Setup(u => u.GetRolesAsync(user))
                .ReturnsAsync(roles);

            var getUserInfo = sut.Invoking(s => s.GetUserInfoAsync(email));

            await getUserInfo.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}