namespace Aristoxenus.Domain.Core.Models
{
    public enum PitchPatternStepType
    {
        /// <summary>
        /// A step that represents a number of scale degrees away from the 
        /// target.
        /// </summary>
        Diatonic,

        /// <summary>
        /// A step that represents a number of semitones away from the target.
        /// </summary>
        Chromatic
    }
}
