namespace Domain.Entities.Users
{
    public class ContactInfo : ValueObject
    {
        public PhoneNumber? PhoneNumber { get; private init; }

        public string? InstagramProfileUrl { get; private init; }

        public string? FacebookProfileUrl { get; private init; }

        private ContactInfo()
        {
        }

        public ContactInfo(PhoneNumber? phoneNumber = null,
            string? instagramProfileUrl = null,
            string? facebookProfileUrl = null)
        {
            PhoneNumber = phoneNumber;
            InstagramProfileUrl = instagramProfileUrl;
            FacebookProfileUrl = facebookProfileUrl;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return PhoneNumber;
            yield return InstagramProfileUrl;
            yield return FacebookProfileUrl;
        }
    }
}