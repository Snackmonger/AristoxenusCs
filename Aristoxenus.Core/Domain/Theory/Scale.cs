using Aristoxenus.Core.Constants;
using Aristoxenus.Core.SharedKernel.Bases;

namespace Aristoxenus.Core.Domain.Theory
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

        private Scale(NoteName tonic, ScalePattern scalePattern, int modeNumber)
        {
            Tonic = tonic;
            ScalePattern = scalePattern;
            ModeNumber = 
                modeNumber > 0 
                && modeNumber <= ScalePattern.Count
                    ? modeNumber 
                    : 1;
            Intervals = IntervalStructure.CreateNew();
            Notes = [];
            if (ScalePattern.Count == Numbers.NotesInOctave)
            {
                Intervals = IntervalStructure.CreateAsHeptatonic(ScalePattern.Mode(ModeNumber));
            }
            else
            {
                var mode = ScalePattern.Mode(ModeNumber);
                for (int i = 0; i < ScalePattern.Count; i++)
                {
                    Intervals = ScalePatternsHeptatonic.Diatonic.Contains(mode[i]) 
                        ? Intervals.Add(IntervalStructure.Diatonic[i]) 
                        : Intervals.Add(IntervalStructure.Altered[i]);
                }
            }
            foreach (Interval interval in Intervals)
            {
                Notes.Add(Tonic.NoteNameAbove(interval));
            }
        }

        //public Scale CreateNew(string tonic, string scaleName, int modeNumber)
        //    => new(NoteName.CreateNew(tonic), ScalePattern.CreateNew(scaleName), modeNumber);


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
