namespace Aristoxenus.Core.Domain.Theory;

using Aristoxenus.Core.Constants;
using Aristoxenus.Core.Domain.Exceptions;
using Aristoxenus.Core.SharedKernel.Bases;
using Aristoxenus.Core.SharedKernel.Interfaces;
using Aristoxenus.Core.Utilities;

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


    // Constructors, Factories ////////////////////////////////////////////////
    private Accidental(int value)
    {
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

    public static Accidental CreateNew(int value)
        => value is > Numbers.MaxAccidentals or < -Numbers.MaxAccidentals
        ? throw new AccidentalOutOfRange(value)
        : new Accidental(value);

    public static readonly Accidental Natural = CreateNew(0);
    public static readonly Accidental Sharp = CreateNew(1);
    public static readonly Accidental Flat = CreateNew(-1);
    public static readonly Accidental DoubleSharp = CreateNew(2);
    public static readonly Accidental DoubleFlat = CreateNew(-2);


    // ISymbolSerializable ////////////////////////////////////////////////////
    public string ToSymbol()
        => Value == Numbers.Zero ? string.Empty
            : Value > Numbers.Zero ? string.Concat(Enumerable.Repeat(Symbols.Sharp, Math.Abs(Value)))
            : string.Concat(Enumerable.Repeat(Symbols.Flat, Math.Abs(Value)));

    public static Accidental FromSymbol(string symbol)
        => CreateNew(
            RegularExpressions.SharpSymbol.Count(symbol)
            - RegularExpressions.FlatSymbol.Count(symbol));

    static Accidental ISymbolSerializable<Accidental>.FromSymbol(string symbol)
        => FromSymbol(symbol);

    string ISymbolSerializable<Accidental>.ToSymbol()
        => ToSymbol();

    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }




}

