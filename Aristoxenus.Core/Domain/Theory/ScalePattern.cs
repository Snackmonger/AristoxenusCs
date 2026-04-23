namespace Aristoxenus.Core.Domain.Theory;

using System.Collections;
using System.Collections.Immutable;

using Aristoxenus.Core.Constants;
using Aristoxenus.Core.SharedKernel.Bases;

/// <summary>
/// A representation of the pattern of semitones that make up a scale.
/// </summary>
public sealed class ScalePattern : ValueObject, IReadOnlyList<int>
{
    public ImmutableList<int> Pattern { get; init; }
    public List<int> DoubleOctave { get; init; }
    public int Count { get; init; }

    // Constructors, Factories //////////////////////////////////////////////////////////
    internal ScalePattern(IEnumerable<int> value)
    {
        Pattern = [.. value];
        Count = Pattern.Count;
        DoubleOctave = [.. Pattern, .. Pattern.Select(n => n + Numbers.SemitonesInOctave)];
    }

    public ScalePattern CreateNew(IEnumerable<int> value)
    {
        value = [Numbers.Zero, .. value];
        return new(value.ToHashSet().Order());
    }


    /// <summary>
    /// Return this pattern's values, rotated so that the value at the given
    /// <paramref name="modeNumber"/> is now considered 0, and the others
    /// are offset by a compensatory amount, wrapping around to 1 after 12.
    /// E.g. [0, 2, 4, 5, 7, 9, 11] mode 2 => [0, 2, 3, 5, 7, 9, 10]
    /// </summary>
    /// <param name="modeNumber"></param>
    /// <returns></returns>
    public List<int> Mode(int modeNumber)
    {
        modeNumber = (modeNumber - Numbers.IndexOffset) % Count;
        int modalSemitonesOffset = Pattern[modeNumber];
        List<int> modalIntervals = [];
        for (int i = Numbers.Zero; i < Count; i++)
        {
            int newNoteIndex = (i + modeNumber) % Count;
            int originalValue = Pattern[newNoteIndex];
            double n = (i + modeNumber) / Count;
            int semitonesOffset = (int)Math.Floor(n) * Numbers.SemitonesInOctave;
            int newValue = originalValue - modalSemitonesOffset + semitonesOffset;
            if (newValue < Numbers.Zero)
                newValue *= Numbers.FlatModifier;
            modalIntervals.Add(newValue);
        }
        return modalIntervals;
    }

    // List-like behaviors ////////////////////////////////////////////////////
    public int this[int index] => Pattern[index];

    /// <summary>
    /// Determines whether an interval value is in the pattern.
    /// </summary>
    public bool Contains(int value) => Pattern.Contains(value);

    /// <summary>
    /// Returns the index of a given value in this pattern.
    /// </summary>
    public int IndexOf(int value) => Pattern.IndexOf(value);

    // IEnumerable ////////////////////////////////////////////////////////////
    public IEnumerator<int> GetEnumerator()
        => Pattern.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (int i in Pattern)
        {
            yield return i;
        }
    }

}
