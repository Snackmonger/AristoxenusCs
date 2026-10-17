using Aristoxenus.Core.Bases;
using Aristoxenus.Core.Interfaces;
using Fractions;

namespace Aristoxenus.Core.Models;

/// <summary>
/// Representation of a note, which is a pitch that extends over a length of
/// time and may have dynamics.
/// </summary>
public class Note : ValueObject, IHasDuration
{
    public Pitch Pitch { get; init; }
    public Duration Duration { get; init; }
    public List<string> Dynamics { get; init; }

    // Constructors, Factories ////////////////////////////////////////////////
    public Note(Pitch pitch, Duration duration, List<string> dynamics)
    {
        Pitch = pitch;
        Duration = duration;
        Dynamics = [.. dynamics.Order()];
    }

    public Note(Pitch pitch, Duration duration)
    {
        Pitch = pitch;
        Duration = duration;
        Dynamics = [];
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Pitch;
        yield return Duration;
        foreach (var component in Dynamics.Order())
        {
            yield return component;
        }
    }

    public IEnumerable<IHasDuration> SubdivideEqully(int subdivisions)
    {
        Fraction subduration = Duration.Value / subdivisions;
        for (int i = 0; i < subdivisions; i++)
        {
            yield return new Note(Pitch, new Duration(subduration), Dynamics);
        }
    }

    public IHasDuration Extend(int multiplier)
    {
        throw new NotImplementedException();
    }
}


