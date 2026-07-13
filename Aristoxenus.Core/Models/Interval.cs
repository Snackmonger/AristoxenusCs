using Aristoxenus.Core.Bases;
using Aristoxenus.Core.Data;
using Aristoxenus.Core.Errors;
using Aristoxenus.Core.Utilities;
using System.Security.Cryptography.X509Certificates;

namespace Aristoxenus.Core.Models;

/// <summary>
/// An interval, which might be expressed by a name that elides its position 
/// in relative octave-space (e.g. we might call an interval a "major third"
/// schematically even though it is technically a "major tenth")
/// </summary>
public class Interval : ValueObject
{
    /// <summary>
    /// The name of the interval, which is derived from the scale degree 
    /// that corresponds to the interval when ascending.
    /// </summary>
    public readonly ScaleDegree Name;

    /// <summary>
    /// The value of the interval, which must correspond with its name.
    /// For example, an interval characterized as a b9 implies a value
    /// of 13, but a value of 25 is also called a b9 for simplicity,
    /// and it is the base value + (n * 12).
    /// </summary>
    public readonly int Value;

    /// <summary>
    /// The raw value of the interval's name's degree.
    /// </summary>
    public int Degree => Name.Degree;

    /// <summary>
    /// The raw value of the interval's name's accidental.
    /// </summary>
    public int Accidental => Name.Accidental;

    /// <summary>
    /// The raw value of the interval, if it were expressed in the first
    /// octave (e.g. a ninth becomes a second, a second remains a second etc.)
    /// </summary>
    public int FirstOctaveValue
        => Value % Numbers.SemitonesInOctave;

    /// <summary>
    /// The raw value of the interval, if it were expressed in the second
    /// octave (e.g. a second becomes a ninth, a ninth remains a ninth etc.)
    /// </summary>
    public int SecondOctaveValue
        => FirstOctaveValue + Numbers.SemitonesInOctave;

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
    public static Interval Unison => new((1, 0));

    public Interval(ScaleDegree name, int value)
    {
        int impliedValue = NomenclatureHelper.GetIntervalValueFromName(name);
        if (impliedValue % Numbers.SemitonesInOctave != value % Numbers.SemitonesInOctave)
        {
            throw new IntervalValueRepresentationMismatch(value, impliedValue, name.ToSymbol());
        }
        Name = name;
        Value = value;
    }

    public Interval(ScaleDegree name)
    {
        Value = NomenclatureHelper.GetIntervalValueFromName(name);
        Name = name;
    }


    /// <summary>
    /// Create an interval from the given interval name symbol, with 
    /// an inferred value.
    /// </summary>
    public static Interval FromScaleDegreeSymbol(string scaleDegreeSymbol)
        => new(ScaleDegree.FromSymbol(scaleDegreeSymbol));


    /// <summary>
    /// Create an interval that will have the given degree, plus as many
    /// accidentals are necessary to reach the given value (limited to
    /// two octaves + 1 semitone).
    /// </summary>
    public static Interval FromExpectedValue(int degree, int expectedValue)
        => new(NomenclatureHelper.GetIntervalNameFromExpectedValue(degree, expectedValue));

    /// <summary>
    /// Widen this interval by the given number of octaves. For example, 
    /// widening a 2 yields a 9. 
    /// 
    /// NOTE: For intervals of more than two octaves, their names will 
    /// be restricted to two octaves, but their values will encode their 
    /// actual width.
    /// </summary>
    public Interval Widen(int octaves)
    {
        octaves = Math.Abs(octaves);
        if (octaves > Numbers.MaxOctaveNumber)
            throw new MaxOctavesExceeded();

        int degree = Degree;
        int value = Value;

        if (Degree <= Numbers.NotesInOctave)
            degree += Numbers.NotesInOctave;

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
            octaves = Value / Numbers.SemitonesInOctave;

        int degree = Degree;
        int value = Value;

        if (degree is > Numbers.NotesInOctave and <= Numbers.NotesInDoubleOctave)
            degree -= Numbers.NotesInOctave;

        value -= (octaves * Numbers.SemitonesInOctave);

        return FromExpectedValue(degree, value);

    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Degree;
        yield return Accidental;
        yield return Value;
    }

    public static implicit operator Interval(ScaleDegree scaleDegree)
        => FromScaleDegreeSymbol(scaleDegree);

    public static implicit operator ScaleDegree(Interval interval)
        => interval.Name;

    public static implicit operator Interval((int Degree, int Accidental) interval)
        => new(interval);

    public static implicit operator (int, int)(Interval interval)
        => (interval.Degree, interval.Accidental);

    public static implicit operator string(Interval interval)
        => interval.Name.ToSymbol();

    public static implicit operator Interval(string intervalName)
        => FromScaleDegreeSymbol(intervalName);
}
