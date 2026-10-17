using Aristoxenus.Domain.Core.Bases;

namespace Aristoxenus.Domain.Core.Models;

/// <summary>
/// A relative interval, which is an intervallic magnitude bound to a 
/// relative direction in pitch space (higher, lower, or equal).
/// </summary>
public class RelativeInterval : ValueObject
{
    public readonly RelativeDirection Direction;
    public readonly Interval Interval;
    public bool IsHigher => Direction == RelativeDirection.Higher;
    public bool IsLower => Direction == RelativeDirection.Lower;
    public bool IsEqual => Direction == RelativeDirection.Equal;

    public RelativeInterval(Interval interval, RelativeDirection direction)
    {
        Interval = interval;
        Direction = direction;
    }

    public static RelativeInterval Unison => new(Interval.Unison, RelativeDirection.Equal);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
