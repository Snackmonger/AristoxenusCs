using Aristoxenus.Core.Data;

namespace Aristoxenus.Core.Utilities
{
    public static class NormalizationHelper
    {
        public static int NormalizeDegree(int degree) => Math.Abs(degree);
        public static int NormalizePitchClass(int pitchClass) => Math.Abs(pitchClass % Numbers.NotesInOctave);
        public static int NormalizeAccidental(int accidental) => accidental % Numbers.MaxAccidentals;
    }
}
