using Fractions;
using Aristoxenus.Core.Interfaces;

namespace Aristoxenus.Core.Models;

/// <summary>
/// A representation of a linear stream of duration-having elements, which
/// are notes and rests.
/// </summary>
public sealed class MelodicLine<T>
{
    public T Id { get; init; }

    /// <summary>
    /// The time signature in which this melodic line is set.
    /// </summary>
    public TimeSignature TimeSignature { get; private set; }

    /// <summary>
    /// The duration-having elements that make up this melodic line.
    /// </summary>
    public List<IHasDuration> Elements { get; private set; } = [];

    /// <summary>
    /// The absolute current time in the melodic line.
    /// </summary>
    public Duration CurrentTime
        => Elements
            .Select(n => n.Duration)
            .Aggregate((acc, next) => acc + next);

    /// <summary>
    /// The current measure, calculated from the durations of all melodic 
    /// <see cref="Elements"/>, relative to the line's 
    /// <see cref="TimeSignature"/>
    /// </summary>
    public int CurrentMeasure
    {
        get
        {
            Duration fullMeasure = new(TimeSignature.BeatType.Value.Multiply(TimeSignature.Beats));
            int measures = 0;
            Duration currentCount = new(0);
            foreach (IHasDuration element in Elements)
            {
                currentCount += element.Duration;
                if (currentCount >= fullMeasure)
                {
                    measures += 1;
                    currentCount -= fullMeasure;
                }
            }
            return measures;
        }
    }


    // Constructors, Factories ////////////////////////////////////////////////
    public MelodicLine(T Id, TimeSignature timeSignature, IEnumerable<IHasDuration> notes)
    {
        Elements = [.. notes];
        TimeSignature = timeSignature;
    }

    public MelodicLine(T id, TimeSignature timeSignature)
    {
        Elements = [];
        TimeSignature = timeSignature;
    }

    // Public Api /////////////////////////////////////////////////////////////

    /// <summary>
    /// Add a new element to the end of the melody.
    /// </summary>
    public MelodicLine<T> Add(IHasDuration element)
    {
        Elements = [.. Elements, element];
        return this;
    }

    /// <summary>
    /// Return true if there is a sounding note at the given time index, and 
    /// write the note to the out parameter if so.
    /// </summary>
    public bool NoteAtTime(double seekTime, out Note? note)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(seekTime);
        note = null;
        if (ElementAtTime(seekTime) is Note n)
        {
            note = n;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Return true if there is a rest at the given point in the melodic 
    /// timeline, and write the rest to the out parameter if so.
    /// </summary>
    public bool RestAtTime(double seekTime, out Rest? rest)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(seekTime);
        rest = null;
        if (ElementAtTime(seekTime) is Rest r)
        {
            rest = r;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Fetch whatever element intersects with the given point on the 
    /// melodic timeline.
    /// <paramref name="seekTime"/>.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">If the seek time has no 
    /// elements (usually because it exceeds the melody's current time)</exception>
    public IHasDuration ElementAtTime(double seekTime)
    {
        Fraction currentTime = 0;
        foreach (IHasDuration el in Elements)
        {
            currentTime += el.Duration.Value;
            if (Fraction.FromDouble(seekTime) < currentTime)
                return el;

        }
        throw new IndexOutOfRangeException($"No melodic elements found at the time index {seekTime}");
    }
}
