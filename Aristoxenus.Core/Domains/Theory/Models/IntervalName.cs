using Libraries.Arithmology;
using Libraries.RomanNumerals;

using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Interfaces;
using Aristoxenus.Core.Shared.Utilities;

namespace Aristoxenus.Core.Domains.Theory.Models;

public class IntervalName : ValueObject, ISymbolSerializable<IntervalName>
{
    /// <summary>
    /// The degree of the interval, expressed as a number between 1 and 7.
    /// </summary>
    public readonly int Degree;

    /// <summary>
    /// The positive or negative amount by which the normal value of the 
    /// degree is skewed higher or lower.
    /// </summary>
    public readonly int Accidental;

    /// <summary>
    /// The raw value of the interval name, without the perspective of 
    /// the name (e.g. #4 and b5 both have a value of 6).
    /// </summary>
    public readonly int Value;

    /// <summary>
    /// The value of the interval, if it were situated in the first octave
    /// (e.g. a value of 14 becomes 2, while a value of 2 stays the same).
    /// </summary>
    public readonly int FirstOctaveValue;

    /// <summary>
    /// The value of the interval, if it were situated in the second octave
    /// (e.g. a value of 2 becomes 14, while a value of 14 stays the same).
    /// </summary>
    public readonly int SecondOctaveValue;

    /// <summary>
    /// The Roman numeral symbol for this interval (e.g. "#II")
    /// </summary>
    public readonly string RomanName;

    /// <summary>
    /// The cardinal symbol for this interval name (e.g. "sharp two")
    /// </summary>
    public readonly string LongCardinalName;

    /// <summary>
    /// The ordinal symbol for this interval name (e.g. "sharp second")
    /// </summary>
    public readonly string LongOrdinalName;

    // Constructors, Factories ////////////////////////////////////////////////
    public IntervalName(int degree, int accidental)
    {
        Accidental = ValidationHelper.NormalizeAccidental(accidental);
        Degree = ValidationHelper.NormalizeDegree(degree);
        Value = PatternHelper
            .GetDoubleOctave([.. RawScalePatterns.Diatonic])[
                (Degree - 1) % (Numbers.NotesInOctave * 2)
            ] + accidental;
        FirstOctaveValue = Value < Numbers.SemitonesInOctave
            ? Value
            : Value % Numbers.SemitonesInOctave;
        SecondOctaveValue = FirstOctaveValue + Numbers.SemitonesInOctave;
        RomanName = SymbolHelper.CreateAccidentalSymbol(accidental)
            + RomanNumerals.Encode(degree % (Numbers.NotesInOctave * 2));
        LongCardinalName = $"{ NomenclatureHelper.GetAccidentalName(accidental) } { Arithmology.Cardinal(degree) }";
        LongOrdinalName = $"{ NomenclatureHelper.GetAccidentalName(accidental) } { Arithmology.Ordinal(degree) }";
    }

    /// <summary>
    /// Create a new instance with the degree named as given, plus as many
    /// accidentals as necessary to provide the expected value.
    /// </summary>
    /// <param name="expectedDegree">The degree you want the interval to be (e.g. 5)</param>
    /// <param name="expectedValue">The value you want the interval to have (e.g. 6)</param>
    /// <remarks>Note that any value greater than two octaves will be reduced
    /// to fit within two octaves.</remarks>
    /// <returns>An interval name with the desired degree and value (e.g. b5)</returns>
    public static IntervalName FromExpectedValue(int expectedDegree, int expectedValue) 
        => NomenclatureHelper
            .GetIntervalNameFromExpectedValue(expectedDegree, expectedValue);


    public static IntervalName FromSymbol(string intervalSymbol) 
        => SymbolHelper.ParseIntervalNameSymbol(intervalSymbol);
    public string ToSymbol() => SymbolHelper.CreateIntervalNameSymbol(this);
    static IntervalName ISymbolSerializable<IntervalName>.FromSymbol(string symbol)
        => SymbolHelper.ParseIntervalNameSymbol(symbol);
    string ISymbolSerializable<IntervalName>.ToSymbol() => ToSymbol();

    public void Deconstruct(out int degree, out int accidental) 
        => (degree, accidental) = (Degree, Accidental);


    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Degree;
        yield return Accidental;
    }


    public static implicit operator (int, int)(IntervalName intervalName) 
        => (intervalName.Degree, intervalName.Accidental);

    public static implicit operator IntervalName((int degree, int accidental) intervalName) 
        => new(intervalName.degree, intervalName.accidental);

    public static implicit operator string(IntervalName intervalName) 
        => SymbolHelper.CreateIntervalNameSymbol(intervalName);

    public static implicit operator IntervalName(string intervalName) 
        => SymbolHelper.ParseIntervalNameSymbol(intervalName);
}
