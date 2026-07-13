namespace Aristoxenus.Core.Data;

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
    public const int SemitonesInDoubleOctave = SemitonesInOctave * 2;
    public const int NotesInOctave = 7;
    public const int NotesInDoubleOctave = NotesInOctave * 2;
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
}








