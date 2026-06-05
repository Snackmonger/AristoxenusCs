using System.Text.RegularExpressions;

using Libraries.RomanNumerals;

using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Errors;
using Aristoxenus.Core.Shared.Interfaces;
using Aristoxenus.Core.Shared.Utilities;

namespace Aristoxenus.Core.Domains.Theory.Atoms;

public class IntervalName : ValueObject, ISymbolSerializable<IntervalName>
{

    public readonly Degree Degree;
    public readonly Accidental Accidental;

    public readonly (int, int) RawValues;

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
    public IntervalName(Degree degree, Accidental accidental)
    {
        Degree = degree;
        Accidental = accidental;
        RawValues = (degree.Value, accidental.Value);
        Value = ScalePattern
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
    {
        expectedValue = Math.Abs(expectedValue);

        if (expectedValue > (Numbers.SemitonesInOctave * 2))
            expectedValue = (expectedValue % Numbers.SemitonesInOctave) + Numbers.SemitonesInOctave;
        int index = expectedDegree - Numbers.IndexOffset;
        int normalValue = ScalePattern.Diatonic.DoubleOctave[index];
        Accidental accidental = new(expectedValue - normalValue);
        return new(new Degree(expectedDegree), accidental);
    }

    public void Deconstruct(out int degree, out int accidental)
    {
        (degree, accidental) = RawValues;
    }

    // ISymbolSerializable ////////////////////////////////////////////////////
    public static IntervalName FromSymbol(string intervalSymbol)
    {
        Match data = RegularExpressions.IntervalSymbol.Match(intervalSymbol);
        if (!data.Success)
            throw new InvalidAsSymbol(intervalSymbol, nameof(intervalSymbol));

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
            // The actual string will only ever have one or the other, per the regex.
            int sharps = RegularExpressions.SharpSymbol.Count(groups[accidentalIdx].Value);
            int flats = RegularExpressions.FlatSymbol.Count(groups[accidentalIdx].Value);
            accidental = new(sharps - flats);
        }
        return new(new Degree(degree), accidental);
    }
    public string ToSymbol()
        => NomenclatureHelper.GetIntervalSymbol(this);

    static IntervalName ISymbolSerializable<IntervalName>.FromSymbol(string symbol)
        => FromSymbol(symbol);

    string ISymbolSerializable<IntervalName>.ToSymbol()
        => ToSymbol();

    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ToSymbol();
    }

    public static implicit operator (int, int)(IntervalName intervalName)
        => (intervalName.Degree.Value, intervalName.Accidental.Value);
    public static implicit operator IntervalName((int degree, int accidental) intervalName)
        => new(intervalName.degree, intervalName.accidental);

}
