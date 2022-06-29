using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Authentication.Commands.Authenticate;

namespace UnitTests.Application.Features.Authentication.Commands.Authenticate
{
    public class AuthenticateCommandTests
    {
        [Theory, AutoMoqData]
        public async Task Handle_ThrowsAuthenticationFailedException_WhenAuthenticationFails([Frozen] Mock<IIdentityService> identityServiceMock,
            AuthenticateCommandHandler sut,
            AuthenticateCommand command)
        {
            identityServiceMock
                .Setup(i => i.AuthenticateAsync(command.Email, command.Password))
                .ReturnsAsync(value: null);

            await sut.Invoking(s => s.Handle(command, default))
                .Should()
                .ThrowAsync<AuthenticationFailedException>();

            identityServiceMock.Verify(i => i.AuthenticateAsync(command.Email, command.Password));
        }

        [Theory(Skip = "TODO Refactor"), AutoMoqData]
        public async Task Handle_ReturnsCorrectInformation_WhenAuthenticationSucceeds([Frozen] Mock<IIdentityService> identityServiceMock,
            AuthenticateCommandHandler sut,
            AuthenticateCommand command,
            UserInfo userInfo,
            string token)
        {
            identityServiceMock
                .Setup(i => i.AuthenticateAsync(command.Email, command.Password))
                .ReturnsAsync(token);

            var actual = await sut.Handle(command, default);

            identityServiceMock.Verify(i => i.AuthenticateAsync(command.Email, command.Password));

            actual.Should().NotBeNull();
            actual.Id.Should().Be(userInfo.Id);
            actual.Email.Should().Be(userInfo.Email);
            actual.FirstName.Should().Be(userInfo.FirstName);
            actual.LastName.Should().Be(userInfo.LastName);
            actual.Role.Should().Be(userInfo.Role);
            actual.Token.Should().Be(token);
        }
    }
}