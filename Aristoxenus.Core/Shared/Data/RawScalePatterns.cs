namespace Aristoxenus.Core.Shared.Data;


/// <summary>
/// Collections of integers representing the intervals of different scale
/// patterns.
/// </summary>
public class RawScalePatterns
{
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
}
