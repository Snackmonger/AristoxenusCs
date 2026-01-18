namespace Aristoxenus.Domain.Models;

using Aristoxenus.Constants;
using Aristoxenus.Domain.Exceptions;
using Aristoxenus.SharedKernel.Bases;
using Aristoxenus.SharedKernel.Interfaces;
using Aristoxenus.Utilities;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class IntervalName : ValueObject, ISymbolSerializable<IntervalName>
{
    public readonly Degree Degree;
    public readonly Accidental Accidental;

    public int Value
    {
        get
        {
            int i = (Degree.Value - 1) % (Numbers.NotesInOctave * 2);
            int baseValue = RawScalePatterns.MakeDoubleOctave(RawScalePatterns.Diatonic)[i];
            return baseValue + Accidental.Value;
        }
    }

    public int SingleOctaveValue
        => Value < Numbers.SemitonesInOctave
            ? Value
            : Value % Numbers.SemitonesInOctave;

    public int DoubleOctaveValue 
        => SingleOctaveValue + Numbers.SemitonesInOctave;

    public string RomanName
        => Accidental.Symbol + Degree.Roman;

    public string Symbol
        => Accidental.Symbol + Degree.Symbol;

    public string LongCardinalName
        => $"{Accidental.Name} {Degree.Cardinal}";

    public string LongOrdinalName
        => $"{Accidental.Name} {Degree.Ordinal}";


    private IntervalName(Degree degree, Accidental accidental)
    {
        Degree = degree;
        Accidental = accidental;
    }

    public static IntervalName CreateNew(Degree degree, Accidental accidental)
        => new(degree, accidental);

    public static IntervalName CreateNew(int degree, int? accidental)
        => CreateNew(
            Degree.CreateNew(degree),
            Accidental.CreateNew(accidental ?? 0)
        );


    public static IntervalName FromExpectedValue(int expectedDegree, int expectedValue)
    {
        expectedValue = Math.Abs(expectedValue);
        // If the value is anything more than two octaves, reduce it to fit
        // within two octaves.
        if (expectedValue > Numbers.SemitonesInOctave)
        {
            expectedValue = (expectedValue % Numbers.SemitonesInOctave) + Numbers.SemitonesInOctave;
        }
        int normalValue = RawScalePatterns.MakeDoubleOctave(RawScalePatterns.Diatonic)[expectedDegree - 1];
        Accidental accidental = Accidental.CreateNew(expectedValue - normalValue);
        return CreateNew(Degree.CreateNew(expectedDegree), accidental);
    }

    public static IntervalName FromSymbol(string intervalSymbol)
    {
        Match data = RegularExpressions.IntervalSymbol.Match(intervalSymbol);
        if (!data.Success)
        {
            throw new InvalidAsSymbol(intervalSymbol, nameof(intervalSymbol));
        }
        GroupCollection groups = data.Groups;
        string name = groups[2].Value;
        int degree = name.All(char.IsDigit)
            ? int.Parse(name)
            : RomanNumerals.Decode(name.ToUpper());
        Accidental accidental = Accidental.CreateNew(0);
        if (groups[1] != null)
        {
            // The actual string will only ever
            // have one or the other, per the regex.
            int sharps = RegularExpressions.SharpSymbol.Count(groups[1].Value);
            int flats = RegularExpressions.FlatSymbol.Count(groups[1].Value);
            accidental = Accidental.CreateNew(sharps - flats);
        }
        return CreateNew(Degree.CreateNew(degree), accidental);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Symbol;
    }
}
