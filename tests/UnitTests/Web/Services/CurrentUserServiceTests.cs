using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using UnitTests.Configuration;
using WebApi.Services;
using Xunit;

namespace UnitTests.Web.Services
{
    public class CurrentUserServiceTests
    {
        [Theory, AutoMoqData]
        public void Principal_ShouldReturnNull_WhenHttpContextIsNull([Frozen] Mock<IHttpContextAccessor> httpContextAccessorMock,
            CurrentUserService sut)
        {
            httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(value: null);

            var principal = sut.Invoking(s => s.Principal);

            httpContextAccessorMock.Verify(x => x.HttpContext);

            principal.Should().NotThrow<NullReferenceException>();
            principal.Invoke().Should().BeNull();
        }
    }
}