
namespace Aristoxenus.Core.Shared.Bases;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj) => obj is ValueObject other && Equals(other);

    public bool Equals(ValueObject? other) =>
        other is not null && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public override int GetHashCode() =>
        GetEqualityComponents().Select(x => x?.GetHashCode() ?? 0).Aggregate(HashCode.Combine);

    public static bool operator ==(ValueObject? l, ValueObject? r) => Equals(l, r);
    public static bool operator !=(ValueObject? l, ValueObject? r) => !Equals(l, r);
}
