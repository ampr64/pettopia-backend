using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace UnitTests.Configuration
{
    internal class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() =>
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            fixture.Customizations.Add(new LongEmailRelay());

            return fixture;
        })
        {
        }
    }
}