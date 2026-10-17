using Aristoxenus.Core.Interfaces;

namespace Aristoxenus.Core.Models;

/// <summary>
/// A strategy for generating a sequence of pitches around an unknown
/// target pitch.
/// </summary>
public class PitchPatternStrategy : IPitchPatternStrategy
{
    private readonly List<RelativeInterval> _relativeIntervals;
    public PitchPatternStrategy(IEnumerable<RelativeInterval> intervals)
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
