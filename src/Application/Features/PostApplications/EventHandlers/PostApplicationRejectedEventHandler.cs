using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PostApplications.EventHandlers
{
    public class PostApplicationRejectedEventHandler : INotificationHandler<PostApplicationRejectedEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailSender _emailSender;

        public PostApplicationRejectedEventHandler(IApplicationDbContext dbContext, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _emailSender = emailSender;
        }

        public async Task Handle(PostApplicationRejectedEvent notification, CancellationToken cancellationToken)
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

            var subject = "Your application has been rejected.";

            var body = $"Hi, <b>{applicationInfo.Applicant.FirstName}<b>!"
                + $"<br>Unfortunately, your application for {applicationInfo.PetName} has been rejected."
                + $"But don't worry! There's plenty of pets need your help.";

            await _emailSender.SendAsync(applicationInfo.Applicant.Email, subject, body, cancellationToken);
        }
    }
}