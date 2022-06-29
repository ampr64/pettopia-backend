namespace Domain.Entities.Users
{
    public class EndUser : Member
    {
        private EndUser()
        {
        }

        public EndUser(string id,
            string firstName,
            string lastName,
            string email,
            DateTime birthDate,
            DateTime registeredAt)
            : base(id, firstName, lastName, email, birthDate, registeredAt)
        {
        }
    }
}