using Application.Common.Models;

namespace UnitTests.Application.Common
{
    public class ResultTests
    {
        [Fact]
        public void Succeeded_ShouldBeTrue_WhenSuccess()
        {
            var sut = Result<int>.Success(1);

            sut.Succeeded.Should().BeTrue();
        }

        [Theory, AutoMoqData]
        public void Data_ShouldBeStored_WhenSuccess(string data)
        {
            var sut = Result<string>.Success(data);

            sut.Data.Should().Be(data);
        }

        [Fact]
        public void Success_ThrowsArgumentNullException_WhenDataIsNull()
        {
            var actual = FluentActions.Invoking(() => Result<int?>.Success(null));

            actual.Should().Throw<ArgumentNullException>();
        }

        [Theory, AutoMoqData]
        public void Succeeded_ShouldBeFalse_WhenFailure(IEnumerable<string> errors)
        {
            var sut = Result<int?>.Failure(errors);

            sut.Succeeded.Should().BeFalse();
        }

        [Fact]
        public void Failure_ShouldThrowArgumentNullException_WhenErrorsIsNull()
        {
            var actual = FluentActions.Invoking(() => Result<string?>.Failure(null!));

            actual.Should().Throw<ArgumentNullException>();
        }
    }
}