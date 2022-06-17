using Microsoft.AspNetCore.Http;
using WebApi.Services;

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

        [Theory, AutoMoqData]
        public void UserId_ShouldReturnNull_WhenHttpContextIsNull([Frozen] Mock<IHttpContextAccessor> httpContextAccessorMock,
            CurrentUserService sut)
        {
            httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(value: null);

            var userId = sut.Invoking(s => s.UserId);

            httpContextAccessorMock.Verify(x => x.HttpContext);

            userId.Should().NotThrow<NullReferenceException>();
            userId.Invoke().Should().BeNull();
        }
    }
}