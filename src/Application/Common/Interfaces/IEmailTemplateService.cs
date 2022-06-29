
namespace Application.Common.Interfaces
{
    public interface IEmailTemplateService
    {
        string BuildPostCreatedTemplate(string firstName, string petName, string postLink);

        string BuildPostCompletedTemplate(string firstName, string petName, string postLink);
    }
}
