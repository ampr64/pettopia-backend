using Domain.Events;
using MediatR;

namespace Application.Features.Users.EventHandlers
{
    public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailSender _emailService;
        private readonly IUriComposer _uriComposer;

        public UserRegisteredEventHandler(IIdentityService identityService, IEmailSender emailService, IUriComposer uriComposer)
        {
            _identityService = identityService;
            _emailService = emailService;
            _uriComposer = uriComposer;
        }

        public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            var token = await _identityService.GetEmailConfirmationToken(notification.Member.Id);

            var subject = "Confirmá tu dirección de correo electrónico";
            var callbackUrl = _uriComposer.GetEmailConfirmationUrl(notification.Member.Email, token!);

            var body = $"¡Hola, {notification.Member.FirstName}! Necesitamos que confirmes tu email clickeando en <a href={callbackUrl}>este enlace.</a>";

            await _emailService.SendAsync(notification.Member.Email, subject, body, cancellationToken);
        }
    }
}