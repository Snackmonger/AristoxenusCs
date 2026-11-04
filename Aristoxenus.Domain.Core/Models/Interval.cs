using Aristoxenus.Domain.Core.Bases;
using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Errors;
using Aristoxenus.Domain.Core.Interfaces;
using Aristoxenus.Domain.Core.Utilities;

namespace Aristoxenus.Domain.Core.Models;


public class Interval(int degree, int accidental) : ValueObject, ISymbolSerializable<Interval>
{
    public Interval((int Degree, int Accidental) interval) : this(interval.Degree, interval.Accidental) { }

    public int Value => NomenclatureHelper.GetIntervalValueFromDegreeName((Degree, Accidental));

    /// <summary>
    /// The raw value of the interval's name's degree.
    /// </summary>
    public int Degree => NormalizationHelper.NormalizeDegree(degree);

    /// <summary>
    /// The raw value of the interval's name's accidental.
    /// </summary>
    public int Accidental => NormalizationHelper.NormalizeAccidental(accidental);


    /// <summary>
    /// The quality of the interval, expressed as a formal name, e.g. 
    /// "perfect", "augmented", etc.
    /// </summary>
    public string Quality
        => NomenclatureHelper.GetIntervalQuality(Degree, Value);

    /// <summary>
    /// The reciprocal of this interval.
    /// </summary>
    public Interval Reciprocal
        => NomenclatureHelper.GetReciprocalInterval(this);

    /// <summary>
    /// The unison, which is the default interval that all interval structures 
    /// must have.
    /// </summary>
    public static Interval Unison => new(1, 0);


    /// <summary>
    /// Create an interval that will have the given degree, plus as many
    /// accidentals are necessary to reach the given value (limited to
    /// two octaves + 1 semitone).
    /// </summary>
    public static Interval FromExpectedValue(int degree, int expectedValue)
        => new(NomenclatureHelper.GetIntervalDegreeNameFromExpectedValue(degree, expectedValue));

    /// <summary>
    /// Widen this interval by the given number of octaves. For example, 
    /// widening a 2 yields a 9. 
    /// </summary>
    public Interval Widen(int octaves)
    {
        octaves = Math.Abs(octaves);
        if (octaves > Numbers.MaxOctaveNumber)
        {
            throw new MaximumOctavesExceeded();
        }

        int degree = Degree;
        int value = Value;

        degree += (octaves * Numbers.NotesInOctave);

        value += (octaves * Numbers.SemitonesInOctave);

        return FromExpectedValue(degree, value);
    }

    /// <summary>
    /// Narrow an interval by the given number of intervals. For example, 
    /// narrowing a 9 yeilds a 2. 
    /// </summary>
    public Interval Narrow(int octaves)
    {
        octaves = Math.Abs(octaves);
        if (Value / Numbers.SemitonesInOctave < octaves)
        {
            octaves = Value / Numbers.SemitonesInOctave;
        }

        int degree = Degree;
        int value = Value;

        if (degree is > Numbers.NotesInOctave)
        {
            degree -= (octaves * Numbers.NotesInOctave);
            value -= (octaves * Numbers.SemitonesInOctave);
        }


        return FromExpectedValue(degree, value);

    }

    public static Interval FromSymbol(string scaleDegreeSymbol)
    {
        (int degree, int accidental) = SymbolHelper.ParseScaleDegreeSymbol(scaleDegreeSymbol);
        return new(degree, accidental);
    }

    public string ToSymbol()
        => SymbolHelper.CreateScaleDegreeSymbol((Degree, Accidental));

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Degree;
        yield return Accidental;
    }


    public static implicit operator Interval((int Degree, int Accidental) interval)
        => new(interval);

    public static implicit operator (int, int)(Interval interval)
        => (interval.Degree, interval.Accidental);

    public static implicit operator string(Interval interval)
        => interval.ToSymbol();

    public static implicit operator Interval(string intervalName)
        => FromSymbol(intervalName);
}
