namespace Domain.Entities.Users
{
    public class ApplicationForm : Entity<Guid>
    {
        public string FostererId { get; private set; }

        public IReadOnlyList<AdoptionRequirement> Requirements { get; private init; }

        private ApplicationForm()
        {
        }

        public ApplicationForm(string fostererId, IEnumerable<string> requirements)
        {
            Id = Guid.NewGuid();

            FostererId = fostererId;
            Requirements = requirements.Select(r => new AdoptionRequirement(r)).ToList();
        }
    }
}