using Aristoxenus.Domain.Core.Data;

namespace Aristoxenus.Domain.Core.Utilities
{
    /**
     * A static class that provides normalization methods for musical concepts.
     * 
     * These limits set by these normalizations are not necessarily inherent
     * in the musical concepts themselves, but are instead imposed by the
     * program for practical reasons.
     */
    public static class NormalizationHelper
    {
        /**
         * Normalize a degree. A degree needs to be a positive integer,
         * so negatives will be normalized to their positive equivalents. 
         * 
         * WARNING: A degree of 0 is assumed to be an off-by-one error and 
         * will be normalized to 1.
         */
        public static int NormalizeDegree(int degree) => degree == 0 ? 1: Math.Abs(degree);

        /**
         * Normalize a pitch class. A pitch class needs to be a non-negative 
         * integer between 0 and 6, so negatives will be normalized to their 
         * positive equivalents.
         * 
         * Their meaning is as follows:
         * 
         * | C | D | E | F | G | A | B |
         * |---|---|---|---|---|---|---|
         * | 0 | 1 | 2 | 3 | 4 | 5 | 6 |
         * */
        public static int NormalizePitchClass(int pitchClass) => Math.Abs(pitchClass % Numbers.NotesInOctave);

        /**
         * Normalize an accidental. An accidental needs to be an integer between -11 and 11. 
         * */
        public static int NormalizeAccidental(int accidental) => accidental % Numbers.SemitonesInOctave;
        // NOTE: Most people will not want so many accidentals, so having a way to clamp this 
        // to a runtime constant instead of a compile-time constant would be useful.
        // However, this is not currently implemented.

    }
}
