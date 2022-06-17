using Ardalis.SmartEnum;

namespace Domain.Enumerations
{
    public class NeuterStatus : SmartEnum<NeuterStatus>
    {
        public static readonly NeuterStatus Neutered = new(nameof(Neutered), 1);
        public static readonly NeuterStatus NotNeutered = new(nameof(NotNeutered), 2);
        public static readonly NeuterStatus Unknown = new(nameof(Unknown), 3);

        private NeuterStatus(string name, int value) : base(name, value)
        {
        }
    }
}