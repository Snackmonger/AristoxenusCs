using Aristoxenus.Core.Domains.Theory.Atoms;
using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;

namespace Aristoxenus.Core.Domains.Theory.Harmony
{
    /// <summary>
    /// Representation of a scaleform, with an underlying interval pattern 
    /// and a degree of modal rotation, centred on a particular tonic note.
    /// </summary>
    public class Scale : ValueObject
    {
        public readonly NoteName Tonic;
        public readonly ScalePattern ScalePattern;
        public readonly int ModeNumber;
        public readonly IntervalStructure Intervals;
        public readonly List<NoteName> Notes;

        public Scale(NoteName tonic, ScalePattern scalePattern, int modeNumber = 1)
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
                Intervals = IntervalStructure.CreateAsHeptatonic(ScalePattern.Mode(ModeNumber));
            else
            {
                List<Interval> intervals = [];
                List<int> mode = ScalePattern.Mode(ModeNumber);
                for (int i = 0; i < ScalePattern.Count; i++)
                    if (ScalePattern.Diatonic.Contains(mode[i]))
                    {
                        intervals.Add(IntervalStructure.Diatonic[i]);
                    }
                    else
                        intervals.Add(IntervalStructure.Altered[i]);
                Intervals = new(intervals);
            }
        }


        // ValueObject ////////////////////////////////////////////////////////
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Tonic.ToSymbol();
            foreach (string symbol in Intervals.IntervalSymbols)
            {
                yield return symbol;
            }
        }
    }
}
