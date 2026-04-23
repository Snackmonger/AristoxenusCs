using Aristoxenus.Core.Constants;
using Aristoxenus.Core.Domain.Exceptions;
using Aristoxenus.Core.SharedKernel.Bases;
using Aristoxenus.Core.SharedKernel.Interfaces;
using Aristoxenus.Core.Utilities;

namespace Aristoxenus.Core.Domain.Theory;

/// <summary>
/// The numerical degree by which a scale step or interval can be named.
/// </summary>
public class Degree : ValueObject, ISymbolSerializable<Degree>
{
    /// <summary>
    /// The integer value of this degree.
    /// </summary>
    public readonly int Value;

    /// <summary>
    /// The value of the degree, but projected into the second octave,
    /// e.g. 6 becomes 13, etc. If the degree is already in the second
    /// octave, this simply returns the degree.
    /// </summary>
    public readonly int SecondOctaveValue;

    /// <summary>
    /// The value of the degree, but restricted to a single octave,
    /// e.g. 13 becomes 6, etc. If the degree is already in the first 
    /// octave, this simply returns the degree.
    /// </summary>
    public readonly int FirstOctaveValue;

    /// <summary>
    /// The Roman numeral name, e.g. "I", "II", etc.
    /// </summary>
    public readonly string Roman;

    /// <summary>
    /// The cardinal name, e.g. "one", "two", etc.
    /// </summary>
    public readonly string Cardinal;

    /// <summary>
    /// The ordinal name, e.g. "first", "second", etc.
    /// </summary>
    public readonly string Ordinal;

    // Constructors, Factories ////////////////////////////////////////////////
    private Degree(int value)
    {
        Value = value;
        Roman = RomanNumerals.Encode(Value % (Numbers.NotesInOctave * 2));
        Cardinal = Arithmology.Cardinal(Value);
        Ordinal = Arithmology.Ordinal(Value);
        FirstOctaveValue = Value % Numbers.NotesInOctave;
        SecondOctaveValue = FirstOctaveValue == Value
            ? Value + Numbers.NotesInOctave
            : Value;
    }

    public static Degree CreateNew(int degreeValue)
        => degreeValue is < 1 or > 15
            ? throw new IntegerOutOfRange(1, 15, nameof(degreeValue), degreeValue)
            : new Degree(degreeValue);

    public static Degree CreateNew(string degreeSymbol)
    {
        return FromSymbol(degreeSymbol);
    }

    public static readonly Degree One = CreateNew(1);
    public static readonly Degree Two = CreateNew(2);
    public static readonly Degree Three = CreateNew(3);
    public static readonly Degree Four = CreateNew(4);
    public static readonly Degree Five = CreateNew(5);
    public static readonly Degree Six = CreateNew(6);
    public static readonly Degree Seven = CreateNew(7);
    public static readonly Degree Eight = CreateNew(8);
    public static readonly Degree Nine = CreateNew(9);
    public static readonly Degree Ten = CreateNew(10);
    public static readonly Degree Eleven = CreateNew(11);
    public static readonly Degree Twelve = CreateNew(12);
    public static readonly Degree Thirteen = CreateNew(13);
    public static readonly Degree Fourteen = CreateNew(14);
    public static readonly Degree Fifteen = CreateNew(15);

    // ISymbolSerializable ////////////////////////////////////////////////////
    string ISymbolSerializable<Degree>.ToSymbol()
        => ToSymbol();

    static Degree ISymbolSerializable<Degree>.FromSymbol(string symbol) 
        => FromSymbol(symbol);

    public string ToSymbol()
        => Value.ToString();

    public static Degree FromSymbol(string symbol)
        => CreateNew(int.Parse(symbol));

    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
}
