using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Interfaces;
using Aristoxenus.Core.Shared.Utilities;

namespace Aristoxenus.Core.Domains.Theory.Models;

/// <summary>
/// Representation of a NoteName in the concrete tonal space of a range of
/// octaves.
/// </summary>
public class Pitch : ISymbolSerializable<Pitch>
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


    private Pitch(NoteName noteName, int octaveNumber)
    {
        Name = noteName;
        Octave = Math.Abs(octaveNumber % Numbers.MaxOctaveNumber);
        Value = Name.AbsoluteValue + (Octave * Numbers.SemitonesInOctave);
        MidiNumber = Value + Numbers.SemitonesInOctave;
        Frequency = FrequencyHelper.GetFrequencyFromMidiNumber(MidiNumber);
    }

    /// <summary>
    /// Create a new instance from the given frequency.
    /// </summary>
    /// <param name="useSharps">Flag that indicates whether the output pitch 
    /// will use sharps (true) or flats (false), if necessary.</param>
    public static Pitch FromFrequency(double frequency, bool useSharps = true)
    {
        ((int, int) name, int octave) = NomenclatureHelper.NamePitchAtFrequency(frequency, useSharps);
        return new(name, octave);
    }


    public static Pitch FromMidiNumber(int midiNumber)
        => FromFrequency(
            FrequencyHelper.GetFrequencyFromMidiNumber(midiNumber)
        );


    // Public Api /////////////////////////////////////////////////////////////

    /// <summary>
    /// Return the number of semitones that exist between this pitch and
    /// another pitch.
    /// </summary>
    public int GetSemitonesBetween(Pitch pitch)
        => FrequencyHelper.GetSemitonesBetwenFrequencies(pitch.Frequency, Frequency);


    public static Pitch FromSymbol(string pitchSymbol)
    {
        ((int, int) noteName, int octave) = SymbolHelper.ParsePitchSymbol(pitchSymbol);
        return new(noteName, octave);
    }

    public string ToSymbol() => SymbolHelper.CreatePitchSymbol(Name, Octave);
    string ISymbolSerializable<Pitch>.ToSymbol() => ToSymbol();
    static Pitch ISymbolSerializable<Pitch>.FromSymbol(string symbol)=> FromSymbol(symbol);


    public static implicit operator string(Pitch p) => p.ToSymbol();

    public static implicit operator Pitch(string p) => FromSymbol(p);

    public static implicit operator double(Pitch p) => p.Frequency;

    public static implicit operator Pitch(double p) => FromFrequency(p);
}
