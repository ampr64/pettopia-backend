using Application.Common.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Posts.EventHandlers
{
    public class SendAuthorPostCompletedMessageEventHandler : INotificationHandler<PostCompletedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;

        public SendAuthorPostCompletedMessageEventHandler(IEmailService emailService, IIdentityService identityService)
        {
            _emailService = emailService;
            _identityService = identityService;
        }

        public async Task Handle(PostCompletedEvent notification, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserInfoByIdAsync(notification.Post.CreatedBy);

            var subject = "Su publicación se ha completado exitosamente";
            var body = $"<b>¡Hola {user!.FirstName}!</b>"
                + "<br>Te queríamos informar que tu publicación para <b>{notification.Post.PetName}</b> se completó exitosamente.<br>"
                + "¡Muchas gracias por usar nuestra plataforma!";

            await _emailService.SendAsync(user.Email, subject, body, cancellationToken);
        }
    }
}