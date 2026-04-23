using Aristoxenus.Core.Constants;
using Aristoxenus.Core.SharedKernel.Bases;

namespace Aristoxenus.Core.Domain.Theory;

public class Duration(double value) : IComparable<Duration>
{
    public NonNegative<double> Value { get; init; } = value;

    // Static Api /////////////////////////////////////////////////////////////

    /// <summary>
    /// Return the value of the two durations combined.
    /// </summary>
    /// <example>
    /// Add(DurationValues.Eighth, quarter) == 2.6666666666666665;
    /// </example>
    public static Duration Add(Duration d1, Duration d2)
        => 1 / (1.0 / d1 + 1.0 / d2);

    /// <summary>
    /// Return the note value for value1 minus value2
    /// (WARNING: values below zero will raise an exception)
    /// </summary>
    /// <example>
    /// Subtract(DurationValues.Quarter, DurationValues.Eighth) == 8.0;
    /// </example>
    public static Duration Subtract(Duration d1, Duration d2)
        => 1 / (1.0 / d1 - 1.0 / d2);

    /// <summary>
    /// Return the dotted duration value.
    /// 
    /// A dot adds half the duration of the note. A second dot adds half of what 
    /// was added before, etc. So a dotted eighth note has the length of three 
    /// sixteenth notes.An eighth note with two dots has the length of seven 
    /// thirty second notes.
    /// </summary>
    /// <example>
    /// Dots(DurationValues.Eighth) == 5.3333333333333333;
    /// Dots(DurationValues.Eighth, 2d) == 4.5714285714285712;
    /// Dots(DurationValues.Quarter) == 2.6666666666666665;
    /// </example>
    public static Duration Dots(Duration value, int numberOfDots = 1)
        => (0.5 * value) / Math.Pow(1.0 - 0.5, (numberOfDots + 1));

    /// <summary>
    /// Return the value of each element in a tuplet derived from the
    /// original given <paramref name="value"/> and ratio 
    /// (<paramref name="rat1"/>/<paramref name="rat2"/>). 
    /// 
    /// A tuplet can be written as a ratio, e.g: 5:4 means that you play 
    /// 5 notes in the duration of 4 (a quintuplet), 3:2 means that you play 3 
    /// notes in the duration of 2 (a triplet), etc.
    /// </summary>
    /// <example>
    /// Tuplet(8d, 3, 2) == 12.0;
    /// </example>
    public static Duration Tuplet(Duration value, int rat1, int rat2)
        => (rat1 * value) / (double)rat2;

    /// <summary>
    /// Return the triplet note value.
    ///
    /// A triplet divides the base value above into three parts, so a 
    /// triplet eighth note is a third of a quarter note.
    /// </summary>
    /// <example>
    /// Triplet(DurationValues.Eighth) == 12;
    /// Triplet(4) == 6;
    /// </example>
    public static Duration Triplet(Duration value)
        => Tuplet(value, 3, 2);

    /// <summary>
    /// Return the quintuplet note value.
    /// 
    /// A quintuplet divides the base value two above into five parts.So a
    /// quintuplet eighth note is a fifth of a half note.
    /// </summary>
    /// <example>
    /// Quintuplet(8d) == 10;
    /// Quintuplet(4d) == 5;
    /// </example>
    public static Duration Quintuplet(Duration value)
        => Tuplet(value, 5, 4);

    /// <summary>
    /// Return the septuplet note value.
    /// 
    /// The usage of a septuplet is ambigious: seven notes can be played 
    /// in the duration of four or eighth notes.
    /// 
    /// If <paramref name="inFourths"/> is true, this function will 
    /// use 4, otherwise 8 notes.So a septuplet eighth note is 
    /// respectively either 14 or 7.
    /// 
    /// Notice how `Septuplet(8d, false) == Septuplet(4d, true)`
    /// 
    /// Examples:
    /// Septuplet(8d) == 14;
    /// Septuplet(8d, false) == 7;
    /// </summary>
    public static Duration Septuplet(Duration value, bool inFourths = true)
        => inFourths
            ? Tuplet(value, 7, 4)
            : Tuplet(value, 7, 8);

    /// <summary>
    /// Analyse the value and return a tuple containing the parts it's made of. 
    /// </summary>
    /// <example>
    /// Determine(8) ==  (8, 0, 1, 1)
    /// Determine(12) == (8, 0, 3, 2)
    /// Determine(14) == (8, 0, 7, 4)
    /// </example>
    public static (Duration baseValue, int numberOfDots, int rat1, int rat2) Determine(Duration value)
    {
        int i = -2;
        double? val = null;
        foreach (double v in Numbers.BaseDurations)
        {
            val = v;
            if (value == v) return (value, 0, 1, 1);
            if (value < v) break;
            i += 1;
        }
        double scaled = value / Math.Pow(2, i);
        if (scaled >= 0.9375) return (Numbers.BaseDurations[i], 0, 1, 1);
        if (scaled >= 0.8125) return (Numbers.BaseDurations[i + 1], 0, 7, 4);
        if (scaled >= 17 / 24.0) return (Numbers.BaseDurations[i + 1], 0, 3, 2);
        if (scaled >= 31 / 48.0) return ((double)val!, 1, 1, 1);
        if (scaled >= 67 / 112.0) return (Numbers.BaseDurations[i + 1], 0, 5, 4);
        double d = 3;
        for (int x = 2; x < 6; x++)
        {
            d += Math.Pow(2, x);
            if (scaled == Math.Pow(2.0, x) / d)
                return ((double)val!, x, 1, 1);
        }
        return (Numbers.BaseDurations[i + 1], 0, 1, 1);
    }

    // Non-static Api /////////////////////////////////////////////////////////
    public Duration Add(Duration d) => Add(this, d);
    public Duration Subtract(Duration d) => Subtract(this, d);
    public Duration Dots(int numberOfDots = 1) => Dots(this, numberOfDots);
    public (Duration baseValue, int numberOfDots, int rat1, int rat2) Determine() => Determine(this);
    public Duration Tuplet(int rat1, int rat2) => Tuplet(this, rat1, rat2);
    public Duration Triplet() => Triplet(this);
    public Duration Quintuplet() => Quintuplet(this);
    public Duration Septuplet() => Septuplet(this);

    // Operator Overloads /////////////////////////////////////////////////////
    public static Duration operator +(Duration value1, Duration value2)
    => value1.Add(value2);
    public static Duration operator -(Duration value1, Duration value2)
        => value1.Subtract(value2);
    public static bool operator >(Duration value1, Duration value2)
        => value1.Value > value2.Value;
    public static bool operator <(Duration value1, Duration value2)
        => value1.Value < value2.Value;
    public static bool operator >=(Duration value1, Duration value2)
        => value1.Value >= value2.Value;
    public static bool operator <=(Duration value1, Duration value2)
        => value1.Value <= value2.Value;

    // Casts //////////////////////////////////////////////////////////////////

    public static implicit operator double(Duration d) => d.Value;
    public static implicit operator Duration(double d) => new(d);

    public static implicit operator NonNegative<double>(Duration d) => d.Value;

    // IComparable ////////////////////////////////////////////////////////////
    public int CompareTo(Duration other)
    => Value.CompareTo(other.Value);

    public override string ToString()
        => Value.ToString() ?? "0";
}
/// <summary>
/// Durations are expressed with a backwards logic, so that the higher 
/// the value, the smaller the division. This class helps to sugar away
/// some unsightly calculations (logic and documentation borrowed 
/// from `python-mingus` with minor edits).
/// </summary>
public class DurationHelper
{
    /// <summary>
    /// Return the value of the two durations combined.
    /// </summary>
    /// <example>
    /// Add(DurationValues.Eighth, quarter) == 2.6666666666666665;
    /// </example>
    public static double Add(double d1, double d2)
        => 1 / (1.0 / d1 + 1.0 / d2);

    /// <summary>
    /// Return the note value for value1 minus value2.
    /// There are no exceptions for producing negative values, which can be
    /// useful for taking differences.
    /// </summary>
    /// <example>
    /// Subtract(DurationValues.Quarter, DurationValues.Eighth) == 8.0;
    /// </example>
    public static double Subtract(double d1, double d2)
        => 1 / (1.0 / d1 - 1.0 / d2);

    /// <summary>
    /// Return the dotted note value.
    /// 
    /// A dot adds half the duration of the note. A second dot adds half of what 
    /// was added before, etc. So a dotted eighth note has the length of three 
    /// sixteenth notes.An eighth note with two dots has the length of seven 
    /// thirty second notes.
    /// </summary>
    /// <example>
    /// Dots(DurationValues.Eighth) == 5.3333333333333333;
    /// Dots(DurationValues.Eighth, 2d) == 4.5714285714285712;
    /// Dots(DurationValues.Quarter) == 2.6666666666666665;
    /// </example>
    public static double Dots(double value, int numberOfDots = 1)
        => (0.5 * value) / Math.Pow(1.0 - 0.5, (numberOfDots + 1));

    /// <summary>
    /// Return the value of each element in a tuplet derived from the
    /// original given <paramref name="value"/> and ratio 
    /// (<paramref name="rat1"/>/<paramref name="rat2"/>). 
    /// 
    /// A tuplet can be written as a ratio, e.g: 5:4 means that you play 
    /// 5 notes in the duration of 4 (a quintuplet), 3:2 means that you play 3 
    /// notes in the duration of 2 (a triplet), etc.
    /// </summary>
    /// <example>
    /// Tuplet(8d, 3, 2) == 12.0;
    /// </example>
    public static double Tuplet(double value, int rat1, int rat2)
        => (rat1 * value) / (double)rat2;

    /// <summary>
    /// Return the triplet note value.
    ///
    /// A triplet divides the base value above into three parts, so a 
    /// triplet eighth note is a third of a quarter note.
    /// </summary>
    /// <example>
    /// Triplet(DurationValues.Eighth) == 12;
    /// Triplet(4) == 6;
    /// </example>
    public static double Triplet(double value)
        => Tuplet(value, 3, 2);

    /// <summary>
    /// Return the quintuplet note value.
    /// 
    /// A quintuplet divides the base value two above into five parts.So a
    /// quintuplet eighth note is a fifth of a half note.
    /// </summary>
    /// <example>
    /// Quintuplet(8d) == 10;
    /// Quintuplet(4d) == 5;
    /// </example>
    public static double Quintuplet(double value)
        => Tuplet(value, 5, 4);

    /// <summary>
    /// Return the septuplet note value.
    /// 
    /// The usage of a septuplet is ambigious: seven notes can be played 
    /// in the duration of four or eighth notes.
    /// 
    /// If <paramref name="inFourths"/> is true, this function will 
    /// use 4, otherwise 8 notes.So a septuplet eighth note is 
    /// respectively either 14 or 7.
    /// 
    /// Notice how `Septuplet(8d, false) == Septuplet(4d, true)`
    /// 
    /// Examples:
    /// Septuplet(8d) == 14;
    /// Septuplet(8d, false) == 7;
    /// </summary>
    public static double Septuplet(double value, bool inFourths = true)
        => inFourths
            ? Tuplet(value, 7, 4)
            : Tuplet(value, 7, 8);

    /// <summary>
    /// Analyse the value and return a tuple containing the parts it's made of. 
    /// </summary>
    /// <example>
    /// Examples:
    /// Determine(8)
    /// (8, 0, 1, 1)
    /// Determine(12)
    /// (8, 0, 3, 2)
    /// Determine(14)
    /// (8, 0, 7, 4)
    /// </example>
    public static (double baseValue, int numberOfDots, int rat1, int rat2) Determine(double value)
    {
        int i = -2;
        double? val = null;
        foreach (double v in Numbers.BaseDurations)
        {
            val = v;
            if (value == v) return (value, 0, 1, 1);
            if (value < v) break;
            i += 1;
        }
        double scaled = value / Math.Pow(2, i);
        if (scaled >= 0.9375) return (Numbers.BaseDurations[i], 0, 1, 1);
        if (scaled >= 0.8125) return (Numbers.BaseDurations[i + 1], 0, 7, 4);
        if (scaled >= 17 / 24.0) return (Numbers.BaseDurations[i + 1], 0, 3, 2);
        if (scaled >= 31 / 48.0) return ((double)val!, 1, 1, 1);
        if (scaled >= 67 / 112.0) return (Numbers.BaseDurations[i + 1], 0, 5, 4);
        double d = 3;
        for (int x = 2; x < 6; x++)
        {
            d += Math.Pow(2, x);
            if (scaled == Math.Pow(2.0, x) / d)
                return ((double)val!, x, 1, 1);
        }
        return (Numbers.BaseDurations[i + 1], 0, 1, 1);
    }
}
