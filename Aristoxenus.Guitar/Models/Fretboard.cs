using Aristoxenus.Core.Bases;
using Aristoxenus.Core.Data;
using Aristoxenus.Core.Models;
using Aristoxenus.Core.Utilities;

namespace Aristoxenus.Guitar.Models;


public class Fretboard(IDictionary<int, int> strings, int numberOfFrets) : ValueObject
{
    public Dictionary<int, int> OpenStrings { get; init; } = strings.ToDictionary();
    public int NumberOfFrets { get; init; } = numberOfFrets;

    public HashSet<Fret> FindPitch(int pitch) => FindPitches([pitch]);

    /// <summary>
    /// Find all the frets where the given pitches exist.
    /// </summary>
    /// <param name="midiNumbers">The midi numbers of the pitches to search 
    /// for.</param>
    /// <returns>A set of frets with the given pitches.</returns>
    public HashSet<Fret> FindPitches(IEnumerable<int> midiNumbers)
    {
        HashSet<Fret> result = [];
        foreach (int pitch in midiNumbers)

            foreach ((int stringNumber, int openStringPitch) in OpenStrings)

                foreach ((int fretNumber, int midiNumber) in GetString(stringNumber).Select((i, midiNumber) => (i, midiNumber)))

                    if (pitch == midiNumber)
                        result.Add(new Fret(stringNumber, fretNumber, pitch));

        return result;
    }

    /// <summary>
    /// Get the midi numbers for the entire string, starting from the open 
    /// string and running for as many frets as the board has.
    /// </summary>
    /// <param name="openString">The number of the string to query.</param>
    /// <returns>A list of midi numbers starting at the open string and 
    /// continuing for as many frets as the fretboard has.
    /// </returns>
    private List<int> GetString(int openString)
    {
        List<int> result = [];
        for (int i = 0; i < NumberOfFrets; i++)
            result.Add(OpenStrings[openString] + i);
        return result;
    }

    /// <summary>
    /// Get the frets where a pitch with the given note names can be found.
    /// </summary>
    /// <param name="noteNames">The names of the notes to search for.</param>
    /// <returns>A set of frets where the given notes can be found</returns>
    /// <exception cref="NotImplementedException"></exception>
    public HashSet<Fret> FindNotes(IEnumerable<NoteName> noteNames)
    {
        int minOctave = OpenStrings.Values.Min() / Numbers.SemitonesInOctave;
        int maxOctave = (OpenStrings.Values.Max() / Numbers.SemitonesInOctave) % Numbers.MaxOctaveNumber;

        HashSet<int> pitches = [];
        for (int i = minOctave; i < maxOctave; i++)
            foreach (NoteName noteName in noteNames)
                pitches.Add(FrequencyHelper.GetMidiNumberAtPitch(noteName, i));
        return FindPitches(pitches);
    }

    /// <summary>
    /// Get the frets where a pitch with the given note names can be found.
    /// </summary>
    /// <param name="noteNames">The names of the notes to search for.</param>
    /// <returns>A set of frets where the requested notes can be found</returns>
    /// <exception cref="NotImplementedException"></exception>
    public HashSet<Fret> FindNotes(IEnumerable<string> noteNames)
        => FindNotes(noteNames.Select(NoteName.FromSymbol));

    /// <summary>
    /// Get the frets where pitches with note names in the given chord name
    /// can be found.
    /// </summary>
    /// <param name="chordName">The name of the chord of which to find the notes</param>
    /// <returns>A set of frets where the requested notes can be found</returns>
    public HashSet<Fret> FindNotes(ChordName chordName)
        => FindNotes(chordName.GetNoteNames());

    /// <summary>
    /// Get the frets where pitches with note names in the given scale form
    /// can be found.
    /// </summary>
    /// <param name="scale">The scale form of which to find the notes</param>
    /// <returns>A set of frets where the requested notes can be found</returns>
    public HashSet<Fret> FindNotes(Scale scale)
        => FindNotes(scale.Notes);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return OpenStrings;
        yield return NumberOfFrets;
    }
}
