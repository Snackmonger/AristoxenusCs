using Aristoxenus.Core.Domain.Theory.Interfaces;
using Aristoxenus.Core.SharedKernel.Bases;

namespace Aristoxenus.Core.Domain.Theory;

/// <summary>
/// A representation of a linear stream of duration-having elements, which
/// are notes and rests.
/// </summary>
public sealed class MelodicLine : ValueObject
{
    /// <summary>
    /// The time signature in which this melodic line is set.
    /// </summary>
    public TimeSignature TimeSignature { get; private set; }

    /// <summary>
    /// The duration-having elements that make up this melodic line.
    /// </summary>
    public List<IHasDuration> Elements { get; private set; } = [];

    /// <summary>
    /// The current time in the melodic line (i.e. the sum of all current 
    /// melodic elements' durations).
    /// </summary>
    public NonNegative<double> CurrentTime 
        => Elements.Select(n => n.Duration).Aggregate((NonNegative<double> acc, NonNegative<double> next) => acc + next);

    // Constructors, Factories ////////////////////////////////////////////////
    private MelodicLine(TimeSignature timeSignature, IEnumerable<IHasDuration> notes)
    {
        Elements = [.. notes];
        TimeSignature = timeSignature;
    }

    public static MelodicLine CreateNew(TimeSignature timeSignature) 
        => new(timeSignature, []);

    public static MelodicLine Reconstitute(TimeSignature timeSignature, IEnumerable<IHasDuration> notes)
        => new(timeSignature, notes);

    // Public Api /////////////////////////////////////////////////////////////

    /// <summary>
    /// Add a new element to the end of the melody.
    /// </summary>
    public MelodicLine Add(IHasDuration element) 
        => new(TimeSignature, [.. Elements, element]);

    /// <summary>
    /// Return true if there is a sounding note at the given time index, and 
    /// write the note to the out parameter if so.
    /// </summary>
    public bool NoteAtTime(NonNegative<double> seekTime, out Note? note)
    {
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
    public bool RestAtTime(NonNegative<double> seekTime, out Rest? rest)
    {
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
    public IHasDuration ElementAtTime(NonNegative<double> seekTime)
    {
        NonNegative<double> currentTime = 0;
        foreach (IHasDuration el in Elements)
        {
            currentTime += el.Duration.Value;
            if (seekTime < currentTime)
                return el;
            
        }
        throw new IndexOutOfRangeException($"No melodic elements found at the time index {seekTime}");
    }

    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TimeSignature;
        foreach (var element in Elements)
        {
            if (element is Rest r)
            {
                yield return r.Duration.Value;
            }
            else if (element is Note n)
            {
                yield return n.Name;
                yield return n.Duration.Value;
                foreach (string d in n.Dynamics)
                    yield return d;
            }
        }
    }
}
