using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Users.Queries.GetCurrentUser;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using System.Security.Claims;
using UnitTests.Configuration;
using Xunit;

namespace UnitTests.Application.Features.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryTests
    {
        [Theory, AutoMoqData]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenPrincipalIsNull([Frozen] Mock<ICurrentUserService> currentUserServiceMock,
            GetCurrentUserQueryHandler sut,
            GetCurrentUserQuery query)
        {
            currentUserServiceMock
                .Setup(c => c.Principal)
                .Returns(value: null);

            var handle = sut.Invoking(s => s.Handle(query, default));

            await handle.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Theory, AutoMoqData]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenIdentityServiceReturnsNull([Frozen] Mock<ICurrentUserService> currentUserServiceMock,
            [Frozen] Mock<IIdentityService> identityServiceMock,
            GetCurrentUserQueryHandler sut,
            GetCurrentUserQuery query,
            ClaimsPrincipal principal)
        {
            currentUserServiceMock
                .Setup(c => c.Principal)
                .Returns(principal);

            identityServiceMock
                .Setup(i => i.GetUserInfoAsync(principal))
                .ReturnsAsync(value: null);

            var handle = sut.Invoking(s => s.Handle(query, default));

            await handle.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Theory, AutoMoqData]
        public async Task Handle_ShouldReturnUserDto_WhenUserInfoCouldBeRetrieved([Frozen] Mock<ICurrentUserService> currentUserServiceMock,
            [Frozen] Mock<IIdentityService> identityServiceMock,
            GetCurrentUserQueryHandler sut,
            GetCurrentUserQuery query,
            ClaimsPrincipal principal,
            UserInfo userInfo)
        {
            currentUserServiceMock
                .Setup(c => c.Principal)
                .Returns(principal);

            identityServiceMock
                .Setup(i => i.GetUserInfoAsync(principal))
                .ReturnsAsync(userInfo);

            var userDto = await sut.Handle(query, default);

            userDto.Should().NotBeNull();
            userDto.Should().BeEquivalentTo(userInfo);
        }
    }
}