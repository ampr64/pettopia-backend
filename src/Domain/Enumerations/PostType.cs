using Ardalis.SmartEnum;

namespace Domain.Enumerations
{
    public class PostType : SmartEnum<PostType>
    {
        public static readonly PostType Adoption = new(nameof(Adoption), 1);
        public static readonly PostType Missing = new(nameof(Missing), 2);
        public static readonly PostType Found = new(nameof(Found), 3);

        private PostType(string name, int value) : base(name, value)
        {
        }
    }
}