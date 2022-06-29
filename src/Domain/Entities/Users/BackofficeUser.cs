namespace Domain.Entities.Users
{
    public class BackOfficeUser : Member
    {
        private BackOfficeUser()
        {
        }

        public BackOfficeUser(string id,
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