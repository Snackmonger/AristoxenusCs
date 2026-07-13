
using Aristoxenus.Core.Data;
using Aristoxenus.Core.Interfaces;

namespace Aristoxenus.Core.Models;

/// <summary>
/// A builder that creates a <see cref="IPitchSequenceStrategy"/>
/// </summary>
public class PitchSequenceStrategyBuilder
{

    /// <summary>
    /// A pattern of positive and negative steps relative to the target 
    /// pitch.
    /// </summary>
    private List<PitchPatternStep> _pattern;

    /// <summary>
    /// The interval structure of the scale of which the target note 
    /// should be considered the tonic.
    /// </summary>
    private IntervalStructure? _scale;

    private PitchSequenceStrategyBuilder AddStep(PitchPatternStep step)
    {
        _pattern.Add(step);
        return this;
    }

    public PitchSequenceStrategyBuilder UseScale(IntervalStructure intervals)
    {
        _scale = intervals;
        return this;
    }

    public PitchSequenceStrategyBuilder AddChromaticStep(int offset)
        => AddStep(new(PitchStepType.Chromatic, offset));

    public PitchSequenceStrategyBuilder AddDiatonicStepAbove(int offset)
        => AddStep(new(PitchStepType.Diatonic, offset));

    private RelativeInterval Parse(PitchPatternStep step)
    {
        Interval interval;
        int degree;
        // Chromatic steps move by semitone invariously. Therefore, no
        // special consideration is needed for ascending/descending
        // steps, since they always spell their exact width.
        if (step.IsChromatic)
        {
            int value = step.Offset;
            int adjustedValue = value % Numbers.SemitonesInDoubleOctave;
            int[] diatonic = RawScalePatterns.GetDoubleOctaveOfPattern(Keywords.Diatonic);
            int[] altered = RawScalePatterns.GetDoubleOctaveOfPattern(Keywords.Altered);

            // Check if the builder has a scale set & use its degree
            // if available. Otherwise, use the diatonic degree if 
            // available. Otherwise, use the altered degree.
            if (_scale!.Intervals.Select(it => it.Value).Contains(value))
            {
                degree = _scale!.Intervals.Select(it => it.Value).ToList().IndexOf(value);
            }
            else if (diatonic.Contains(adjustedValue))
                degree = Array.IndexOf(diatonic, adjustedValue) + Numbers.IndexOffset;
            else
                degree = Array.IndexOf(altered, adjustedValue) + Numbers.IndexOffset;

            interval = Interval.FromExpectedValue(degree, step.Offset);
        }

        // Diatonic steps move by whatever intervals the scale has. The steps
        // used in ascent have reversed and reciprocal values in descent.
        // E.g., in C diatonic:
        // CDEFGAB = [0, 2, 4, 5, 7, 9, 11]
        // CBAGFED = [11, 9, 7, 5, 4, 2, 0] (reversed)
        // CBAGFED = [1, 3, 5, 7, 8, 10, 0] (reciprocals)
        // Thus, C to higher A is a sixth with a value of 9,
        // C to lower A is the reciprocal third with a value of 3.
        else
        {
            // Scale is required for diatonic steps. If not already set, we use
            // the diatonic scale as a default.
            _scale ??= IntervalStructure.Diatonic;
            if (step.Offset > Numbers.NotesInOctave)
            {
                int octaves = step.Offset / Numbers.NotesInOctave;
            }
            if (step.IsDescending)
            {
                _scale = new(_scale.Select(it => it.Reciprocal).Reverse());
            }

            int index = step.Offset % Numbers.NotesInOctave;
            interval = _scale[index];


        }
        throw new NotImplementedException();
    }
    private RelativeInterval ParseStepBelow(PitchPatternStep step)
    {
        throw new NotImplementedException();
    }

    private RelativeInterval ParseStep(PitchPatternStep step)
    {
        return RelativeInterval.Unison;
    }

    public IPitchSequenceStrategy Build()
    {
        if (_pattern.Any(it => it.Type == PitchStepType.Diatonic) && _scale == null)
        {
            throw new InvalidOperationException("Diatonic steps are used, but no scale is defined");
        }
        List<RelativeInterval> steps = [.. _pattern.Select(ParseStep)];
        throw new NotImplementedException();

    }
}
