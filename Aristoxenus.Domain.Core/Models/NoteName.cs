using Aristoxenus.Domain.Core.Bases;
using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Interfaces;
using Aristoxenus.Domain.Core.Utilities;


namespace Aristoxenus.Domain.Core.Models;

/// <summary>
/// Representation of a musical note name, consisting of a pitch class and 
/// an accidental, which represents a position within an abstract octave.
/// </summary>
public class NoteName(int pitchClass, int accidental) : ValueObject, ISymbolSerializable<NoteName>
{
    /// <summary>
    /// The alphabetic part of the note name
    /// </summary>
    public readonly int PitchClass = NormalizationHelper.NormalizePitchClass(pitchClass);

    /// <summary>
    /// The accidental adjustment of the note name.
    /// </summary>
    public readonly int Accidental = NormalizationHelper.NormalizeAccidental(accidental);

    /// <summary>
    /// Flag indicating whether this note is a natural note or is 
    /// enharmonically equivalent to a natural note (e.g. E# = F).
    /// </summary>
    public bool IsEnharmonicallyNatural
        => SimplifiedName.Accidental == Numbers.Zero;

    /// <summary>
    /// The note name's value, expressed as an absolute index in a idealized 
    /// twelve-tone octave, such that C=0, C#=1, Db=1, ..., B=11, B#=0, 
    /// and Cb=11.
    /// </summary>
    public int AbsoluteValue
        => (RawScalePatterns.Diatonic[PitchClass] + Numbers.SemitonesInOctave + Accidental) % Numbers.SemitonesInOctave;

    /// <summary>
    /// The simplest form of the name, which will be a natural or a note name
    /// with a single accidental, of the same type with which the note 
    /// started, e.g. A#### will be simplified to C#, and Fbbbb will be 
    /// simplified to Db.
    /// </summary>
    public NoteName SimplifiedName
        => NomenclatureHelper.GetSimplifiedName(PitchClass, Accidental);

    /// <summary>
    /// For any note for which two names exist, return the other note name with
    /// the same relative value (e.g. Bb > A#, Db > C#, C > B#, Cb > B, etc.).
    /// </summary>
    public NoteName FlipBinomial()
        => NomenclatureHelper.FlipBinomial(PitchClass, Accidental);

    /// <summary>
    /// Calculate the note name that lies at the specified interval below 
    /// the given note name.
    /// </summary>
    public NoteName NoteNameBelow(Interval interval)
        => NomenclatureHelper.GetNoteNameAbove(this, interval.Reciprocal);

    /// <summary>
    /// Calculate the note name that lies at the specified interval below
    /// the given note name.
    /// </summary>
    public NoteName NoteNameBelow(string interval)
        => NoteNameBelow(Interval.FromSymbol(interval));

    /// <summary>
    /// Calculate the note name that lies at the specified interval above 
    /// the given note name.
    /// </summary>
    public NoteName NoteNameAbove(Interval interval)
        => NomenclatureHelper.GetNoteNameAbove(this, interval);

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
        => NomenclatureHelper.GetIntervalNameBetweenNoteNames(this, highNote);

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

    /// <summary>
    /// Return true if this note and the other note are two different 
    /// spellings of the same note.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsEnharmonicallyEquivalent(NoteName other)
        => AbsoluteValue == other.AbsoluteValue;

    string ISymbolSerializable<NoteName>.ToSymbol()
        => SymbolHelper.CreateNoteNameSymbol((PitchClass, Accidental));

    static NoteName ISymbolSerializable<NoteName>.FromSymbol(string noteNameSymbol)
        => SymbolHelper.ParseNoteNameSymbol(noteNameSymbol);

    public string ToSymbol()
        => SymbolHelper.CreateNoteNameSymbol(this);

    public static string ToSymbol(NoteName noteName)
        => SymbolHelper.CreateNoteNameSymbol(noteName);

    public static NoteName FromSymbol(string noteNameSymbol)
        => SymbolHelper.ParseNoteNameSymbol(noteNameSymbol);

    public static string PrettyPrintSymbols(IEnumerable<NoteName> noteNames)
        => string.Join(", ", noteNames.Select(ToSymbol));

    public static IEnumerable<NoteName> FromSymbols(IEnumerable<string> noteNameSymbols)
        => noteNameSymbols.Select(FromSymbol);

    public static implicit operator string(NoteName noteName)
        => noteName.ToSymbol();

    public static implicit operator NoteName(string noteNameSymbol)
        => SymbolHelper.ParseNoteNameSymbol(noteNameSymbol);

    public static implicit operator (int pitchClass, int accidental)(NoteName noteName)
        => (noteName.PitchClass, noteName.Accidental);

    public static implicit operator NoteName((int pitchClass, int accidental) noteNameRawValues)
        => new(noteNameRawValues.pitchClass, noteNameRawValues.accidental);

    public void Deconstruct(out int pitchClass, out int accidental)
        => (pitchClass, accidental) = (PitchClass, Accidental);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PitchClass;
        yield return Accidental;
    }

    public static NoteName CFlat => new(0, -1);
    public static NoteName C => new(0, 0);
    public static NoteName CSharp => new(0, 1);
    public static NoteName DFlat => new(1, -1);
    public static NoteName D => new(1, 0);
    public static NoteName DSharp => new(1, 1);
    public static NoteName EFlat => new(2, -1);
    public static NoteName E => new(2, 0);
    public static NoteName ESharp => new(2, 1);
    public static NoteName FFlat => new(3, -1);
    public static NoteName F => new(3, 0);
    public static NoteName FSharp => new(3, 1);
    public static NoteName GFlat => new(4, -1);
    public static NoteName G => new(4, 0);
    public static NoteName GSharp => new(4, 1);
    public static NoteName AFlat => new(5, -1);
    public static NoteName A => new(5, 0);
    public static NoteName ASharp => new(5, 1);
    public static NoteName BFlat => new(6, -1);
    public static NoteName B => new(6, 0);
    public static NoteName BSharp => new(6, 1);

}


