namespace Aristoxenus.Domain.Core.Models;

public record ChordScaleDerivation
{

    public ChordStructure Structure { get; init; }
    public ChordNumberOfNotes NumberOfNotes { get; init; }
    public ChordScaleDerivation(ChordStructure structure, ChordNumberOfNotes numberOfNotes)
    {

        Structure = structure;
        NumberOfNotes = numberOfNotes;

    }

}
