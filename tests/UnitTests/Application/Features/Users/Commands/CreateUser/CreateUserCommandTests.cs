using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Users.Commands.CreateUser;
using Domain.Enumerations;

namespace UnitTests.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandTests
    {
        [Theory(Skip = "TODO Refactor"), AutoMoqData]
        public async Task Handle_ShouldReturnNewUserId_IfCreateUserSucceeds([Frozen] Mock<IIdentityService> identityServiceMock,
            CreateUserCommandHandler sut,
            CreateUserCommand command,
            Role role,
            string userId)
        {
            command.Role = role.Name;

            identityServiceMock
                .Setup(i => i.CreateUserAsync(command.Email, command.Password, role, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<string?>.Success(userId));

            var actual = await sut.Handle(command, default);

            identityServiceMock.Verify(i => i.CreateUserAsync(command.Email, command.Password, role, It.IsAny<CancellationToken>()));

            actual.Should().NotBeNull();
            actual.Should().Be(userId);
        }

        [Theory]
        [AutoMoqInlineData("Admin")]
        public async Task Handle_ShouldThrowForbiddenAccessException_IfRoleIsForbidden(string forbiddenRole,
            CreateUserCommandHandler sut,
            CreateUserCommand command)
        {
            command.Role = forbiddenRole;
            var handle = sut.Invoking(s => s.Handle(command, default));

            await handle.Should().ThrowAsync<ForbiddenAccessException>();
        }

        [Theory(Skip = "TODO Refactor"), AutoMoqData]
        public async Task Handle_ShouldThrowUnprocessableEntityException_IfCreateUserFails([Frozen] Mock<IIdentityService> identityServiceMock,
            CreateUserCommandHandler sut,
            CreateUserCommand command,
            Role role,
            IEnumerable<string> errors)
        {
            identityServiceMock
                .Setup(i => i.CreateUserAsync(command.Email, command.Password, role, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<string?>.Failure(errors));

            var exception = await sut.Invoking(s => s.Handle(command, default))
                .Should()
                .ThrowAsync<UnprocessableEntityException>();

            exception.WithMessage("User could not be created.");
        }
    }
}