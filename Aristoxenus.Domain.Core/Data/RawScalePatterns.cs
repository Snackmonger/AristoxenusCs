namespace Aristoxenus.Domain.Core.Data;


/// <summary>
/// Collections of integers representing the intervals of different scale
/// patterns.
/// </summary>
public class RawScalePatterns
{
    // Heptatonic Scales
    public static readonly int[] Diatonic = [0, 2, 4, 5, 7, 9, 11];
    public static readonly int[] Altered = [1, 3, 4, 6, 8, 9, 10];
    public static readonly int[] Hemitonic = [0, 1, 4, 5, 7, 9, 11];
    public static readonly int[] Hemiolic = [0, 3, 4, 5, 7, 9, 11];
    public static readonly int[] Diminished = [0, 2, 4, 5, 6, 9, 11];
    public static readonly int[] Augmented = [0, 2, 4, 5, 8, 9, 11];
    public static readonly int[] Harmonic = [0, 2, 4, 5, 7, 8, 11];
    public static readonly int[] Biseptimal = [0, 2, 4, 5, 7, 10, 11];
    public static readonly int[] Paleochromatic = [0, 1, 4, 5, 6, 9, 11];
    public static readonly int[] Enigmatic = [0, 1, 4, 6, 8, 10, 11];
    public static readonly int[] DoubleHarmonic = [0, 1, 4, 5, 7, 8, 11];
    public static readonly int[] Neapolitan = [0, 1, 3, 5, 7, 9, 11];
    public static readonly int[] Hungarian = [0, 3, 4, 6, 7, 9, 10];
    public static readonly int[] Persian = [0, 1, 4, 5, 6, 8, 11];
    public static readonly int[] Romanian = [0, 1, 4, 6, 7, 9, 1];

    // Octatonic Scales
    public static readonly int[] Major6Diminished = [0, 2, 4, 5, 7, 8, 9, 11];
    public static readonly int[] Minor6Diminished = [0, 2, 3, 5, 7, 8, 10, 11];
    public static readonly int[] Dominant7Diminished = [0, 2, 4, 5, 7, 8, 9, 10];
    public static readonly int[] Dominant7b5Diminished = [0, 2, 4, 5, 6, 8, 9, 10];
    public static readonly int[] SymmetricalDiminished = [0, 2, 3, 5, 6, 8, 9, 11];

    // Hexatonic Scales
    public static readonly int[] WholeTone = [0, 2, 4, 6, 8, 10];
    public static readonly int[] SymmetricalAugmented = [0, 2, 4, 6, 8, 10];
    public static readonly int[] MajorHexatonic = [0, 2, 4, 5, 7, 9];
    public static readonly int[] Prometheus = [0, 2, 4, 6, 9, 10];

    // Pentatonic Scales
    public static readonly int[] MajorPentatonic = [0, 2, 4, 7, 9];

    public static int[] GetScalePattern(string scaleName)
        => [.. _allScales[scaleName]];
    public static int[] GetDoubleOctaveOfPattern(string scaleName)
        => [.. _allScales[scaleName], .. _allScales[scaleName].Select(it => it + Numbers.SemitonesInOctave)];
    public static IReadOnlyDictionary<string, int[]> AllScales
        => _allScales.AsReadOnly();
    private static Dictionary<string, int[]> _allScales
        => new()
        {
            // Heptaonic Scales
            { nameof(Diatonic), Diatonic },
            { nameof(Altered), Altered },
            { nameof(Hemitonic), Hemitonic },
            { nameof(Hemiolic), Hemiolic },
            { nameof(Diminished), Diminished },
            { nameof(Augmented), Augmented },
            { nameof(Harmonic), Harmonic },
            { nameof(Biseptimal), Biseptimal },
            { nameof(Paleochromatic), Paleochromatic },
            { nameof(Enigmatic), Enigmatic },
            { nameof(DoubleHarmonic), DoubleHarmonic },
            { nameof(Neapolitan), Neapolitan },
            { nameof(Hungarian), Hungarian },
            { nameof(Persian), Persian },
            { nameof(Romanian), Romanian },

            // Octatonic Scales
            { nameof(Major6Diminished), Major6Diminished },
            { nameof(Minor6Diminished), Minor6Diminished },
            { nameof(Dominant7Diminished), Dominant7Diminished },
            { nameof(Dominant7b5Diminished), Dominant7b5Diminished },
            { nameof(SymmetricalDiminished), SymmetricalDiminished },

            // Hexatonic Scales
            { nameof(WholeTone), WholeTone },
            { nameof(SymmetricalAugmented), SymmetricalAugmented },
            { nameof(MajorHexatonic), MajorHexatonic },
            { nameof(Prometheus), Prometheus },

            // Pentatonic Scales
            { nameof(MajorPentatonic), MajorPentatonic }
        };
    
}
