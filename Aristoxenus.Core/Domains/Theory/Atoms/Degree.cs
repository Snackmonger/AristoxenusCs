using Libraries.Arithmology;
using Libraries.RomanNumerals;

using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Interfaces;
using Aristoxenus.Core.Shared.Utilities;

namespace Aristoxenus.Core.Domains.Theory.Atoms;

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
    public Degree(int value)
    {
        if (value is < 1 or > 15)
            throw new ArgumentOutOfRangeException(nameof(value), value, 
                $"Value must be between 1 and 15 (inclusive)."
        );
        Value = value;
        Roman = RomanNumerals.Encode(Value % (Numbers.NotesInOctave * 2));
        Cardinal = Arithmology.Cardinal(Value);
        Ordinal = Arithmology.Ordinal(Value);
        FirstOctaveValue = Value % Numbers.NotesInOctave;
        SecondOctaveValue = FirstOctaveValue == Value
            ? Value + Numbers.NotesInOctave
            : Value;
    }

    // ISymbolSerializable ////////////////////////////////////////////////////
    string ISymbolSerializable<Degree>.ToSymbol() => ToSymbol();

    static Degree ISymbolSerializable<Degree>.FromSymbol(string symbol) => FromSymbol(symbol);

    public string ToSymbol() => NomenclatureHelper.GetDegreeSymbol(Value);

    public static Degree FromSymbol(string symbol) => new(int.Parse(symbol));

    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    // Casts //
    public static implicit operator int(Degree degree) => degree.Value;
    public static implicit operator Degree(int value) => new(value);

}
