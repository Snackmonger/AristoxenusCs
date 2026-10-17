using Aristoxenus.Core.Bases;
using Aristoxenus.Core.Data;

namespace Aristoxenus.Core.Models;

/// <summary>
/// Representation of a specific voicing derived from a root position chord.
/// </summary>
/// <remarks>
/// There are two valid subclasses, representing "drop" chords 
/// (transpose notes down) and "lift" chords (transpose notes up).
/// </remarks>
public abstract class ChordVoicing : ValueObject
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

    public abstract List<int> Apply(List<int> intervals);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        foreach (int displacedIndex in DisplacedIntervalIndices)
        {
            yield return displacedIndex;
        }
        yield return Inversion;
    }


    private static void Validate(List<int> displacedIntervalIndices, int inversion)
    {
        if (displacedIntervalIndices.Select(it => it > Numbers.SemitonesInOctave * 2 || it < Numbers.Zero).Count() > 0 || inversion < 0)
            throw new ArgumentException("Illegal chord structure."); // TODO: more specific error msgs
    }

    public static DropVoicing Drop(List<int> displacedIntervalIndices, int inversion)
    {
        Validate(displacedIntervalIndices, inversion);
        return new(displacedIntervalIndices, inversion);
    }
    public static LiftVoicing Lift(List<int> displacedIntervalIndices, int inversion)
    {
        Validate(displacedIntervalIndices, inversion);
        return new(displacedIntervalIndices, inversion);
    }

    public sealed class DropVoicing : ChordVoicing
    {
        internal DropVoicing(List<int> displacedIntervalIndices, int inversion)
        {
            DisplacedIntervalIndices = displacedIntervalIndices;
            Inversion = inversion;
        }
        public override List<int> Apply(List<int> intervals)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class LiftVoicing : ChordVoicing
    {
        internal LiftVoicing(List<int> displacedIntervalIndices, int inversion)
        {
            DisplacedIntervalIndices = displacedIntervalIndices;
            Inversion = inversion;
        }
        public override List<int> Apply(List<int> intervals)
        {
            throw new NotImplementedException();
        }
    }
}

