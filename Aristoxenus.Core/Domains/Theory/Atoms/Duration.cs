using Fractions;

using Aristoxenus.Core.Shared.Data;

namespace Aristoxenus.Core.Domains.Theory.Atoms;


public class Duration
{
    public Fraction Value { get; init; }
    public Duration(Fraction value)
    {
        Value = value;
    }
    public static Duration Whole => new(DurationValues.Whole);
    public static Duration Half => new(DurationValues.Half);
    public static Duration Quarter => new(DurationValues.Quarter);
    public static Duration Eighth => new(DurationValues.Eighth);
    public static Duration Sixteenth => new(DurationValues.Sixteenth);
    public static Duration ThirtySecond => new(DurationValues.ThirtySecond);
    public static Duration SixtyFourth => new(DurationValues.SixtyFourth);
    public static Duration HundredTwentyEighth => new(DurationValues.HundredTwentyEighth);

    public Duration Triplet => new(Value.Multiply(2).Divide(3));
    public Duration Dotted => new(Value.Add(Value.Divide(2)));

    public static Duration operator +(Duration left, Duration right)
        => new(left.Value + right.Value);
    public static Duration operator -(Duration left, Duration right)
        => new(left.Value - right.Value);
    public static Duration operator *(Duration left, int right)
        => new(left.Value * right);
    public static Duration operator /(Duration left, int right)
        => new(left.Value / right);

    public static bool operator >=(Duration left, Duration right)
    {
        return left.Value >= right.Value;
    }
    public static bool operator <=(Duration left, Duration right)
    {
        return left.Value <= right.Value;
    }

    public override string ToString() => Value.ToString();

}
