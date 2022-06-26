using Ardalis.SmartEnum;

namespace Domain.Enumerations
{
    public class ApplicationStatus : SmartEnum<ApplicationStatus>
    {
        public static readonly ApplicationStatus Pending = new(nameof(Pending), 1);
        public static readonly ApplicationStatus Accepted = new(nameof(Accepted), 2);
        public static readonly ApplicationStatus Rejected = new(nameof(Rejected), 3);
        public static readonly ApplicationStatus Canceled = new(nameof(Canceled), 4);

        private ApplicationStatus(string name, int value) : base(name, value)
        {
        }
    }
}