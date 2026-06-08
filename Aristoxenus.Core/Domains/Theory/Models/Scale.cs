using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Utilities;

namespace Aristoxenus.Core.Domains.Theory.Models
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
            if (ScalePattern.Count == Numbers.NotesInOctave)
                Intervals = IntervalStructure.CreateAsHeptatonic(PatternHelper.GetModeOfIntervalPattern(ScalePattern, ModeNumber));
            else
            {
                List<Interval> intervals = [];
                List<int> mode = PatternHelper.GetModeOfIntervalPattern(ScalePattern, ModeNumber);
                for (int i = 0; i < ScalePattern.Count; i++)
                    if (RawScalePatterns.Diatonic.Contains(mode[i]))
                    {
                        intervals.Add(IntervalStructure.Diatonic[i]);
                    }
                    else
                        intervals.Add(IntervalStructure.Altered[i]);
                Intervals = new(intervals);
            }
        }


    }
}
