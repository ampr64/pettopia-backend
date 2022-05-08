using Domain.Common;

namespace UnitTests.Domain.Common
{
    public class ValueObjectTests
    {
        [Theory, AutoMoqData]
        public void Equals_ShouldReturnTrue_WhenBothHaveSameTypeAndValues(string value1, int value2)
        {
            var valueObjectA = new DummyValueObject(value1, value2);
            var valueObjectB = new DummyValueObject(value1, value2);

            var actual = valueObjectA.Equals(valueObjectB);

            actual.Should().BeTrue();
        }

        [Theory, AutoMoqData]
        public void EqualsObj_ShouldReturnTrue_WhenBothHaveSameTypeAndValues(string value1, int value2)
        {
            var valueObjectA = new DummyValueObject(value1, value2);
            object valueObjectB = new DummyValueObject(value1, value2);

            var actual = valueObjectA.Equals(valueObjectB);

            actual.Should().BeTrue();
        }

        [Theory, AutoMoqData]
        public void Equals_ShouldReturnFalse_WhenTypesDiffer(string value1, int value2)
        {
            var valueObjectA = new DummyValueObject(value1, value2);
            var valueObjectB = new DummyValueObject2(value1, value2);

            var actual = valueObjectA.Equals(valueObjectB);

            actual.Should().BeFalse();
        }

        [Theory, AutoMoqData]
        public void Equals_ShouldReturnFalse_WhenOtherIsNull(string value1, int value2)
        {
            var valueObjectA = new DummyValueObject(value1, value2);
            DummyValueObject? valueObjectB = null;

            var actual = valueObjectA.Equals(valueObjectB);

            actual.Should().BeFalse();
        }

        [Fact]
        public void EqualOperator_ShouldReturnTrue_WhenBothAreNull()
        {
            ValueObject? valueObjectA = null;
            ValueObject? valueObjectB = null;

            var actual = valueObjectA == valueObjectB;

            actual.Should().BeTrue();
        }

        [Theory, AutoMoqData]
        public void NotEqualOperator_ShouldReturnOppositeOfEqualOperator(string value1, int value2)
        {
            var valueObjectA = new DummyValueObject(value1, value2);
            var valueObjectB = new DummyValueObject(value1, value2);

            var equals = valueObjectA == valueObjectB;
            var actual = valueObjectA != valueObjectB;

            actual.Should().NotBe(equals);
        }

        [Theory, AutoMoqData]
        public void GetHashCode_ShouldReturnSameValue_GivenSameEqualityComponents(string value1, int value2)
        {
            var valueObjectA = new DummyValueObject(value1, value2);
            var valueObjectB = new DummyValueObject(value1, value2);

            var hashCodeA = valueObjectA.GetHashCode();
            var hashCodeB = valueObjectB.GetHashCode();

            hashCodeA.Should().Be(hashCodeB);
        }

        [Theory]
        [AutoMoqInlineData("test", 10)]
        [AutoMoqInlineData("abc", 123)]
        [AutoMoqInlineData("string", 500)]
        public void GetHashCode_ShouldDifferentValue_GivenDifferentEqualityComponents(string value1, int value2)
        {
            var valueObjectA = new DummyValueObject(value1, value2);
            var valueObjectB = new DummyValueObject2(value1, value2);

            var hashCodeA = valueObjectA.GetHashCode();
            var hashCodeB = valueObjectB.GetHashCode();

            hashCodeA.Should().NotBe(hashCodeB);
        }

        [Theory, AutoMoqData]
        public void Clone_ShouldReturnEquivalentValueObject(string value1, int value2)
        {
            var valueObject = new DummyValueObject(value1, value2);
            var clone = valueObject.Clone();

            clone.Should().Be(valueObject);
        }

        private class DummyValueObject : ValueObject
        {
            public string Value1 { get; }

            public int Value2 { get; }

            public DummyValueObject(string value1, int value2) => (Value1, Value2) = (value1, value2);

            protected override IEnumerable<object?> GetEqualityComponents()
            {
                yield return Value1;
                yield return Value2;
            }
        }

        private class DummyValueObject2 : ValueObject
        {
            public string Value1 { get; }

            public int Value2 { get; }

            public DummyValueObject2(string value1, int value2) => (Value1, Value2) = (value1, value2);

            protected override IEnumerable<object?> GetEqualityComponents()
            {
                yield return Value1;
            }
        }
    }
}