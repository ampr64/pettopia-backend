using Domain.Enumerations;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.EventHandlers
{
    public class PostCompletedEventHandler : INotificationHandler<PostCompletedEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailSender _emailService;

        public PostCompletedEventHandler(IApplicationDbContext dbContext, IEmailSender emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task Handle(PostCompletedEvent notification, CancellationToken cancellationToken)
        {
            var author = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == notification.Post.CreatedBy, cancellationToken);

            var applicant = notification.Post.Applications.First(x => x.Status == ApplicationStatus.Accepted.Value && 
                                                                 x.PostId == notification.Post.Id);

            var subject = "Your post has been completed successfully.";
            var body = $"<b>Hi, {author!.FirstName}!</b>"
                + $"<br>We wanted to let you know that your post for <b>{notification.Post.PetName}</b> has been completed succesfully.<br>"
                + "Thank you for using our platform!";

            await _emailService.SendAsync(author.Email, subject, body, cancellationToken);
        }
    }
}