namespace Domain.Entities.Users
{
    public class Administrator : Member
    {
        private Administrator()
        {
        }

        public Administrator(string id,
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