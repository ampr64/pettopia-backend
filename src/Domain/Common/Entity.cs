namespace Domain.Common
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
        where TId : notnull, IEquatable<TId>
    {
        private int? _hashCode;

        public TId Id { get; protected set; } = default!;

        protected bool IsTransient => Id?.Equals(default) ?? true;

        public override bool Equals(object? obj)
        {
            return Equals(obj as Entity<TId>);
        }

        public bool Equals(Entity<TId>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            if (IsTransient || other.IsTransient)
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            if (IsTransient)
            {
                return base.GetHashCode();
            }

            _hashCode ??= Id.GetHashCode() ^ 31;
            return _hashCode.Value;
        }

        public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        {
            return !(left == right);
        }
    }
}