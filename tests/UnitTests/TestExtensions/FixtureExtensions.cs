using AutoFixture;
using Moq;

namespace UnitTests.TestExtensions
{
    internal static class FixtureExtensions
    {
        internal static Mock<T> FreezeMoq<T>(this IFixture fixture)
            where T : class
        {
            var mock = new Mock<T>();
            fixture.Register(() => mock.Object);
            return mock;
        }

        internal static Mock<T> FreezeMoq<T>(this IFixture fixture, Func<Mock<T>> mockFactory)
            where T : class
        {
            return fixture.Freeze<Mock<T>>(builder => builder.FromFactory(() =>
            {
                var mock = mockFactory();
                fixture.Register(() => mock.Object);
                return mock;
            }));
        }
    }
}