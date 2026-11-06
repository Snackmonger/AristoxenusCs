namespace Aristoxenus.Domain.Core.Models;

public record ChordScaleDerivation
{
    // [Nov 05 2026]
    // Right now, we're limiting chords to what can fit within two octaves.
    //
    // |        | Tertial              | Quartal              | Quintal              |
    // | ------ | -------------------- | -------------------- | -------------------- |
    // | Triad  | C4 E4 G4             | C4 F4 B4             | C4 G4 D5             |
    // | Tetrad | C4 E4 G4 B4          | C4 F4 B4 E5          | C4 G4 D5 A5          |
    // | Pentad | C4 E4 G4 B4 D5       | C4 F4 B4 E5 A5       | C4 G4 D5 A5 E6       |
    // | Hexad  | C4 E4 G4 B4 D5 F5    | C4 F4 B4 E5 A5 D6    | C4 G4 D5 A5 E6 B6    |
    // | Heptad | C4 E4 G4 B4 D5 F5 A5 | C4 F4 B4 E5 A5 D6 G6 | C4 G4 D5 A5 E6 B6 F7 |
    // 
    // - Tertials can fit chords of up to a heptad into two octaves.
    // - Quartals can fit chords of up to a pentad into two octaves.
    // - Quintals can fit chords of up to a tetrad into two octaves.
    //
    // Theoretically, this limit doesn't really need to exist, but allowing
    // for extremely wide chords is probably going to lead to some downstream
    // complexities (trying to make sure they don't exceed the actual limit
    // of the context where they're being used).

    public ChordStructure Structure { get; init; }
    public ChordNumberOfNotes NumberOfNotes { get; init; }
    public ChordScaleDerivation(ChordStructure structure, ChordNumberOfNotes numberOfNotes)
    {
        //if (structure == ChordStructure.Quartal && numberOfNotes > ChordNumberOfNotes.Pentad)
        //    throw new ArgumentException($"Quartal chords cannot have more than {ChordNumberOfNotes.Pentad} notes.");

        //if (structure == ChordStructure.Quintal && numberOfNotes > ChordNumberOfNotes.Tetrad)
        //    throw new ArgumentException($"Quintal chords cannot have more than {ChordNumberOfNotes.Tetrad} notes.");
        
        Structure = structure;
        NumberOfNotes = numberOfNotes;

    }

}
