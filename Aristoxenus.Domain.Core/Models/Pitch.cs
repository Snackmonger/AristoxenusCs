using Aristoxenus.Domain.Core.Bases;
using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Errors;
using Aristoxenus.Domain.Core.Utilities;
using Aristoxenus.Domain.Core.Interfaces;

namespace Aristoxenus.Domain.Core.Models;

/// <summary>
/// Representation of a NoteName in the concrete tonal space of a range of
/// octaves.
/// </summary>
public class Pitch :  ValueObject, ISymbolSerializable<Pitch>
{

    public NoteName Name;
    public int Octave;

    /// <summary>
    /// The absolute value of this pitch, expressed as an integer between 1
    /// and 108.
    /// </summary>
    public readonly int Value;

    /// <summary>
    /// The Midi number of this pitch, expressed as an integer between 12 
    /// and 119.
    /// </summary>
    public readonly int MidiNumber;

    /// <summary>
    /// The frequency of this pitch.
    /// </summary>
    public readonly double Frequency;


    public Pitch(NoteName noteName, int octaveNumber)
    {
        Name = noteName;
        Octave = Math.Abs(octaveNumber % (Numbers.MaxOctaveNumber + Numbers.IndexOffset));
        Value = Name.AbsoluteValue + (Octave * Numbers.SemitonesInOctave);
        MidiNumber = Value + Numbers.SemitonesInOctave;
        Frequency = FrequencyHelper.GetFrequencyFromMidiNumber(MidiNumber);
    }

    /// <summary>
    /// Create a new instance from the given <paramref name="noteName"/>
    /// and <paramref name="expectedValue"/>.
    /// </summary>
    public static Pitch FromExpectedValue(NoteName noteName, int expectedValue)
    {
        if (expectedValue % Numbers.SemitonesInOctave != noteName.AbsoluteValue)
            throw new PitchValueRepresentationMismatch(noteName, expectedValue);
        return new Pitch(noteName, expectedValue / Numbers.SemitonesInOctave);
    }

    /// <summary>
    /// Create a new instance from the given <paramref name="frequency"/>.
    /// </summary>
    /// <param name="useSharps">Flag that indicates whether the output pitch 
    /// will use sharps (true) or flats (false), if necessary.</param>
    public static Pitch FromFrequency(double frequency, bool useSharps = true)
    {
        ((int, int) name, int octave) = NomenclatureHelper.NamePitchAtFrequency(frequency, useSharps);
        return new(name, octave);
    }

    /// <summary>
    /// Create a new instance from the given <paramref name="midiNumber"/>.
    /// </summary>
    /// <param name="useSharps">Flag that indicates whether the output pitch 
    /// will use sharps (true) or flats (false), if necessary.</param>
    public static Pitch FromMidiNumber(int midiNumber, bool useSharps = true)
        => FromFrequency(
            FrequencyHelper.GetFrequencyFromMidiNumber(midiNumber), useSharps
        );

    /// <summary>
    /// Return the number of semitones that exist between this pitch and
    /// another pitch.
    /// </summary>
    public int GetSemitonesBetween(Pitch pitch)
        => FrequencyHelper.GetSemitonesBetwenFrequencies(pitch.Frequency, Frequency);

    /// <summary>
    /// Get the pitch that stands N <paramref name="semitones"/> below from this
    /// pitch, expressed in its simplest form, using sharps or flats according
    /// to the flag <paramref name="useSharps"/>.
    /// </summary>
    public Pitch GetPitchBelow(int semitones, bool useSharps = true) 
        => FromMidiNumber(MidiNumber - semitones, useSharps);

    /// <summary>
    /// Get the pitch that stands N <paramref name="semitones"/> above from this
    /// pitch, expressed in its simplest form, using sharps or flats according
    /// to the flag <paramref name="useSharps"/>.
    /// </summary>
    public Pitch GetPitchAbove(int semitones, bool useSharps = true) 
        => FromMidiNumber(MidiNumber + semitones, useSharps);

    /// <summary>
    /// Get the pitch that stands at the given interval below this pitch.
    /// </summary>
    public Pitch GetPitchBelow(Interval interval)
    {
        var noteName = Name.NoteNameBelow(interval);
        int value = Value - interval.Value;
        return FromExpectedValue(noteName, value);
    }

    /// <summary>
    /// Get the pitch that stands at the given interval above this pitch.
    /// </summary>
    public Pitch GetPitchAbove(Interval interval)
    {
        var noteName = Name.NoteNameAbove(interval);
        int value = Value + interval.Value;
        return FromExpectedValue(noteName, value);
    }

    public static Pitch FromSymbol(string pitchSymbol)
    {
        ((int, int) noteName, int octave) = SymbolHelper.ParsePitchSymbol(pitchSymbol);
        return new(noteName, octave);
    }

    public string ToSymbol() => SymbolHelper.CreatePitchSymbol(Name, Octave);
    string ISymbolSerializable<Pitch>.ToSymbol() => ToSymbol();
    static Pitch ISymbolSerializable<Pitch>.FromSymbol(string symbol) => FromSymbol(symbol);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Octave;
    }

    public static implicit operator string(Pitch p) => p.ToSymbol();

    public static implicit operator Pitch(string p) => FromSymbol(p);

    public static implicit operator double(Pitch p) => p.Frequency;

    public static implicit operator Pitch(double p) => FromFrequency(p);
}
