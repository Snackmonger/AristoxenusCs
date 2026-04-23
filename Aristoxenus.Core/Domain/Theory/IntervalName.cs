namespace Aristoxenus.Core.Domain.Theory;

using System.Collections.Generic;
using System.Text.RegularExpressions;

using Aristoxenus.Core.Constants;
using Aristoxenus.Core.SharedKernel.Bases;
using Aristoxenus.Core.SharedKernel.Interfaces;
using Aristoxenus.Core.Utilities;
using Aristoxenus.Core.Domain.Exceptions;

public class IntervalName : ValueObject, ISymbolSerializable<IntervalName>
{

    public readonly Degree Degree;
    public readonly Accidental Accidental;

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
    private IntervalName(Degree degree, Accidental accidental)
    {
        Degree = degree;
        Accidental = accidental;
        Value = ScalePatternsHeptatonic
                .Diatonic
                .DoubleOctave[(Degree.Value - 1) % (Numbers.NotesInOctave * 2)]
                + Accidental.Value;
        FirstOctaveValue = Value < Numbers.SemitonesInOctave
            ? Value
            : Value % Numbers.SemitonesInOctave;
        SecondOctaveValue = FirstOctaveValue + Numbers.SemitonesInOctave;
        RomanName = Accidental.ToSymbol() + Degree.Roman;
        LongCardinalName = $"{Accidental.Name} {Degree.Cardinal}";
        LongOrdinalName = $"{Accidental.Name} {Degree.Ordinal}";
    }

    public static IntervalName CreateNew(Degree degree, Accidental accidental)
        => new(degree, accidental);

    public static IntervalName CreateNew(int degree, int? accidental)
        => CreateNew(
            Degree.CreateNew(degree),
            Accidental.CreateNew(accidental ?? 0)
        );

    /// <summary>
    /// Create a new instance with the degree named as given, plus as many
    /// accidentals as necessary to provide the expected value.
    /// </summary>
    /// <param name="expectedDegree">
    /// The degree you want the interval to be (e.g. 5)
    /// </param>
    /// <param name="expectedValue">
    /// The value you want the interval to have (e.g. 6)
    /// </param>
    /// <returns>
    /// An interval name with the desired degree and value.
    /// </returns>
    public static IntervalName FromExpectedValue(int expectedDegree, int expectedValue)
    {
        expectedValue = Math.Abs(expectedValue);
        // If the value is anything more than two octaves, reduce it to fit
        // within two octaves.
        expectedValue = expectedValue > (Numbers.SemitonesInOctave * 2)
            ? (expectedValue % Numbers.SemitonesInOctave) + Numbers.SemitonesInOctave
            : expectedValue;
        int normalValue = ScalePatternsHeptatonic.Diatonic.DoubleOctave[expectedDegree - Numbers.IndexOffset];
        Accidental accidental = Accidental.CreateNew(expectedValue - normalValue);
        return CreateNew(Degree.CreateNew(expectedDegree), accidental);
    }

    // ISymbolSerializable ////////////////////////////////////////////////////
    public static IntervalName FromSymbol(string intervalSymbol)
    {
        Match data = RegularExpressions.IntervalSymbol.Match(intervalSymbol);
        if (!data.Success)
        {
            throw new InvalidAsSymbol(intervalSymbol, nameof(intervalSymbol));
        }
        GroupCollection groups = data.Groups;
        int accidentalIdx = 1;
        int nameIdx = 2;
        string name = groups[nameIdx].Value;
        int degree = name.All(char.IsDigit)
            ? int.Parse(name)
            : RomanNumerals.Decode(name.ToUpper());
        var accidental = Accidental.Natural;
        if (groups[accidentalIdx] != null)
        {
            // The actual string will only ever
            // have one or the other, per the regex.
            int sharps = RegularExpressions.SharpSymbol.Count(groups[accidentalIdx].Value);
            int flats = RegularExpressions.FlatSymbol.Count(groups[accidentalIdx].Value);
            accidental = Accidental.CreateNew(sharps - flats);
        }
        return CreateNew(Degree.CreateNew(degree), accidental);
    }
    public string ToSymbol()
        => Accidental.ToSymbol() + Degree.ToSymbol();

    static IntervalName ISymbolSerializable<IntervalName>.FromSymbol(string symbol)
        => FromSymbol(symbol);

    string ISymbolSerializable<IntervalName>.ToSymbol()
        => ToSymbol();

    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ToSymbol();
    }
 
}
