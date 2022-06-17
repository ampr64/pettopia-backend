using Ardalis.SmartEnum;

namespace Domain.Enumerations
{
    public class PetAge : SmartEnum<PetAge>
    {
        public static readonly PetAge Young = new(nameof(Young), 1);
        public static readonly PetAge Adult = new(nameof(Adult), 2);
        public static readonly PetAge Mature = new(nameof(Mature), 3);
        public static readonly PetAge Geriatric = new(nameof(Geriatric), 4);

        private PetAge(string name, int value) : base(name, value)
        {
        }
    }
}