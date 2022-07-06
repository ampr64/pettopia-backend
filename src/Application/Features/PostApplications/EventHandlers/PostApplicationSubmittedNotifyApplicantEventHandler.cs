using Domain.Entities.Users;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PostApplications.EventHandlers
{
    public class PostApplicationSubmittedNotifyApplicantEventHandler : INotificationHandler<PostApplicationSubmittedEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailSender _emailSender;

        public PostApplicationSubmittedNotifyApplicantEventHandler(IApplicationDbContext dbContext, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _emailSender = emailSender;
        }

        public async Task Handle(PostApplicationSubmittedEvent notification, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts
                .AsNoTracking()
                .Include(p => p.Author)
                .SingleAsync(p => p.Id == notification.Application.PostId, cancellationToken);

            var authorName = (post.Author as Fosterer)?.OrganizationName ?? post.Author.FirstName;

            var recipientEmail = notification.Application.ApplicantInfo.Email;

            var subject = $"Application submitted.";

            var body = $"Hi, <b>{notification.Application.ApplicantInfo.Name}</b>!"
                + $"<br>Your application for {post.PetName} has been submitted. {post.Author.FirstName} will contact you shortly.";

            await _emailSender.SendAsync(notification.Application.ApplicantInfo.Email, subject, body, cancellationToken);
        }
    }
}