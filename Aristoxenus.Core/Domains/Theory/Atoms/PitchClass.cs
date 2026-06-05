using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Interfaces;

namespace Aristoxenus.Core.Domains.Theory.Atoms;

/// <summary>
/// Represents the alphabetic part of a note name, which may also be thought
/// of as a position in the diatonic scale.
/// </summary>
public class PitchClass : ValueObject, ISymbolSerializable<PitchClass>
{
    public readonly int Value;

    public PitchClass(int value)
    {
        if (value < Numbers.Zero || value > Numbers.NotesInOctave - Numbers.IndexOffset)
            throw new ArgumentException("Pitch class must be between 0 and 6.");
        Value = value;
    }

    // ValueObject //////////////////////////////////////////////////

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    // ISymbolSerializable<PitchClass> ////////////////////////////////////////
    public static PitchClass FromSymbol(string symbol)
        => Symbols.NaturalNoteNames.Contains(symbol.ToUpper())
            ? new(Symbols.NaturalNoteNames.IndexOf(symbol.ToUpper()))
            : throw new ArgumentException($"Pitch class must be one of {Symbols.NaturalNoteNames}, got {symbol}");

    string ISymbolSerializable<PitchClass>.ToSymbol()
        => ToSymbol();

    static PitchClass ISymbolSerializable<PitchClass>.FromSymbol(string symbol)
        => FromSymbol(symbol);

    public string ToSymbol() 
        => Symbols.NaturalNoteNames[Value].ToString();

    // Casts //
    public static implicit operator int(PitchClass p) => p.Value;
    public static implicit operator PitchClass(int i) => new(i);

}
