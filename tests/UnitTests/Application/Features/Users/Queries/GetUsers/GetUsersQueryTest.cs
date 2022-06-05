using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Users.Queries;

namespace UnitTests.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQueryTest
    {
        [Theory, AutoMoqData]
        public async Task Handle_ShouldReturnAListOfUsers([Frozen] Mock<IIdentityService> identityServiceMock,
            GetUsersQueryHandler sut,
            GetUsersQuery query,
            IReadOnlyList<UserInfo?> userInfos
            )
        {
            identityServiceMock
                .Setup(i => i.GetUsersByRoleAsync(query.Role))
                .ReturnsAsync(userInfos);

            var actual = await sut.Handle(query, default);

            identityServiceMock.Verify(i => i.GetUsersByRoleAsync(query.Role));

            actual.Should().NotBeNull();
        }
    }
}
