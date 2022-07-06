using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.EventHandlers
{
    public class PostCreatedEventHandler : INotificationHandler<PostCreatedEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailSender _emailService;
        private readonly IUriComposer _uriComposer;

        public PostCreatedEventHandler(IApplicationDbContext dbContext, IEmailSender emailService, IUriComposer uriComposer)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _uriComposer = uriComposer;
        }

        public async Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken)
        {
            var author = await _dbContext.Members
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == notification.Post.CreatedBy, cancellationToken);

            var postUrl = _uriComposer.GetPostDetailUrl(notification.Post.Id);
            
            var subject = "Post created successfully.";

            var body = $"<b>Hi, {author!.FirstName}!</b>"
                + $"<br>We wanted to let you know that your post for <b>{notification.Post.PetName}</b> has been created successfully.<br>"
                + "Thanks for using our platform!";

            await _emailService.SendAsync(author.Email, subject, body, cancellationToken);
        }
    }
}