using Aristoxenus.Constants;
using Aristoxenus.Domain.Exceptions;
using Aristoxenus.SharedKernel.Bases;
using Aristoxenus.SharedKernel.Interfaces;
using Aristoxenus.Utilities;

namespace Aristoxenus.Domain.Models;

/// <summary>
/// The numerical degree by which a scale step or interval can be named.
/// </summary>
public class Degree : ValueObject, ISymbolSerializable<Degree>
{
    public readonly int Value;

    public Degree One => CreateNew(1);
    public Degree Two => CreateNew(2);
    public Degree Three => CreateNew(3);
    public Degree Four => CreateNew(4);
    public Degree Five => CreateNew(5);
    public Degree Six => CreateNew(6);
    public Degree Seven => CreateNew(7);
    public Degree Eight => CreateNew(8);
    public Degree Nine => CreateNew(9);
    public Degree Ten => CreateNew(10);
    public Degree Eleven => CreateNew(11);
    public Degree Twelve => CreateNew(12);
    public Degree Thirteen => CreateNew(13);
    public Degree Fourteen => CreateNew(14);
    public Degree Fifteen => CreateNew(15);

    private Degree(int value)
    {
        Value = value;
    }

    public static Degree CreateNew(int degreeValue)
        => degreeValue is < 1 or > 15
            ? throw new IntegerOutOfRange(1, 15, nameof(degreeValue), degreeValue)
            : new Degree(degreeValue);

    public static Degree CreateNew(string degreeSymbol)
    {
        return FromSymbol(degreeSymbol);
    }

    /// <summary>
    /// The value of the degree, but projected into the second octave,
    /// e.g. 6 becomes 13, etc. If the degree is already in the second
    /// octave, this simply returns the degree.
    /// </summary>
    public int SecondOctaveValue
        => FirstOctaveValue == Value
            ? Value + Numbers.NotesInOctave
            : Value;

    /// <summary>
    /// The value of the degree, but restricted to a single octave,
    /// e.g. 13 becomes 6, etc. If the degree is already in the first 
    /// octave, this simply returns the degree.
    /// </summary>
    public int FirstOctaveValue
        => Value % Numbers.NotesInOctave;

    /// <summary>
    /// The short name, e.g. "1", "2", etc.
    /// </summary>
    public string Symbol
        => Value.ToString();

    /// <summary>
    /// The Roman numeral name, e.g. "I", "II", etc.
    /// </summary>
    public string Roman
        => RomanNumerals.Encode(Value % (Numbers.NotesInOctave * 2));

    /// <summary>
    /// The cardinal name, e.g. "one", "two", etc.
    /// </summary>
    public string Cardinal
        => Arithmology.Cardinal(Value);

    /// <summary>
    /// The ordinal name, e.g. "first", "second", etc.
    /// </summary>
    public string Ordinal
        => Arithmology.Ordinal(Value);


    /// <summary>
    /// Create a new instance of the object from a string symbol.
    /// </summary>
    public static Degree FromSymbol(string symbol)
    {
        return CreateNew(int.Parse(symbol));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
