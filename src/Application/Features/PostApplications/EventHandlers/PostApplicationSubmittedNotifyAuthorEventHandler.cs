using Domain.Entities.Users;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PostApplications.EventHandlers
{
    public class PostApplicationSubmittedNotifyAuthorEventHandler : INotificationHandler<PostApplicationSubmittedEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailSender _emailSender;

        public PostApplicationSubmittedNotifyAuthorEventHandler(IApplicationDbContext dbContext, IEmailSender emailSender)
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

            var subject = $"New application received for {post.PetName}";

            var body = $"Hi, <b>{authorName}</b>!"
                + $"<br>There is a new application for {post.PetName}. Below are the applicant's contact details."
                + $"<br>Name: {notification.Application.ApplicantInfo.Name}"
                + $"<br>Email: {notification.Application.ApplicantInfo.Email}"
                + $"<br>Phone number: {notification.Application.ApplicantInfo.PhoneNumber}";

            await _emailSender.SendAsync(notification.Application.ApplicantInfo.Email, subject, body, cancellationToken);
        }
    }
}