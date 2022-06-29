using Domain.Enumerations;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.EventHandlers
{
    public class SendAuthorPostCompletedMessageEventHandler : INotificationHandler<PostCompletedEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;

        public SendAuthorPostCompletedMessageEventHandler(IApplicationDbContext dbContext, IEmailService emailService, IEmailTemplateService emailTemplateService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
        }

        public async Task Handle(PostCompletedEvent notification, CancellationToken cancellationToken)
        {
            var author = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == notification.Post.CreatedBy, cancellationToken);

            var applicant = notification.Post.Applications.First(x => x.Status == ApplicationStatus.Accepted.Value && 
                                                                 x.PostId == notification.Post.Id);
                                                      
            var subject = "Su publicación se ha completado exitosamente";
                       
            string body = _emailTemplateService.BuildPostCompletedTemplate(author!.FirstName, notification.Post.PetName, applicant.ApplicantInfo.Name, applicant.ApplicantInfo.Email, applicant.ApplicantInfo.PhoneNumber);
                        
            await _emailService.SendAsync(author.Email, subject, body, cancellationToken);
        }
    }
}