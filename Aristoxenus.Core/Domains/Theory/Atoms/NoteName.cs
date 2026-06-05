using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Errors;
using Aristoxenus.Core.Shared.Interfaces;
using Aristoxenus.Core.Shared.Utilities;

namespace Aristoxenus.Core.Domains.Theory.Atoms;

/// <summary>
/// Representation of a musical note name, consisting of a pitch class and 
/// an accidental, which represents a position within an abstract octave.
/// </summary>
public class NoteName : ValueObject, ISymbolSerializable<NoteName>
{
    /// <summary>
    /// The alphabetic part of the note name
    /// </summary>
    public readonly PitchClass PitchClass;

    /// <summary>
    /// The accidental adjustment of the note name.
    /// </summary>
    public readonly Accidental Accidental;

    /// <summary>
    /// The index of this note's pitch class (e.g. C=0, D=1, etc.)
    /// </summary>
    public readonly int PitchClassIndex;

    /// <summary>
    /// Flag indicating whether this note is a natural note or is 
    /// enharmonically equivalent to a natural note (e.g. E# = F).
    /// </summary>
    public readonly bool IsEnharmonicallyNatural;

    /// <summary>
    /// The note name's value, expressed as an absolute index in a idealized 
    /// twelve-tone octave, such that C=0, C#=1, Db=1, ..., B=11, B#=0, 
    /// and Cb=11.
    /// </summary>
    public readonly int AbsoluteValue;


    /// <summary>
    /// The simplest form of the name, which will be a natural or a note name
    /// with a single accidental, of the same type with which the note 
    /// started, e.g. A#### will be simplified to C#, and Fbbbb will be 
    /// simplified to Db.
    /// </summary>
    public readonly NoteName SimplifiedName;


    // Constructors, Factories ////////////////////////////////////////////////
    public NoteName(PitchClass pitchClass, Accidental accidental)
    {
        PitchClass = pitchClass;
        Accidental = accidental;
        AbsoluteValue =
            (RawScalePatterns.Diatonic[PitchClassIndex] + Accidental.Value)
            % Numbers.SemitonesInOctave;
        PitchClassIndex = PitchClass.Value;
        (int n, int a) = NomenclatureHelper.GetSimplifiedName(PitchClass.Value, Accidental.Value);
        // Simplified name is only needed if this is not already the simplest
        // name. NB: This check prevents an infinite recursion.
        if (n != PitchClass.Value && a != Accidental.Value)
            SimplifiedName = new NoteName(n, a);
        else
            SimplifiedName = this;
        IsEnharmonicallyNatural = SimplifiedName.Accidental.Value == 0;
    }

    /// <summary>
    /// For any note for which two names exist, return the other note name with
    /// the same relative value (e.g. Bb > A#, Db > C#, C > B#, Cb > B, etc.)
    /// </summary>
    public NoteName FlipBinomial()
    {
        (int alpha, int accidentals) = NomenclatureHelper.FlipBinomial(PitchClass.Value, Accidental.Value);
        return new NoteName(new PitchClass(alpha), new Accidental(accidentals));
    }

    /// <summary>
    /// Calculate the note name that lies at the specified interval above 
    /// the given note name (e.g. From C, a perfect fifth above gives G;
    /// from G, a perfect fifth above gives D).
    /// </summary>
    public NoteName NoteNameAbove(Interval interval)
    {


        // From G to bb7
        int accidentals = 0;
        int nameOffset = (PitchClassIndex + interval.Degree - Numbers.IndexOffset); // 4 + 7 - 1 = 10
        int nameIndex = nameOffset % Numbers.NotesInOctave;                         // 10 % 7 = 3
        int originalValue = ScalePattern.Diatonic[nameIndex];            // diatonic[3] = (F) = 5
        int intervalDegreeNaturalValue = RawScalePatterns.Diatonic[interval.Degree - Numbers.IndexOffset]; // diatonic[6] = 11
        int intervalDegreeActualValue = (intervalDegreeNaturalValue + interval.Accidental)
            % Numbers.SemitonesInOctave;                                            // (11 + -2 = 9) % 12 = 9

        // else, check what the diatonic name would be with this note as tonic      // F#
        // if diatonic name.Accidental > 0 : accidentals -= name.Accidental         // F  =>                      
        // if the name 
        List<int> diatonicMode = [];
        if (intervalDegreeNaturalValue > intervalDegreeActualValue)
        {
            accidentals += diatonicMode[interval.Degree - Numbers.IndexOffset] - intervalDegreeActualValue;
        }
        else if (intervalDegreeNaturalValue < intervalDegreeActualValue)
        {
            accidentals += intervalDegreeActualValue - diatonicMode[interval.Degree - Numbers.IndexOffset];
        }
        else
        {
            Console.WriteLine("Unexpected Condition");
        }

        return new(nameIndex, accidentals);
    }

    /// <summary>
    /// Calculate the note name that lies at the specified interval
    /// above the given note name.
    /// </summary>
    public NoteName NoteNameAbove(string interval)
        => NoteNameAbove(Interval.FromSymbol(interval));

    /// <summary>
    /// Calculate the interval that exists between this note name and a 
    /// higher note name (e.g. Lower C to higher G is a perfect fifth,
    /// but a lower G to a higher C is a perfect fourth).
    public Interval IntervalAbove(NoteName highNote)
    {
        int degree = PitchClassIndex > highNote.PitchClassIndex
            ? highNote.PitchClassIndex + PitchClassIndex
            : PitchClassIndex - highNote.PitchClassIndex;
        if (degree < 0)
        {
            degree += Numbers.NotesInOctave;
        }
        int highValue = highNote.AbsoluteValue;
        int lowValue = AbsoluteValue;
        if (highValue < lowValue)
        {
            highValue += Numbers.SemitonesInOctave;
        }
        return Interval.FromExpectedValue(degree, Math.Abs(highValue - lowValue));
    }

    /// <summary>
    /// Calculate the interval that exists between this 
    /// note name and a higher note name.
    public Interval IntervalAbove(string highNote)
        => IntervalAbove(FromSymbol(highNote));

    /// <summary>
    /// Calculate the interval that exists between this 
    /// note name and a lower note name.
    public Interval IntervalBelow(NoteName lowNote)
        => lowNote.IntervalAbove(this);

    /// <summary>
    /// Calculate the interval that exists between this 
    /// note name and a lower note name.
    public Interval IntervalBelow(string lowNote)
        => IntervalBelow(FromSymbol(lowNote));


    // ISymbolSerializable ////////////////////////////////////////////////////
    public string ToSymbol()
        => PitchClass.ToSymbol() + Accidental.ToSymbol();

    public static NoteName FromSymbol(string noteNameSymbol)
    {
        Accidental accidental;
        var data = RegularExpressions.NoteName.Match(noteNameSymbol);
        if (!data.Success)
        {
            throw new InvalidAsSymbol(noteNameSymbol, nameof(noteNameSymbol));
        }
        var groups = data.Groups;
        string naturalNoteName = groups[1].Value;
        if (groups[2].Value != null)
        {
            int sharps = RegularExpressions.SharpSymbol.Count(groups[2].Value);
            int flats = RegularExpressions.FlatSymbol.Count(groups[2].Value);
            accidental = new(sharps - flats);
        }
        else
        {
            accidental = Accidental.Natural;
        }

        return new(PitchClass.FromSymbol(naturalNoteName), accidental);
    }

    string ISymbolSerializable<NoteName>.ToSymbol()
    {
        throw new NotImplementedException();
    }

    static NoteName ISymbolSerializable<NoteName>.FromSymbol(string symbol)
    {
        throw new NotImplementedException();
    }

    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ToSymbol();
    }

    // Casts //////////////////////////////////////////////////////////////////
    public static implicit operator string(NoteName noteName)
        => noteName.ToSymbol();

    public static implicit operator NoteName(string noteName)
        => NoteName.FromSymbol(noteName);

}


