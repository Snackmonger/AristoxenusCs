using Aristoxenus.Core.Domains.Theory.Interfaces;
using Aristoxenus.Core.Shared.Bases;

namespace Aristoxenus.Core.Domains.Theory.Atoms;

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

    public Note(string pitch, Duration duration, List<string> dynamics)
    {
        Pitch = Pitch.FromSymbol(pitch);
        Duration = duration;
        Dynamics = [.. dynamics.Order()];
    }

    public Note(Pitch pitch, Duration duration)
    {
        Pitch = pitch;
        Duration = duration;
        Dynamics = [];
    }

    public Note(string pitch, Duration duration)
    {
        Pitch = Pitch.FromSymbol(pitch);
        Duration = duration;
        Dynamics = [];
    }

    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }


    // ISymbolSerializable ////////////////////////////////////////////////////


}


