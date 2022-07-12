using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PostApplications.EventHandlers
{
    public class PostApplicationCanceledEventHandler : INotificationHandler<PostApplicationCanceledEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailSender _emailSender;

        public PostApplicationCanceledEventHandler(IApplicationDbContext dbContext, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _emailSender = emailSender;
        }

        public async Task Handle(PostApplicationCanceledEvent notification, CancellationToken cancellationToken)
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

            var subject = "Your application has been canceled.";

            var body = $"Hi, <b>{applicationInfo.Applicant.FirstName}<b>!"
                + $"<br>Your application for {applicationInfo.PetName} has been canceled.";

            await _emailSender.SendAsync(applicationInfo.Applicant.Email, subject, body, cancellationToken);
        }
    }
}