using Aristoxenus.Domain.Core.Bases;
using Aristoxenus.Domain.Core.Data;

namespace Aristoxenus.Domain.Core.Models;

/// <summary>
/// Representation of a specific voicing derived from a root position chord.
/// </summary>
/// <remarks>
/// There are two valid subclasses, representing "drop" chords 
/// (transpose notes down) and "lift" chords (transpose notes up).
/// </remarks>
public class ChordVoicing : ValueObject
{
    /// <summary>
    /// The indices of the notes in the inverted chord that will 
    /// be transposed to form the specific voicing. 
    /// </summary>
    public List<int> DisplacedIntervalIndices { get; init; } = [];

    /// <summary>
    /// The inversion/rotation of the interval set.
    /// </summary>
    public int Inversion { get; init; }

    //public List<Interval> Apply(IEnumerable<Interval> intervals)
    //{
    //    List<Interval> _intervals = [..intervals];
    //    foreach (int displacedIndex in DisplacedIntervalIndices)
    //    {
    //        if (displacedIndex <= 0 || displacedIndex >= intervals.Count())
    //        {
    //            throw new ArgumentOutOfRangeException($"Displaced interval index {displacedIndex} is out of range for the provided intervals.");
    //        }


    //    }
    //}

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        foreach (int displacedIndex in DisplacedIntervalIndices)
        {
            yield return displacedIndex;
        }
        yield return Inversion;
    }


}

