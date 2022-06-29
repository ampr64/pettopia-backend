using Application.Common.Interfaces;
using Domain.Enumerations;
using MimeKit;
using System.Reflection;

namespace Infrastructure.Mail.Templates
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string BuildPostCompletedTemplate(string firstName, string petName, string postLink)
        {
            var builder = new BodyBuilder();

            string path = Assembly.GetExecutingAssembly().Location;

            string templateFileName = TemplateFileName.PostCompletedFileName.Value;

            string templatePath = path.Replace("WebApi\\bin\\Debug\\net6.0\\Infrastructure.dll",
                                               "Infrastructure\\Mail\\Templates\\Html\\" + templateFileName);

            using (StreamReader SourceReader = File.OpenText(templatePath))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string body = string.Format(builder.HtmlBody, firstName, petName, postLink);

            return body;
        }

        public string BuildPostCreatedTemplate(string firstName, string petName, string postLink)
        {
            var builder = new BodyBuilder();

            string path = Assembly.GetExecutingAssembly().Location;

            string templateFileName = TemplateFileName.PostCreatedFileName.Value;

            string templatePath = path.Replace("WebApi\\bin\\Debug\\net6.0\\Infrastructure.dll",
                                               "Infrastructure\\Mail\\Templates\\Html\\" + templateFileName);

            using (StreamReader SourceReader = File.OpenText(templatePath))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string body = string.Format(builder.HtmlBody, firstName, petName, postLink);

            return body;
        }

    }
}
