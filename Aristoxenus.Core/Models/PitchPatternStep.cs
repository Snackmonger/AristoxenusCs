
namespace Aristoxenus.Core.Models
{
    /// <summary>
    /// A step in a pitch sequence pattern, which is a note that is offset
    /// from a target note, and which may be borrowed from a related scale
    /// (diatonic) or from the single semitone steps around the target.
    /// </summary>
    /// <param name="Type"></param>
    /// <param name="Offset"></param>
    public readonly record struct PitchPatternStep(PitchStepType Type, int Offset)
    {
        public bool IsDiatonic => Type == PitchStepType.Diatonic;
        public bool IsChromatic => Type == PitchStepType.Chromatic;
        public bool IsAscending => Offset > 0;
        public bool IsDescending => Offset < 0;

    };
}
