using Aristoxenus.Domain.Core.Bases;
using Aristoxenus.Domain.Core.Data;

namespace Aristoxenus.Domain.Core.Models;

/// <summary>
/// Representation of a specific voicing derived from a root position chord.
/// </summary>
public class ChordVoicing : ValueObject
{
    /// <summary>
    /// The indices of the notes in the inverted chord that will 
    /// be transposed to form the specific voicing. 
    /// </summary>
    public List<int> DisplacedIntervalIndices { get; init; }

    protected ChordVoicing(IEnumerable<int> displacedIntervalIndices)
    {
        DisplacedIntervalIndices = [.. displacedIntervalIndices];
    }

    public static ChordVoicing RootPosition => new([]);
    public static ChordVoicing Drop2 => new(RawVoicings.Drop2);
    public static ChordVoicing Drop3 => new(RawVoicings.Drop3);
    public static ChordVoicing Drop2and4 => new(RawVoicings.Drop2And4);
    public static ChordVoicing SpreadTriad => new(RawVoicings.SpreadTriad);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        foreach (int displacedIndex in DisplacedIntervalIndices)
        {
            yield return displacedIndex;
        }
    }


}

