using Aristoxenus.Core.Domains.Theory.Errors;

using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Utilities;

namespace Aristoxenus.Core.Domains.Theory.Atoms;

/// <summary>
/// An interval, which might be expressed by a name that elides its position 
/// in relative octave-space (e.g. we might call an interval a "major third"
/// schematically even though it is technically a "major tenth")
/// </summary>
public class Interval : ValueObject
{
    public readonly IntervalName Name;
    public readonly int Value;

    /// <summary>
    /// The raw value of the interval degree.
    /// </summary>
    public readonly int Degree;

    /// <summary>
    /// The raw value of the interval accidental.
    /// </summary>
    public readonly int Accidental;

    /// <summary>
    /// The raw value of the interval, if it were expressed in the first
    /// octave (e.g. a ninth becomes a second, a second remains a second etc.)
    /// </summary>
    public readonly int FirstOctaveValue;

    /// <summary>
    /// The raw value of the interval, if it were expressed in the second
    /// octave (e.g. a second becomes a ninth, a ninth remains a ninth etc.)
    /// </summary>
    public readonly int SecondOctaveValue;

    /// <summary>
    /// The quality of the interval, expressed as a formal name, e.g. 
    /// "perfect", "augmented", etc.
    /// </summary>
    public readonly string Quality;

    public Interval(IntervalName name, int value)
    {
        if (name.FirstOctaveValue != (value % Numbers.SemitonesInOctave))
        {
            throw new IntervalValueRepresentationMismatch(value, name.Value, name.ToSymbol());
        }
        Name = name;
        Value = value;
        Degree = Name.Degree.Value;
        Accidental = Name.Accidental.Value;
        FirstOctaveValue = Name.FirstOctaveValue;
        SecondOctaveValue = Name.SecondOctaveValue;
        Quality = NomenclatureHelper.GetIntervalQuality(Degree, Value);
    }


    /// <summary>
    /// Create an interval from raw primitives for the name, with its base 
    /// value.
    /// </summary>
    public static Interval FromIntervalName(int degree, int accidental)
    {
        var name = new IntervalName(degree, accidental);
        return new(name, name.Value);
    }

    /// <summary>
    /// Create an interval from an interval name object, with its base value.
    /// </summary>
    public Interval FromIntervalName(IntervalName name) => new(name, name.Value);
  

    /// <summary>
    /// Create an interval that will have the given degree, plus as many
    /// accidentals are necessary to reach the given value (limited to
    /// two octaves).
    /// </summary>
    public static Interval FromExpectedValue(int degree, int value)
    {
        var name = IntervalName.FromExpectedValue(degree, value);
        return new(name, name.Value);
    }

    /// <summary>
    /// Create an interval from the given interval name symbol, with 
    /// an inferred value.
    /// </summary>
    public static Interval FromSymbol(string intervalSymbol)
    {
        var name = IntervalName.FromSymbol(intervalSymbol);
        return new(name, name.Value);
    }


    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name.ToSymbol();
        yield return Value;
    }
}
