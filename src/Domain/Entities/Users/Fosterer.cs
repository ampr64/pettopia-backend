namespace Domain.Entities.Users
{
    public class Fosterer : Member
    {
        public string OrganizationName { get; private set; }

        public ApplicationForm ApplicationForm { get; private set; }

        private readonly List<FostererPicture> _pictures = new();
        public IReadOnlyList<FostererPicture> Pictures => _pictures.AsReadOnly();

        //public override bool IsComplete => base.IsComplete && AdoptionForm is { Length: > 0 };

        private Fosterer()
        {
        }

        public Fosterer(string id,
            string organizationName,
            string firstName,
            string lastName,
            string email,
            DateTime birthDate,
            DateTime registeredAt)
            : base(id, firstName, lastName, email, birthDate, registeredAt)
        {
            OrganizationName = organizationName;
        }

        public void ChangeOrganizationName(string organizationName)
        {
            OrganizationName = organizationName;
        }

        public void SetApplicationForm(DateTime updatedAt, ApplicationForm applicationForm)
        {
            ApplicationForm = applicationForm;
            UpdatedAt = updatedAt;
        }

        public void SetPictures(DateTime updatedAt, List<FostererPicture> pictures)
        {
            _pictures.Clear();
            _pictures.AddRange(pictures);

            UpdatedAt = updatedAt;
        }
    }
}