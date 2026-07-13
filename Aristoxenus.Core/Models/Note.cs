using Aristoxenus.Core.Interfaces;

namespace Aristoxenus.Core.Models;

/// <summary>
/// Representation of a note, which is a pitch that extends over a length of
/// time and may have dynamics.
/// </summary>
public record Note : IHasDuration
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
        Duration= duration;
        Dynamics = [];
    }


}


