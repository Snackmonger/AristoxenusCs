using System.Collections;

using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Utilities;

namespace Aristoxenus.Core.Domains.Theory.Atoms;

/// <summary>
/// A representation of the pattern of semitones that make up a scale.
/// </summary>
public sealed class ScalePattern : ValueObject, IReadOnlyList<int>
{
    public List<int> Pattern { get; init; }
    public List<int> DoubleOctave { get; init; }
    public int Count { get; init; }

    // Constructors, Factories //////////////////////////////////////////////////////////
    public ScalePattern(IEnumerable<int> value)
    {
        value = [Numbers.Zero, .. value.Select(n => n % Numbers.SemitonesInOctave)];
        Pattern = [.. value.ToHashSet().Order()];
        Count = Pattern.Count;
        DoubleOctave = [.. Pattern, .. Pattern.Select(n => n + Numbers.SemitonesInOctave)];
    }

    public static readonly Dictionary<string, int[]> ScaleMap = new() {
        { Keywords.Diatonic, RawScalePatterns.Diatonic},
        { Keywords.Altered, RawScalePatterns.Altered },
        { Keywords.Hemitonic, RawScalePatterns.Hemitonic },
        { Keywords.Hemiolic, RawScalePatterns.Hemiolic },
        { Keywords.Diminished, RawScalePatterns.Diminished },
        { Keywords.Augmented, RawScalePatterns.Augmented },
        { Keywords.Harmonic, RawScalePatterns.Harmonic },
        { Keywords.Biseptimal, RawScalePatterns.Biseptimal },
        { Keywords.Paleochromatic, RawScalePatterns.Paleochromatic },
        { Keywords.Enigmatic, RawScalePatterns.Enigmatic },
        { Keywords.DoubleHarmonic, RawScalePatterns.DoubleHarmonic },
        { Keywords.Neapolitan, RawScalePatterns.Neapolitan },
        { Keywords.Hungarian, RawScalePatterns.Hungarian },
        { Keywords.Persian, RawScalePatterns.Persian },
        { Keywords.Romanian, RawScalePatterns.Romanian}
    };

    public static readonly ScalePattern Diatonic = new(RawScalePatterns.Diatonic);
    public static readonly ScalePattern Altered = new(RawScalePatterns.Altered);
    public static readonly ScalePattern Hemitonic = new(RawScalePatterns.Hemitonic);
    public static readonly ScalePattern Hemiolic = new(RawScalePatterns.Hemiolic);
    public static readonly ScalePattern Diminished = new(RawScalePatterns.Diminished);
    public static readonly ScalePattern Augmented = new(RawScalePatterns.Augmented);
    public static readonly ScalePattern Biseptimal = new(RawScalePatterns.Biseptimal);
    public static readonly ScalePattern Paleochromatic = new(RawScalePatterns.Paleochromatic);
    public static readonly ScalePattern Enigmatic = new(RawScalePatterns.Enigmatic);
    public static readonly ScalePattern Persian = new(RawScalePatterns.Persian);
    public static readonly ScalePattern Hungarian = new(RawScalePatterns.Hungarian);
    public static readonly ScalePattern DoubleHarmonic = new(RawScalePatterns.DoubleHarmonic);
    public static readonly ScalePattern Neapolitan = new(RawScalePatterns.Neapolitan);
    public static readonly ScalePattern Romanian = new(RawScalePatterns.Romanian);

    /// <summary>
    /// Return this pattern's values, rotated so that the value at the given
    /// <paramref name="modeNumber"/> is now considered 0, and the others
    /// are offset by a compensatory amount, wrapping around to 1 after 12.
    /// E.g. [0, 2, 4, 5, 7, 9, 11] mode 2 => [0, 2, 3, 5, 7, 9, 10]
    /// </summary>
    /// <param name="modeNumber"></param>
    /// <returns></returns>
    public List<int> Mode(int modeNumber) 
        => NomenclatureHelper.GetModeOfIntervalPattern(Pattern, modeNumber);

    // List-like behaviors ////////////////////////////////////////////////////
    public int this[int index] 
        => Pattern[index];

    /// <summary>
    /// Determines whether an interval value is in the pattern.
    /// </summary>
    public bool Contains(int value) 
        => Pattern.Contains(value);

    /// <summary>
    /// Returns the index of a given value in this pattern.
    /// </summary>
    public int IndexOf(int value) 
        => Pattern.IndexOf(value);

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
