using Domain.Common;

namespace UnitTests.Domain.Common
{
    public class EntityTests
    {
        [Theory, AutoMoqData]
        public void Equals_ShouldReturnTrue_WhenBothHaveSameTypeAndId(int id)
        {
            var entityA = new DummyEntityIntId(id);
            var entityB = new DummyEntityIntId(id);

            var actual = entityA.Equals(entityB);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenBothHaveReferenceEquality()
        {
            int id = default;
            var entityA = new DummyEntityIntId(id);
            var entityB = entityA;

            var actual = entityA.Equals(entityB);

            actual.Should().BeTrue();
        }

        [Theory, AutoMoqData]
        public void EqualsObj_ShouldReturnTrue_WhenBothHaveSameTypeAndId(int id)
        {
            var entityA = new DummyEntityIntId(id);
            object entityB = new DummyEntityIntId(id);

            var actual = entityA.Equals(entityB);

            actual.Should().BeTrue();
        }

        [Theory, AutoMoqData]
        public void Equals_ShouldReturnFalse_WhenOtherIsNull(string id)
        {
            var entityA = new DummyEntityStringId(id);
            DummyEntityStringId? entityB = null;

            var actual = entityA.Equals(entityB);

            actual.Should().BeFalse();
        }

        [Theory, AutoMoqData]
        public void Equals_ShouldReturnFalse_WhenTypesDiffer(string id)
        {
            var entityA = new DummyEntityStringId(id);
            var entityB = new DummyEntityStringId2(id);

            var actual = entityA.Equals(entityB);

            actual.Should().BeFalse();
        }

        [Theory, AutoMoqData]
        public void Equals_ShouldReturnFalse_WhenLeftIsTransient(string id)
        {
            var entityA = new DummyEntityStringId(default!);
            var entityB = new DummyEntityStringId(id);

            var actual = entityA.Equals(entityB);

            actual.Should().BeFalse();
        }

        [Theory, AutoMoqData]
        public void Equals_ShouldReturnFalse_WhenRightIsTransient(string id)
        {
            var entityA = new DummyEntityStringId(id);
            var entityB = new DummyEntityStringId(default!);

            var actual = entityA.Equals(entityB);

            actual.Should().BeFalse();
        }

        [Fact]
        public void EqualOperator_ShouldReturnTrue_WhenBothAreNull()
        {
            DummyEntityIntId? entityA = null;
            DummyEntityIntId? entityB = null;

            var actual = entityA == entityB;

            actual.Should().BeTrue();
        }

        [Theory, AutoMoqData]
        public void NotEqualOperator_ShouldReturnOppositeOfEqualOperator(int id)
        {
            var entityA = new DummyEntityIntId(id);
            var entityB = new DummyEntityIntId(id);

            var equals = entityA == entityB;
            var actual = entityA != entityB;

            actual.Should().NotBe(equals);
        }

        [Theory, AutoMoqData]
        public void GetHashCode_ShouldReturnSameValue_GivenSameId(int id)
        {
            var entityA = new DummyEntityIntId(id);
            var entityB = new DummyEntityIntId(id);

            var hashCodeA = entityA.GetHashCode();
            var hashCodeB = entityB.GetHashCode();

            hashCodeB.Should().Be(hashCodeA);
        }

        private class DummyEntityIntId : Entity<int>
        {
            public DummyEntityIntId(int id) => Id = id;
        }

        private class DummyEntityStringId : Entity<string>
        {
            public DummyEntityStringId(string id) => Id = id;
        }

        private class DummyEntityStringId2 : Entity<string>
        {
            public DummyEntityStringId2(string id) => Id = id;
        }
    }
}