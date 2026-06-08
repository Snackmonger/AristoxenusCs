

using Aristoxenus.Core.Shared.Data;

namespace Aristoxenus.Core.Shared.Utilities
{
    public static class ValidationHelper
    {
        public static int NormalizeDegree(int degree) => Math.Abs(degree %( Numbers.NotesInOctave * 2 + Numbers.IndexOffset));
        public static int NormalizePitchClass(int pitchClass) => Math.Abs(pitchClass % Numbers.NotesInOctave);
        public static int NormalizeAccidental(int accidental) => accidental % Numbers.MaxAccidentals;
    }
}
