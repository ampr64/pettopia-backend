using AutoFixture.Xunit2;

namespace UnitTests.Configuration
{
    public class AutoMoqInlineDataAttribute : InlineAutoDataAttribute
    {
        public AutoMoqInlineDataAttribute(params object[] values)
            : base(new AutoMoqDataAttribute(), values)
        {
        }
    }
}