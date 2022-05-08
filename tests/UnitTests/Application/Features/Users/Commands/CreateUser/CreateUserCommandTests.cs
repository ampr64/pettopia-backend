using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Users.Commands.CreateUser;

namespace UnitTests.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandTests
    {
        [Theory, AutoMoqData]
        public async Task Handle_ShouldReturnNewUserId_IfCreateUserSucceeds([Frozen] Mock<IIdentityService> identityServiceMock,
            CreateUserCommandHandler sut,
            CreateUserCommand command,
            string userId)
        {
            identityServiceMock
                .Setup(i => i.CreateUserAsync(command.Email, command.Password, command.FirstName, command.LastName, command.BirthDate))
                .ReturnsAsync(Result<string?>.Success(userId));

            var actual = await sut.Handle(command, default);

            identityServiceMock.Verify(i => i.CreateUserAsync(command.Email, command.Password, command.FirstName, command.LastName, command.BirthDate));

            actual.Should().NotBeNull();
            actual.Should().Be(userId);
        }

        [Theory, AutoMoqData]
        public async Task Handle_ShouldThrowUnprocessableEntityException_IfCreateUserFails([Frozen] Mock<IIdentityService> identityServiceMock,
            CreateUserCommandHandler sut,
            CreateUserCommand command,
            IEnumerable<string> errors)
        {
            identityServiceMock
                .Setup(i => i.CreateUserAsync(command.Email, command.Password, command.FirstName, command.LastName, command.BirthDate))
                .ReturnsAsync(Result<string?>.Failure(errors));

            var exception = await sut.Invoking(s => s.Handle(command, default))
                .Should()
                .ThrowAsync<UnprocessableEntityException>();

            exception.WithMessage("User could not be created.");
        }
    }
}