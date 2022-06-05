using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Users.Commands.UpdateUser;
using MediatR;

namespace UnitTests.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandTest
    {
        [Theory, AutoMoqData]
        public async Task Handle_ShouldReturnUnitValue([Frozen] Mock<IIdentityService> identityServiceMock,
           UpdateUserCommandHandler sut,
           UpdateUserCommand command,
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
