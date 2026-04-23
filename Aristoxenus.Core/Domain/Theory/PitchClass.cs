using Aristoxenus.Core.Constants;
using Aristoxenus.Core.SharedKernel.Bases;
using Aristoxenus.Core.SharedKernel.Interfaces;

namespace Aristoxenus.Core.Domain.Theory;

/// <summary>
/// Represents the alphabetic part of a note name, which may also be thought
/// of as a position in the diatonic scale.
/// </summary>
public class PitchClass : ValueObject, ISymbolSerializable<PitchClass>
{
    public readonly int Value;

    private PitchClass(int value)
    {
        Value = value;
    }

    public static PitchClass CreateNew(int value)
        => value is < Numbers.Zero or > Numbers.NotesInOctave - Numbers.IndexOffset
            ? throw new ArgumentException("Pitch class must be between 0 and 6.")
            : new PitchClass(value);

    public static PitchClass CreateNew(string symbol)
        => FromSymbol(symbol);

    // ValueObject //////////////////////////////////////////////////

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    // ISymbolSerializable<PitchClass> ////////////////////////////////////////
    public static PitchClass FromSymbol(string symbol)
        => Symbols.NaturalNoteNames.Contains(symbol.ToUpper())
            ? CreateNew(Symbols.NaturalNoteNames.IndexOf(symbol.ToUpper()))
            : throw new ArgumentException($"Pitch class must be one of {Symbols.NaturalNoteNames}, got {symbol}");

    string ISymbolSerializable<PitchClass>.ToSymbol()
        => ToSymbol();

    static PitchClass ISymbolSerializable<PitchClass>.FromSymbol(string symbol)
        => FromSymbol(symbol);

    public string ToSymbol() 
        => Symbols.NaturalNoteNames[Value].ToString();

}
