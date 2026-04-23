namespace Aristoxenus.Core.Constants;

/// <summary>
/// Numeric values central to the basic logic of the program.
/// </summary>
public class Numbers
{
    // Misc values to avoid common magic numbers
    public const int IndexOffset = 1;
    public const int Zero = 0;

    // Primary assumptions: heptatonic scales in 12-tone
    // equal temperament with octave eqivalence.
    public const int SemitonesInOctave = 12;
    public const int NotesInOctave = 7;
    public const int FlatModifier = -1;
    public const int SharpModifier = 1;
    public const int OctaveEquvalenceFactor = 2;

    // Range limits
    public const double MinFrequency = 16.35;
    public const double MaxFrequency = 7902.13;
    public const int MinOctaveNumber = 1;
    public const int MaxOctaveNumber = 8;
    public const int MaxAccidentals = SemitonesInOctave - IndexOffset;
    public const int CentralMidiNumber = 69;
    public const double CentralFrequency = 440.0;

    // Core values for legal note durations in different divisions (borrowed from
    // https://bspaans.github.io/python-mingus/doc/wiki/refMingusCoreValue.html)
    public static readonly List<double> BaseDurations =  [0.25, 0.5, 1, 2, 4, 8, 16, 32, 64, 128];
    public static readonly List<double> BaseTriplets = [0.375, 0.75, 1.5, 3, 6, 12, 24, 48, 96, 192];
    public static readonly List<double> BaseSeptuplets = [0.4375, 0.875, 1.75, 3.5, 7, 14, 28, 56, 112, 224];
    public static readonly List<double> BaseQuintuppets = [0.3125, 0.625, 1.25, 2.5, 5, 10, 20, 40, 80, 160];
}








