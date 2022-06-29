using Ardalis.SmartEnum;

namespace Domain.Entities.Users
{
    public class AccountStatus : SmartEnum<AccountStatus>
    {
        public static readonly AccountStatus Active = new(nameof(Active), 1);
        public static readonly AccountStatus Deactivated = new(nameof(Deactivated), 2);
        public static readonly AccountStatus Banned = new(nameof(Banned), 3);

        private AccountStatus(string name, int value) : base(name, value)
        {
        }
    }
}