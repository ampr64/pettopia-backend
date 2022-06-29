namespace Domain.Entities.Users
{
    public class ProfilePicture : ValueObject
    {
        public string Blob { get; private init; }

        public ProfilePicture(string blob)
        {
            Blob = blob;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Blob;
        }
    }
}