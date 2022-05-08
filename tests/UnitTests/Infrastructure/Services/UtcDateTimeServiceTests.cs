using Infrastructure.Services;

namespace UnitTests.Infrastructure.Services
{
    public class UtcDateTimeServiceTests
    {
        [Theory, AutoMoqData]
        public void Now_ShouldReturnDateTimeInUtc(UtcDateTimeService sut)
        {
            var actual = sut.Now;

            actual.Should().BeIn(DateTimeKind.Utc);
        }
    }
}