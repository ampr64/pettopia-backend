using Ardalis.SmartEnum;

namespace Domain.Enumerations
{
    public class PostStatus : SmartEnum<PostStatus>
    {
        public static readonly PostStatus Open = new(nameof(Open), 1);
        public static readonly PostStatus Completed = new(nameof(Completed), 2);
        public static readonly PostStatus Closed = new(nameof(Closed), 3);

        private PostStatus(string name, int value) : base(name, value)
        {
        }
    }
}