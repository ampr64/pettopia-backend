using Domain.ValueObjects;
using FluentAssertions;
using UnitTests.Configuration;
using Xunit;

namespace UnitTests.Domain.ValueObjects
{
    public class AddressTests
    {
        [Theory, AutoMoqData]
        public void GivenSameValues_ShouldBeEquivalent(string province,
            string city,
            string line1,
            string? line2,
            string zipCode)
        {
            var addressA = new Address(province, city, line1, line2, zipCode);
            var addressB = new Address(province, city, line1, line2, zipCode);

            addressB.Should().Be(addressA);
        }
    }
}