using Aristoxenus.Core.Shared.Data;

namespace Aristoxenus.Core.Shared.Utilities;

public static partial class SymbolHelper
{
    public static string CreatePitchSymbol((int, int) name, int octave)
        => CreateNoteNameSymbol(name) + octave.ToString();

    public static string CreateAccidentalSymbol(int accidental)
    {
        if (accidental == Numbers.Zero)
            return string.Empty;
        if (accidental > Numbers.Zero)
            return string.Concat(Enumerable.Repeat(Symbols.Sharp, Math.Abs(accidental % Numbers.MaxAccidentals)));
        else
            return string.Concat(Enumerable.Repeat(Symbols.Flat, Math.Abs(accidental % Numbers.MaxAccidentals)));
    }

    public static string CreatePitchClassSymbol(int nameIndex)
        => Symbols.NaturalNoteNames[nameIndex % Numbers.NotesInOctave].ToString();

    public static string CreateIntervalNameSymbol((int degree, int accidental) interval)
        => CreateAccidentalSymbol(interval.accidental) + CreateDegreeSymbol(interval.degree);

    public static string CreateDegreeSymbol(int degree)
    {
        int value = degree % (Numbers.NotesInOctave * 2 + 1);
        return value.ToString();
    }

    public static string CreateNoteNameSymbol((int index, int accidental) noteName)
        => CreatePitchClassSymbol(noteName.index) + CreateAccidentalSymbol(noteName.accidental);

}
