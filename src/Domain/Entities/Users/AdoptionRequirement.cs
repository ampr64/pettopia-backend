namespace Domain.Entities.Users
{
    public class AdoptionRequirement : ValueObject
    {
        public string Requirement { get; private init; }

        public AdoptionRequirement(string requirement)
        {
            Requirement = requirement;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Requirement;
        }

        public override string ToString()
        {
            return Requirement;
        }
    }
}