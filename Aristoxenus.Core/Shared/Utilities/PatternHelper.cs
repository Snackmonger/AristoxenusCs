
using Aristoxenus.Core.Shared.Data;

namespace Aristoxenus.Core.Shared.Utilities;

public class PatternHelper
{
    /// <summary>
    /// Return the <paramref name="pattern"/>'s values, rotated so that the 
    /// value at the given <paramref name="modeNumber"/> is now considered 0, 
    /// and the others are offset by a compensatory amount, wrapping around 
    /// to 1 after 12. 
    /// 
    /// E.g. [0, 2, 4, 5, 7, 9, 11] mode 2 => [0, 2, 3, 5, 7, 9, 10]
    /// </summary>
    public static List<int> GetModeOfIntervalPattern(IList<int> _pattern, int modeNumber)
    {
        modeNumber = (modeNumber - Numbers.IndexOffset) % _pattern.Count;
        int modalSemitonesOffset = _pattern[modeNumber];
        List<int> modalIntervals = [];
        for (int i = Numbers.Zero; i < _pattern.Count; i++)
        {
            int newNoteIndex = (i + modeNumber) % _pattern.Count;
            int originalValue = _pattern[newNoteIndex];
            double n = (i + modeNumber) / _pattern.Count;
            int semitonesOffset = (int)Math.Floor(n) * Numbers.SemitonesInOctave;
            int newValue = originalValue - modalSemitonesOffset + semitonesOffset;
            if (newValue < Numbers.Zero)
                newValue *= Numbers.FlatModifier;
            modalIntervals.Add(newValue);
        }
        return modalIntervals;
    }

    public static List<int> GetDoubleOctave(List<int> pattern) 
        => [.. pattern, .. pattern.Select(i => i + Numbers.SemitonesInOctave)];
}
