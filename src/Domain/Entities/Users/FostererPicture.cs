namespace Domain.Entities.Users
{
    public class FostererPicture
    {
        public string FostererId { get; private init; }

        public string Blob { get; private init; }

        public int Order { get; private init; }

        public FostererPicture(string fostererId, string blob, int order)
        {
            FostererId = fostererId;
            Blob = blob;
            Order = order;
        }
    }
}