using Domain.Common;
using Domain.Entities.Posts;
using Domain.Enumerations;
using Domain.Events;
using Domain.ValueObjects;
using UnitTests.TestExtensions;

namespace UnitTests.Domain.Entities.Posts
{
    public class PostTests
    {
        [Theory, AutoMoqData]
        public void AddApplication_ShouldRaisePostApplicationSubmittedEvent(DateTime submitDate,
            string applicantId,
            string applicantName,
            string applicantEmail,
            PhoneNumber applicantPhoneNumber)
        {
            var sut = CreateEmptyPost();

            var applicationId = sut.AddApplication(submitDate, applicantId, applicantName, applicantEmail, applicantPhoneNumber);

            sut.DomainEvents
                .OfType<PostApplicationSubmittedEvent>()
                .Should()
                .NotBeEmpty()
                .And
                .Subject
                .Should()
                .ContainSingle(e => e.Application.Id == applicationId);
        }

        [Theory]
        [AutoMoqInlineData(nameof(PostStatus.Closed))]
        [AutoMoqInlineData(nameof(PostStatus.Completed))]
        public void AddApplication_ShouldThrow_WhenPostIsNotOpen(string statusName)
        {
            var status = PostStatus.FromName(statusName);
            var sut = CreateEmptyPost();

            sut.SetProperty(p => p.Status, status);

            var actual = FluentActions.Invoking(() => sut.AddApplication(
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<PhoneNumber?>()));

            actual.Should().Throw<DomainException>();
        }

        [Theory, AutoMoqData]
        public void Close_ShouldSetStatusToClosed(List<PostApplication> applications)
        {
            var sut = CreateEmptyPost();
            sut.SetFieldByConvention(p => p.Applications, applications);

            sut.Close(It.IsAny<DateTime>());

            sut.Status.Should().Be(PostStatus.Closed);
        }

        [Theory, AutoMoqData]
        public void Close_ShouldCancelPendingApplications(List<PostApplication> applications)
        {
            var post = CreateEmptyPost();
            post.SetFieldByConvention(p => p.Applications, applications);

            post.Close(It.IsAny<DateTime>());

            post.Applications.Should().OnlyContain(a => a.Status == ApplicationStatus.Canceled);
        }

        [Theory, AutoMoqData]
        public void Complete_ShouldAcceptApplication(List<PostApplication> applications)
        {
            var acceptedApplication = applications.First();
            var sut = CreateEmptyPost();
            sut.SetFieldByConvention(p => p.Applications, applications);

            sut.Complete(acceptedApplication, It.IsAny<DateTime>());

            acceptedApplication.Status.Should().Be(ApplicationStatus.Accepted);
        }

        [Theory, AutoMoqData]
        public void Complete_ShouldRejectPendingApplications(List<PostApplication> applications)
        {
            var acceptedApplication = applications.First();
            var sut = CreateEmptyPost();
            sut.SetFieldByConvention(p => p.Applications, applications);

            sut.Complete(acceptedApplication, It.IsAny<DateTime>());

            sut.Applications.Should().ContainSingle(a => a.Status == ApplicationStatus.Accepted);
            sut.Applications.Skip(1).Should().OnlyContain(a => a == acceptedApplication || a.Status == ApplicationStatus.Rejected);
        }

        private static Post CreateEmptyPost()
        {
            return (Post)Activator.CreateInstance(typeof(Post), true)!;
        }
    }
}