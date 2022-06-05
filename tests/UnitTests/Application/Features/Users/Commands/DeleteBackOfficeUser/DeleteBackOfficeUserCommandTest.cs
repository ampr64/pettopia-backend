using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Users.Commands.DeleteUser;
using MediatR;
using static Application.Features.Users.Commands.DeleteUser.DeleteBackOfficeUserCommand;

namespace UnitTests.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteBackOfficeUserCommandTest
    {
        [Theory, AutoMoqData]
        public async Task Handle_ShouldReturnUnitValue([Frozen] Mock<IIdentityService> identityServiceMock,
           DeleteBackOfficeUserCommandHandler sut,
           DeleteBackOfficeUserCommand command,
           UserInfo? userInfo
           )
        {
            identityServiceMock
                .Setup(i => i.GetUserInfoByIdAsync(command.Id))
                .ReturnsAsync(userInfo);

            var actual = await sut.Handle(command, default);


            actual.Should().Be(Unit.Value);

        }
    }
}
