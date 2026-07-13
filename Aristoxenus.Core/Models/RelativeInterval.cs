namespace Aristoxenus.Core.Models;

public readonly record struct RelativeInterval(Interval Interval, RelativeDirection Direction)
{
    public bool IsHigher => Direction == RelativeDirection.Higher;
    public bool IsLower => Direction == RelativeDirection.Lower;
    public bool IsEqual => Direction == RelativeDirection.Equal;

    public static RelativeInterval Unison => new(Interval.Unison, RelativeDirection.Equal);
}
