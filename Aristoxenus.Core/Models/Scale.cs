
using Aristoxenus.Core.Utilities;

namespace Aristoxenus.Core.Models
{
    /// <summary>
    /// Representation of a scaleform, with an underlying interval pattern 
    /// and a degree of modal rotation, centred on a particular tonic note.
    /// </summary>
    public class Scale
    {
        public readonly NoteName Tonic;
        public readonly List<int> ScalePattern;
        public readonly int ModeNumber;
        public readonly IntervalStructure Intervals;
        public readonly List<NoteName> Notes;

        public Scale(NoteName tonic, List<int> scalePattern, int modeNumber = 1)
        {
            Tonic = tonic;
            ScalePattern = scalePattern;
            ModeNumber =
                modeNumber > 0
                && modeNumber <= ScalePattern.Count
                    ? modeNumber
                    : 1;
            Notes = [];
            Intervals = IntervalStructure.CreateAsScale(PatternHelper.GetModeOfIntervalPattern(ScalePattern, ModeNumber));
            
        }


    }
}
