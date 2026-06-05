using Libraries.Arithmology;

using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Interfaces;
using Aristoxenus.Core.Shared.Utilities;

namespace Aristoxenus.Core.Domains.Theory.Atoms;

/// <summary>
/// Represents the amount by which a note or interval's value has been 
/// adjusted compared to its base value (positive or negative).
/// </summary>
public class Accidental : ValueObject, ISymbolSerializable<Accidental>
{

    /// <summary>
    /// The value of the accidental, expressed as a positive or negative
    /// integer.
    /// </summary>
    public readonly int Value;

    /// <summary>
    /// A full-name string representation of the accidental.
    /// </summary>
    public string Name;

    public Accidental(int value)
    {
        if (value is > Numbers.MaxAccidentals or < -Numbers.MaxAccidentals)
            throw new ArgumentOutOfRangeException(nameof(value), value, 
                $"Value must be between -{Numbers.MaxAccidentals} and {Numbers.MaxAccidentals}"
        );
        Value = value;
        Name = Value switch
        {
            0 => string.Empty,
            1 => Keywords.Sharp,
            -1 => Keywords.Flat,
            > 0 => string.Join(Symbols.Whitespace, Arithmology.Uple(Value), Keywords.Sharp),
            _ => string.Join(Symbols.Whitespace, Arithmology.Uple(Math.Abs(Value)), Keywords.Flat)
        };
    }

    public static readonly Accidental Natural = new(0);
    public static readonly Accidental Sharp = new(1);
    public static readonly Accidental Flat = new(-1);
    public static readonly Accidental DoubleSharp = new(2);
    public static readonly Accidental DoubleFlat = new(-2);


    // ISymbolSerializable ////////////////////////////////////////////////////
    public string ToSymbol() => NomenclatureHelper.GetAccidentalSymbol(Value);

    public static Accidental FromSymbol(string symbol)
        => new(RegularExpressions.SharpSymbol.Count(symbol) - RegularExpressions.FlatSymbol.Count(symbol));

    static Accidental ISymbolSerializable<Accidental>.FromSymbol(string symbol)
        => FromSymbol(symbol);

    string ISymbolSerializable<Accidental>.ToSymbol()
        => ToSymbol();

    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    // Casts
    public static implicit operator int(Accidental accidental) => accidental.Value;
    public static implicit operator Accidental(int value) => new(value);



}

