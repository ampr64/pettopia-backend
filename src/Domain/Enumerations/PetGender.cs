using Ardalis.SmartEnum;

namespace Domain.Enumerations
{
    public class PetGender : SmartEnum<PetGender>
    {
        public static readonly PetGender Male = new(nameof(Male), 1);
        public static readonly PetGender Female = new(nameof(Female), 2);
        public static readonly PetGender Unknown = new(nameof(Unknown), 3);

        private PetGender(string name, int value) : base(name, value)
        {
        }
    }
}