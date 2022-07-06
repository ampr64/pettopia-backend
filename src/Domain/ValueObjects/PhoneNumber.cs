namespace Domain.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        public string Prefix { get; private set; }

        public string Number { get; private set; }

        private PhoneNumber()
        {
        }

        public PhoneNumber(string prefix, string number)
        {
            Prefix = prefix;
            Number = number;
        }

        public override string ToString()
        {
            return Prefix + Number;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Prefix;
            yield return Number;
        }

        public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.ToString();
    }
}