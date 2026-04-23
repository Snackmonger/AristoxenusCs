using System.Numerics;

namespace Aristoxenus.Core.SharedKernel.Bases;

/// <summary>
/// General purpose representation of a non-negative number.
/// </summary>
public readonly record struct NonNegative<T> : IComparable<NonNegative<T>>
    where T : struct,
                INumber<T>,
                IComparisonOperators<T, T, bool>
{
    public T Value { get; init; }

    // Constructors, Factories ////////////////////////////////////////////////
    private NonNegative(T value)
    {
        if (value < default(T))
            throw new Exception("Number cannot be less than zero");
        Value = value;
    }

    public NonNegative() => Value = default;

    public static NonNegative<U> CreateNew<U>(U value) where U : struct, INumber<U>
        => new(value);


    // Number-like Api ////////////////////////////////////////////////////////

    public NonNegative<T> Add(NonNegative<T> value)
        => new(Value + value.Value);
    public NonNegative<T> Subtract(NonNegative<T> value)
        => new(Value - value.Value);
    public static implicit operator T(NonNegative<T> value)
        => value.Value;
    public static implicit operator NonNegative<T>(T value)
        => new(value);
    public static NonNegative<T> operator +(NonNegative<T> value1, NonNegative<T> value2)
        => value1.Add(value2);
    public static NonNegative<T> operator -(NonNegative<T> value1, NonNegative<T> value2)
        => value1.Subtract(value2);
    public static bool operator >(NonNegative<T> value1, NonNegative<T> value2)
        => value1.Value > value2.Value;
    public static bool operator <(NonNegative<T> value1, NonNegative<T> value2)
        => value1.Value < value2.Value;
    public static bool operator >=(NonNegative<T> value1, NonNegative<T> value2)
        => value1.Value >= value2.Value;
    public static bool operator <=(NonNegative<T> value1, NonNegative<T> value2)
        => value1.Value <= value2.Value;

    // IComparable ////////////////////////////////////////////////////////////
    public int CompareTo(NonNegative<T> other)
        => Value.CompareTo(other.Value);

    public override string ToString()
        => Value.ToString() ?? "0";
}
