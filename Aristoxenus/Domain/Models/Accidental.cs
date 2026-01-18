namespace Aristoxenus.Domain.Models;

using Aristoxenus.Constants;
using Aristoxenus.Domain.Exceptions;
using Aristoxenus.SharedKernel.Bases;
using Aristoxenus.SharedKernel.Interfaces;
using Aristoxenus.Utilities;
using System.Collections.Generic;

/// <summary>
/// Represents the amount by which a note or interval's value has been 
/// adjusted compared to its base value (positive or negative).
/// </summary>
public class Accidental : ValueObject, ISymbolSerializable<Accidental>
{
    public readonly int Value;
    private Accidental(int value)
    {
        Value = value;
    }
    public static Accidental Sharp => CreateNew(1);
    public static Accidental Flat => CreateNew(-1);
    public static Accidental Natural => CreateNew(0);
    public static Accidental DoubleSharp => CreateNew(2);
    public static Accidental DoubleFlat => CreateNew(-2);
    public static Accidental TripleSharp => CreateNew(3);
    public static Accidental TripleFlat => CreateNew(-3);

    public static Accidental CreateNew(int value)
        => value is > 11 or < -11
        ? throw new AccidentalOutOfRange(value)
        : new Accidental(value);


    public static Accidental FromSymbol(string symbol)
    => CreateNew(
            RegularExpressions.SharpSymbol.Count(symbol)
            - RegularExpressions.FlatSymbol.Count(symbol));

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// A symbol representing this accidental's sharps or flats.
    /// 
    /// E.g.: 
    ///     +1 -> '#'
    ///     +2 -> '##'
    ///     -1 -> 'b'
    ///     -2 -> 'bb'
    ///     -4 -> 'bbbb'
    ///     +3 -> '###'
    /// </summary>
    public string Symbol
        => Value == 0
            ? string.Empty
            : Value > 0
                ? string.Concat(Enumerable.Repeat(Symbols.Sharp, Math.Abs(Value)))
                : string.Concat(Enumerable.Repeat(Symbols.Flat, Math.Abs(Value)));


    /// <summary>
    /// A full-name string representation of the accidental.
    /// E.g.:
    ///     +1 -> 'sharp'
    ///     +2 -> 'double sharp'
    ///     -1 -> 'flat'
    ///     -2 -> 'double flat'
    ///     -4 -> 'quadruple flat'
    ///     +3 -> 'triple sharp'
    /// </summary>
    public string Name
    {
        get
        {
            string kind;
            if (Value == 0)
            {
                return string.Empty;
            }
            if (Value == 1)
            {
                return Keywords.Sharp;
            }
            if (Value == -1)
            {
                return Keywords.Flat;
            }
            if (Value > 0)
            {
                kind = Keywords.Sharp;
            }
            else
            {
                kind = Keywords.Flat;
            }
            int absValue = Math.Abs(Value);
            return $"{Arithmology.Uple(absValue)} {kind}";

        }


    }


}

