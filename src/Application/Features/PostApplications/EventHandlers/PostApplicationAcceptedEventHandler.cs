using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PostApplications.EventHandlers
{
    public class PostApplicationAcceptedEventHandler : INotificationHandler<PostApplicationAcceptedEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailSender _emailSender;

        public PostApplicationAcceptedEventHandler(IApplicationDbContext dbContext, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _emailSender = emailSender;
        }

        public async Task Handle(PostApplicationAcceptedEvent notification, CancellationToken cancellationToken)
        {
            var applicationInfo = await _dbContext.Posts
                .AsNoTracking()
                .Where(p => p.Id == notification.Application.PostId)
                .Select(p => new
                {
                    p.PetName,
                    Applicant = _dbContext.Members
                        .AsNoTracking()
                        .FirstOrDefault(m => m.Id == notification.Application.ApplicantId)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (applicationInfo is null || applicationInfo.Applicant is null) return;

            var subject = "Congratulations! Your application has been accepted.";

            var body = $"Hi, <b>{applicationInfo.Applicant.FirstName}<b>!"
                + $"<br>We're pleased to inform you that your application for {applicationInfo.PetName} has been accepted.";

            await _emailSender.SendAsync(applicationInfo.Applicant.Email, subject, body, cancellationToken);
        }
    }
}