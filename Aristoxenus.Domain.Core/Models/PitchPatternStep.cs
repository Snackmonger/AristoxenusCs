using Aristoxenus.Domain.Core.Bases;
using Aristoxenus.Domain.Core.Data;

namespace Aristoxenus.Domain.Core.Models;

/// <summary>
/// A step in a pitch pattern, which is a note that is offset
/// from a target note, and which may be borrowed from a related scale
/// or from the single semitone steps around the target.
/// </summary>
public class PitchPatternStep(PitchPatternStepType type, int offset) : ValueObject
{
    public readonly PitchPatternStepType Type = type;

    public readonly int Offset = offset;

    public bool IsDiatonic => Type == PitchPatternStepType.Diatonic;
    public bool IsChromatic => Type == PitchPatternStepType.Chromatic;
    public bool IsAscending => Offset > 0;
    public bool IsDescending => Offset < 0;

    public RelativeDirection Direction
        =>
        IsAscending
            ? RelativeDirection.Higher
            :
        IsDescending
            ? RelativeDirection.Lower
            : RelativeDirection.Equal;

    public static PitchPatternStep ScaleStep(int offset)
        => new(PitchPatternStepType.Diatonic, offset);
    public static PitchPatternStep ChromaticStep(int offset)
        => new(PitchPatternStepType.Chromatic, offset);

    /// <summary>
    /// Parse this step from the perspective of the given scale.
    /// </summary>
    public RelativeInterval ParseStep(IntervalStructure scale)
    {
        Interval interval;
        // Chromatic steps move by semitone invariously. Therefore, no
        // special consideration is needed for ascending/descending
        // steps, since they always spell their exact width. If that 
        // value exists in the diatonic scale, then it gets the diatonic
        // name; if not, it gets the altered name. 
        if (IsChromatic)
        {
            int degree;
            int value = Offset;
            int adjustedValue = value % Numbers.SemitonesInDoubleOctave;
            int[] diatonic = RawScalePatterns.GetDoubleOctaveOfPattern(Keywords.Diatonic);
            int[] altered = RawScalePatterns.GetDoubleOctaveOfPattern(Keywords.Altered);

            if (scale.Intervals.Select(it => it.Value).Contains(value))
            {
                degree = scale.Intervals.Select(it => it.Value).ToList().IndexOf(value);
            }
            else if (diatonic.Contains(adjustedValue))
                degree = Array.IndexOf(diatonic, adjustedValue) + Numbers.IndexOffset;
            else
                degree = Array.IndexOf(altered, adjustedValue) + Numbers.IndexOffset;

            interval = Interval.FromExpectedValue(degree, Offset);
        }

        // Scale steps move by whatever intervals the scale has. The steps
        // used in ascent have reversed and reciprocal values in descent.
        // E.g., in C diatonic:
        // CDEFGAB = [0, 2, 4, 5, 7, 9, 11]
        // CBAGFED = [11, 9, 7, 5, 4, 2, 0] (reversed)
        // CBAGFED = [1, 3, 5, 7, 8, 10, 0] (reciprocals)
        // Thus, C to higher A is a sixth with a value of 9,
        // C to lower A is the reciprocal third with a value of 3.
        else
        {
            int octaves = 1;
            if (Offset > Numbers.NotesInOctave)
            {
                octaves = Offset / Numbers.NotesInOctave;
            }
            if (IsDescending)
            {
                scale = new(scale.Select(it => it.Reciprocal).Reverse());
            }

            int index = Offset % Numbers.NotesInOctave;
            interval = scale[index];
            int value = interval.Value * octaves;
            interval = Interval.FromExpectedValue(interval.Degree, value);
        }
        return new(interval, Direction);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return Offset;
    }
}