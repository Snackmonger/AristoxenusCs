using Aristoxenus.Core.Data;

namespace Aristoxenus.Core.Utilities;

public static class PatternHelper
{
    /// <summary>
    /// Return the <paramref name="pattern"/>'s values, rotated so that the 
    /// value at the given <paramref name="modeNumber"/> is now considered 0, 
    /// and the others are offset by a compensatory amount, wrapping around 
    /// to 1 after 12. 
    /// 
    /// E.g. [0, 2, 4, 5, 7, 9, 11] mode 2 => [0, 2, 3, 5, 7, 9, 10]
    /// </summary>
    public static List<int> GetModeOfIntervalPattern(IEnumerable<int> pattern, int modeNumber)
    {
        List<int> _pattern = [.. pattern.ToHashSet().Order()];
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

    /// <summary>
    /// Return true if the <paramref name="pattern"/> (or one of its inversions /
    /// modes) contains the given <paramref name="subPattern"/>.
    /// </summary>
    public static bool TryGetModalSubset(IEnumerable<int> pattern, IEnumerable<int> subPattern, out int mode)
    {
        mode = default;
        for (int i = default; i < pattern.Count(); i++)
        {
            mode = i + Numbers.IndexOffset;
            HashSet<int> modalRotation = [.. GetModeOfIntervalPattern(pattern, mode)];
            if (modalRotation.IsSupersetOf(subPattern))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Return true if the <paramref name="pattern"/> (or one of its inversions /
    /// modes) is equal to the given <paramref name="otherPattern"/>.
    /// </summary>
    public static bool TryGetModalRotation(IEnumerable<int> pattern, IEnumerable<int> otherPattern, out int mode)
    {
        mode = default;
        for (int i = default; i < otherPattern.Count(); i++)
        {
            mode = i + Numbers.IndexOffset;
            HashSet<int> modalRotation = [.. GetModeOfIntervalPattern(otherPattern, mode)];
            if (modalRotation.SetEquals(pattern))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Return true if the given pattern of integers can be recognized as 
    /// a known scale, and if so, write its name and mode to the out 
    /// parameters.
    /// </summary>
    public static bool TryIdentifyScale(IEnumerable<int> pattern, out string? name, out int? mode)
    {
        name = default;
        mode = default;
        foreach (var (scaleName, scalePattern) in RawScalePatterns.AllScales)
            if (TryGetModalRotation(pattern, scalePattern, out int modeNumber))
            {
                name = scaleName;
                mode = modeNumber;
                return true;
            }
        return false;
    }

    /// <summary>
    /// Extend the given pattern by repeating its intervals again an octave
    /// higher.
    /// </summary>
    public static List<int> GetDoubleOctave(IEnumerable<int> pattern)
        => [.. pattern, .. pattern.Select(i => i + Numbers.SemitonesInOctave)];
}
