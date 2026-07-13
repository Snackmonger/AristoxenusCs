
using Aristoxenus.Core.Interfaces;

namespace Aristoxenus.Core.Models;

/// <summary>
/// A strategy for generating a sequence of pitches around an unknown
/// target pitch. This is the default implementation of <see cref=
/// "IPitchSequenceStrategy"/> and is produced by the <see cref=
/// "PitchSequenceStrategyBuilder"/>.
/// </summary>
public class PitchSequenceStrategy : IPitchSequenceStrategy
{
    private List<RelativeInterval> _relativeIntervals;
    internal PitchSequenceStrategy(IEnumerable<RelativeInterval> intervals)
    {
        _relativeIntervals = [.. intervals];
    }
    public List<Pitch> Execute(Pitch targetPitch)
    {
        return [.._relativeIntervals.Select(it => it.Direction switch 
        {
            RelativeDirection.Higher => targetPitch.GetPitchAbove(it.Interval),
            RelativeDirection.Lower => targetPitch.GetPitchBelow(it.Interval),
            _ => targetPitch,
        }
        )];
    }
}
