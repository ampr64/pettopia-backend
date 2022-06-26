namespace Domain.Entities.Posts
{
    public class ApplicantInfo : ValueObject
    {
        public Guid ApplicationId { get; private init; }

        public string Name { get; private init; }

        public string Email { get; private init; }

        public string? PhoneNumber { get; private init; }

        public ApplicantInfo(Guid applicationId, string name, string email, string? phoneNumber)
        {
            ApplicationId = applicationId;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return ApplicationId;
            yield return Email;
        }
    }
}