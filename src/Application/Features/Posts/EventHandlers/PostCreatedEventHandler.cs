using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.EventHandlers
{
    public class PostCreatedEventHandler : INotificationHandler<PostCreatedEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IUriComposer _uriComposer;

        public PostCreatedEventHandler(IApplicationDbContext dbContext, IEmailService emailService, IEmailTemplateService emailTemplateService, IUriComposer uriComposer)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _uriComposer = uriComposer;
        }

        public async Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken)
        {
            var author = await _dbContext.Members
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == notification.Post.CreatedBy, cancellationToken);

            var postUrl = _uriComposer.GetPostDetailUrl(notification.Post.Id);
            
            var subject = "Publicación creada exitosamente";
                                                      
            var body = _emailTemplateService.BuildPostCreatedTemplate(author!.FirstName, notification.Post.PetName, postUrl);
                                  
            await _emailService.SendAsync(author.Email, subject, body, cancellationToken);
        }
    }
}