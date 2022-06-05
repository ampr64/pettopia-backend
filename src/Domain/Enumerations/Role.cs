using Ardalis.SmartEnum;

namespace Domain.Enumerations
{
    public class Role : SmartEnum<Role, string>
    {
        public static readonly Role User = new(nameof(User), "1");
        public static readonly Role Fosterer = new(nameof(Fosterer), "2");
        public static readonly Role Admin = new(nameof(Admin), "3");
        public static readonly Role BackOfficeUser = new(nameof(BackOfficeUser), "4");

        public Role(string name, string value) : base(name, value)
        {
        }
    }
}