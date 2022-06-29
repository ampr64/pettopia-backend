using Ardalis.SmartEnum;

namespace Domain.Enumerations
{
    public class TemplateFileName : SmartEnum<TemplateFileName, string>
    {
        public static readonly TemplateFileName PostCreatedFileName = new(nameof(PostCreatedFileName), "PostCreatedEmailTemplate.html");
        public static readonly TemplateFileName PostCompletedFileName = new(nameof(PostCompletedFileName), "SendAuthorPostCompletedMessageEmailTemplate.html");

        private TemplateFileName(string name, string value) : base(name, value)
        {
        }
    }
}
