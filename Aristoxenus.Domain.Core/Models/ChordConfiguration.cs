namespace Aristoxenus.Domain.Core.Models;

public record ChordConfiguration
{
    public ChordStructure Structure { get; init; }
    public ChordNumberOfNotes NumberOfNotes { get; init; }
    public ChordVoicing? Voicing { get; init; }
    public ChordConfiguration(ChordStructure structure, ChordNumberOfNotes numberOfNotes)
    {
        Structure = structure;
        NumberOfNotes = numberOfNotes;
        Voicing = null;
    }

    public ChordConfiguration(ChordStructure structure, ChordNumberOfNotes numberOfNotes, ChordVoicing? voicing)
    {
        Structure = structure;
        NumberOfNotes = numberOfNotes;
        Voicing = voicing;
    }
}
