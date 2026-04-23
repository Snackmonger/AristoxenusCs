using Aristoxenus.Core.Domain.Theory.Interfaces;
using Aristoxenus.Core.SharedKernel.Bases;
using Aristoxenus.Core.SharedKernel.Interfaces;

namespace Aristoxenus.Core.Domain.Theory;

/// <summary>
/// Representation of a note, which is a pitch expressed over time and with
/// optional dynamics.
/// </summary>
public class Note : ValueObject, IHasDuration, ISymbolSerializable<Note>
{
    public Pitch Pitch { get; init; }
    public NonNegative<double> Duration { get; init; }
    public List<string> Dynamics { get; init; }
    public string Name => Pitch.Name.ToSymbol();

    // Constructors, Factories ////////////////////////////////////////////////
    private Note(Pitch pitch, NonNegative<double> duration, List<string> dynamics)
    {
        Pitch = pitch;
        Duration = duration;
        Dynamics = [..dynamics.Order()];
    }

    public Note CreateNew(Pitch pitch, NonNegative<double> duration, List<string> dynamics)
    {
        return new(pitch, duration, dynamics);
    }

    public Note CreateNew(Pitch pitch, NonNegative<double> duration)
    {
        return new(pitch, duration, []);
    }

    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ToSymbol();
    }


    // ISymbolSerializable ////////////////////////////////////////////////////

    // TODO: definitely the most non-standard symbol in the program.
    // Can we improve on this? E.g. C#5:0.875:pf,sfz,ff 

    public string ToSymbol()
        => $"{Name}:{Duration}:{string.Join(',', Dynamics)}";

    public static Note FromSymbol(string symbol)
    {
        var elements = symbol.Split(':');
        string name = elements[0];
        NonNegative<double> duration = double.Parse(elements[1]); 
        IEnumerable<string> dynamics = elements[2].Split(',');
        return new(Pitch.FromSymbol(name), duration, [..dynamics]);
    }
}


