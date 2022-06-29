using Application.Common.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Posts.EventHandlers
{
    public class SendAuthorPostCompletedMessageEventHandler : INotificationHandler<PostCompletedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;
        private readonly IEmailTemplateService _emailTemplateService;

        public SendAuthorPostCompletedMessageEventHandler(IEmailService emailService, IIdentityService identityService, IEmailTemplateService emailTemplateService)
        {
            _emailService = emailService;
            _identityService = identityService;
            _emailTemplateService = emailTemplateService;
        }

        public async Task Handle(PostCompletedEvent notification, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserInfoByIdAsync(notification.Post.CreatedBy);

            string postId = notification.Post.Id.ToString();

            string postUrl = "https://localhost:4200" + postId;

            var subject = "Su publicación se ha completado exitosamente";
                       
            string body = _emailTemplateService.BuildPostCompletedTemplate(user!.FirstName, notification.Post.PetName, postUrl);
                        
            await _emailService.SendAsync(user.Email, subject, body, cancellationToken);
        }
    }
}