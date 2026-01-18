namespace Aristoxenus.Domain.Models;

using Aristoxenus.Constants;
using Aristoxenus.Domain.Exceptions;
using Aristoxenus.SharedKernel.Bases;
using Aristoxenus.SharedKernel.Interfaces;
using Aristoxenus.Utilities;

/// <summary>
/// Representation of a NoteName in the concrete tonal space of a range of
/// octaves.
/// </summary>
public class Pitch : ValueObject, ISymbolSerializable<Pitch>
{
    public NoteName Name;
    public OctaveNumber Octave;

    public int Value
        => Name.AbsoluteValue + (Octave.Value * Numbers.SemitonesInOctave);
    public int MidiNumber
        => Value + Numbers.SemitonesInOctave;
    public double Frequency
        => FrequencyEquations.GetFrequencyFromMidiNumber(MidiNumber);
    public string Symbol
        => Name.Symbol + Octave.Symbol;
    public int AlphabeticIndex
        => Name.AlphabeticIndex;
    public int Accidental
        => Name.Accidental.Value;

    private Pitch(NoteName noteName, OctaveNumber octaveNumber)
    {
        Name = noteName;
        Octave = octaveNumber;
    }

    public static Pitch CreateNew(NoteName name, OctaveNumber octave)
        => new Pitch(name, octave);

    public static Pitch CreateNew(int pitchClass, int accidental, int octaveNumber)
        => new Pitch(
            NoteName.CreateNew(pitchClass, accidental),
            OctaveNumber.CreateNew(octaveNumber));

    public static Pitch CreateNew(string noteNameSymbol, int octaveNumber)
        => new Pitch(
            NoteName.FromSymbol(noteNameSymbol),
            OctaveNumber.CreateNew(octaveNumber)
        );

    public static Pitch FromSymbol(string pitchSymbol)
    {
        var data = RegularExpressions.PitchSymbol.Match(pitchSymbol);
        if (!data.Success)
        {
            throw new InvalidAsSymbol(pitchSymbol, nameof(pitchSymbol));
        }
        string name = data.Groups[1].Value;
        string accidental = data.Groups[2].Value;
        string octave = data.Groups[3].Value;
        accidental ??= string.Empty;
        return CreateNew(name + accidental, int.Parse(octave));
    }

    public static Pitch FromFrequency(double frequency, bool useSharps = true)
    {
        List<string> dummyNotes = useSharps
            ? ["C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"]
            : ["C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"];
        List<string> notes = [.. Enumerable.Range(1, 9)
            .SelectMany(num => dummyNotes
                .Select(letter => letter + num.ToString())
            )
        ];
        int semitonesFromCentre = FrequencyEquations.GetSemitonesFromCentralFrequency(frequency);
        int A4 = notes.IndexOf(Symbols.CentralNoteName);
        string symbol = notes[A4 + semitonesFromCentre];
        return FromSymbol(symbol);
    }

    public int GetSemitonesBetween(Pitch pitch)
        => FrequencyEquations
            .GetSemitonesBetwenFrequencies(pitch.Frequency, Frequency);



    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Octave;
    }
}
