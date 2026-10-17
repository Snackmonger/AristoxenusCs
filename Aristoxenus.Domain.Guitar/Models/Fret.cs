using Aristoxenus.Domain.Core.Bases;
using Aristoxenus.Domain.Core.Models;

namespace Aristoxenus.Domain.Guitar.Models;

public class Fret : ValueObject
{
    public int String { get; init; }
    public int FretNumber { get; init; }
    public int MidiNumber { get; init; }


    public Fret(int stringNumber, int fretNumber, int midiNumber)
    {
        String = stringNumber;
        FretNumber = fretNumber;
        MidiNumber = midiNumber;
    }

    public Pitch Pitch => Pitch.GetPitchAbove(FretNumber);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return String;
        yield return FretNumber;
    }


}
