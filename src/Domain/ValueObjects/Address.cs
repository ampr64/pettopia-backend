namespace Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public string Province { get; }

        public string City { get; }

        public string Line1 { get; }

        public string? Line2 { get; }

        public string ZipCode { get; }

        public Address(string province,
            string city,
            string line1,
            string? line2,
            string zipCode)
        {
            Province = province;
            City = city;
            Line1 = line1;
            Line2 = line2;
            ZipCode = zipCode;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Province;
            yield return City;
            yield return Line1;
            yield return Line2;
            yield return ZipCode;
        }
    }
}