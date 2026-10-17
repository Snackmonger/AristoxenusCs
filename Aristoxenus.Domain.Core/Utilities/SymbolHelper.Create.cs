using Libraries.Arithmology;

using Aristoxenus.Domain.Core.Data;

namespace Aristoxenus.Domain.Core.Utilities;

public static partial class SymbolHelper
{
    public static string CreatePitchSymbol((int, int) name, int octave)
        => CreateNoteNameSymbol(name) + octave.ToString();

    public static string CreateAccidentalSymbol(int accidental)
    {
        if (accidental == Numbers.Zero)
            return string.Empty;
        if (accidental > Numbers.Zero)
            return string.Concat(Enumerable.Repeat(Symbols.Sharp, Math.Abs(accidental % Numbers.SemitonesInOctave)));
        else
            return string.Concat(Enumerable.Repeat(Symbols.Flat, Math.Abs(accidental % Numbers.SemitonesInOctave)));
    }

    /// <summary>
    /// Get the longform string representation of an accidental name
    /// (e.g. "double sharp").
    /// </summary>
    public static string CreateAccidentalName(int Value)
    {
        return Value switch
        {
            0 => string.Empty,
            1 => Keywords.Sharp,
            -1 => Keywords.Flat,
            > 0 => string.Join(Symbols.Whitespace, Arithmology.Uple(Value), Keywords.Sharp),
            _ => string.Join(Symbols.Whitespace, Arithmology.Uple(Math.Abs(Value)), Keywords.Flat)
        };
    }

    public static string CreatePitchClassSymbol(int nameIndex)
        => Symbols.NaturalNoteNames[nameIndex % Numbers.NotesInOctave].ToString();

    public static string CreateScaleDegreeSymbol((int degree, int accidental) interval)
        => CreateAccidentalSymbol(interval.accidental) + interval.degree.ToString();


    public static string CreateNoteNameSymbol((int index, int accidental) noteName)
        => CreatePitchClassSymbol(noteName.index) + CreateAccidentalSymbol(noteName.accidental);

}
