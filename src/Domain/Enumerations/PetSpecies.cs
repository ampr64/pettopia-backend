using Ardalis.SmartEnum;

namespace Domain.Enumerations
{
    public class PetSpecies : SmartEnum<PetSpecies>
    {
        public static readonly PetSpecies Cat = new(nameof(Cat), 1);
        public static readonly PetSpecies Dog = new(nameof(Dog), 2);

        public PetSpecies(string name, int value) : base(name, value)
        {
        }
    }
}