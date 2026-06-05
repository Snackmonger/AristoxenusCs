namespace Aristoxenus.Core.Domains.Theory.Atoms;

using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Errors;
using Aristoxenus.Core.Shared.Interfaces;
using Aristoxenus.Core.Shared.Utilities;

/// <summary>
/// Representation of a NoteName in the concrete tonal space of a range of
/// octaves.
/// </summary>
public class Pitch : ValueObject, ISymbolSerializable<Pitch>
{

    public NoteName Name;
    public OctaveNumber Octave;

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

    // ISymbolSerializable ////////////////////////////////////////////////////
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
        return new(name + accidental, int.Parse(octave));
    }
    public string ToSymbol()
        => Name.ToSymbol() + Octave.ToSymbol();
    string ISymbolSerializable<Pitch>.ToSymbol()
        => ToSymbol();

    static Pitch ISymbolSerializable<Pitch>.FromSymbol(string symbol)
        => FromSymbol(symbol);

    // Constructors, Factories ////////////////////////////////////////////////
    private Pitch(NoteName noteName, OctaveNumber octaveNumber)
    {
        Name = noteName;
        Octave = octaveNumber;
        Value = Name.AbsoluteValue + (Octave.Value * Numbers.SemitonesInOctave);
        MidiNumber = Value + Numbers.SemitonesInOctave;
        Frequency = FrequencyEquations.GetFrequencyFromMidiNumber(MidiNumber);
    }
    public Pitch(int pitchClass, int accidental, int octaveNumber) 
        : this(new NoteName(pitchClass, accidental), new(octaveNumber)) 
    { }

    public Pitch(string noteNameSymbol, int octaveNumber) 
        : this(NoteName.FromSymbol(noteNameSymbol), new(octaveNumber)) 
    { }

    /// <summary>
    /// Create a new instance from the given frequency.
    /// </summary>
    /// <param name="useSharps">Flag that indicates whether the output pitch 
    /// will use sharps (true) or flats (false), if necessary.</param>
    public static Pitch FromFrequency(double frequency, bool useSharps = true)
    {
        int octaves = (int)FrequencyEquations
            .GetOctavesBetweenFrequencies(
                Numbers.MinFrequency, 
                frequency
            );
        int identity = FrequencyEquations
            .GetSemitonesBetwenFrequencies(
                Numbers.MinFrequency, 
                frequency
            ) % Numbers.SemitonesInOctave;
        int nameIndex;
        int accidentals = Numbers.Zero;
        if (ScalePattern.Diatonic.Contains(identity))
        {
            nameIndex = ScalePattern.Diatonic.IndexOf(identity);
        }
        else if (useSharps)
        {
            nameIndex = ScalePattern.Diatonic.IndexOf(identity + Numbers.FlatModifier);
            accidentals = Numbers.SharpModifier;
        }
        else
        {
            nameIndex = ScalePattern.Diatonic.IndexOf(identity + Numbers.SharpModifier);
            accidentals = Numbers.FlatModifier;
        }
        
        return new(nameIndex, accidentals, octaves);
    }

    public static Pitch FromMidiNumber(int midiNumber)
        => FromFrequency(FrequencyEquations.GetFrequencyFromMidiNumber(midiNumber));


    // Public Api /////////////////////////////////////////////////////////////

    /// <summary>
    /// Return the number of semitones that exist between this pitch and
    /// another pitch.
    /// </summary>
    public int GetSemitonesBetween(Pitch pitch)
        => FrequencyEquations
            .GetSemitonesBetwenFrequencies(pitch.Frequency, Frequency);


    // ValueObject ////////////////////////////////////////////////////////////

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Octave;
    }

}
