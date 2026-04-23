namespace Aristoxenus.Core.Domain.Theory;

using Aristoxenus.Core.Constants;
using Aristoxenus.Core.Domain.Exceptions;
using Aristoxenus.Core.SharedKernel.Bases;
using Aristoxenus.Core.SharedKernel.Interfaces;
using Aristoxenus.Core.Utilities;

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
        return CreateNew(name + accidental, int.Parse(octave));
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

    public static Pitch CreateNew(NoteName name, OctaveNumber octave)
        => new(name, octave);

    public static Pitch CreateNew(int pitchClass, int accidental, int octaveNumber)
        => CreateNew(
            NoteName.CreateNew(pitchClass, accidental),
            OctaveNumber.CreateNew(octaveNumber));

    public static Pitch CreateNew(string noteNameSymbol, int octaveNumber)
        => CreateNew(
            NoteName.FromSymbol(noteNameSymbol),
            OctaveNumber.CreateNew(octaveNumber));

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
        if (ScalePatternsHeptatonic.Diatonic.Contains(identity))
        {
            nameIndex = ScalePatternsHeptatonic.Diatonic.IndexOf(identity);
        }
        else if (useSharps)
        {
            nameIndex = ScalePatternsHeptatonic.Diatonic.IndexOf(identity + Numbers.FlatModifier);
            accidentals = Numbers.SharpModifier;
        }
        else
        {
            nameIndex = ScalePatternsHeptatonic.Diatonic.IndexOf(identity + Numbers.SharpModifier);
            accidentals = Numbers.FlatModifier;
        }

        return CreateNew(nameIndex, accidentals, octaves);
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
