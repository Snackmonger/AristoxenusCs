namespace Aristoxenus.Core.Domain.Theory;

using System.Collections.Generic;

using Aristoxenus.Core.Constants;
using Aristoxenus.Core.Domain.Exceptions;
using Aristoxenus.Core.SharedKernel.Bases;
using Aristoxenus.Core.SharedKernel.Interfaces;
using Aristoxenus.Core.Utilities;


/// <summary>
/// Representation of a musical note name, consisting of a pitch class and an
/// accidental, which represents a position within an abstract octave.
/// </summary>
public class NoteName : ValueObject, ISymbolSerializable<NoteName>
{

    public readonly PitchClass PitchClass;
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
    /// twelve-tone octave, such that C=1, C#=2, Db=2, ..., B=12, B#=1, 
    /// and Cb=12.
    /// </summary>
    public readonly int AbsoluteValue;


    /// <summary>
    /// The simplest form of the name, which will be a natural or a note name
    /// with a single accidental, of the same type with which the note 
    /// started, e.g. A#### will be simplified to C#, and Fbbbb will 
    /// be simplified to Db.
    /// </summary>
    public readonly NoteName SimplifiedName;


    // Constructors, Factories ////////////////////////////////////////////////
    private NoteName(PitchClass pitchClass, Accidental accidental)
    {
        PitchClass = pitchClass;
        Accidental = accidental;
        AbsoluteValue =
            (ScalePatternsHeptatonic.Diatonic[PitchClassIndex] + Accidental.Value)
            % Numbers.SemitonesInOctave;
        PitchClassIndex = PitchClass.Value;
        (int n, int a) = NomenclatureHelper.GetSimplifiedName(PitchClass.Value, Accidental.Value);
        // Simplified name is only needed if this is not already the simplest
        // name (prevents infinite recursion).
        if (n != PitchClass.Value && a != Accidental.Value) 
            SimplifiedName = CreateNew(n, a);
        else 
            SimplifiedName = this;
        IsEnharmonicallyNatural = SimplifiedName.Accidental.Value == 0;
    }

    public static NoteName CreateNew(PitchClass pitchClass, Accidental? accidental)
        => new(pitchClass, accidental ?? Accidental.CreateNew(0));


    public static NoteName CreateNew(int pitchClass, int? accidental)
        => CreateNew(
            PitchClass.CreateNew(pitchClass),
            accidental == null
                ? Accidental.CreateNew(0)
                : Accidental.CreateNew((int)accidental));

    public static NoteName CreateNew(string noteNameSymbol)
        => FromSymbol(noteNameSymbol);

    public static NoteName CreateFromAbsoluteValue(int absoluteValue, bool useSharps = true)
    {
        int accidental;
        int nameIndex;
        if (ScalePatternsHeptatonic.Diatonic.Contains(absoluteValue))
        {
            accidental = 0;
            nameIndex = ScalePatternsHeptatonic.Diatonic.IndexOf(absoluteValue);
        }
        else if (useSharps)
        {
            accidental = Numbers.SharpModifier;
            nameIndex = absoluteValue + Numbers.FlatModifier;
        }
        else
        {
            nameIndex = absoluteValue + Numbers.SharpModifier;
            accidental = Numbers.FlatModifier;
        }
        return CreateNew(nameIndex, accidental);
    }

    // Public Api /////////////////////////////////////////////////////////////

    /// <summary>
    /// Return a new note name with the accidental flipped between sharp and 
    /// flat, preserving its relative meaning. This operation is only valid 
    /// for note names with exactly one accidental.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the current note 
    /// name does not have exactly one accidental.</exception>
    public NoteName FlipBinomial()
    {
        if (Accidental.Value is 0 or > 1 or < -1)
        {
            throw new InvalidOperationException($"The method '{nameof(FlipBinomial)}' is intended for note names with exactly one accidental.");
        }
        int alpha = PitchClassIndex;
        int accidentals = Accidental.Value;
        if (accidentals == Numbers.SharpModifier)
        {
            alpha = alpha == Symbols.NaturalNoteNames.Length - 1 ? 0 : alpha + Numbers.SharpModifier;
            accidentals *= Numbers.FlatModifier;
        }
        if (accidentals == Numbers.FlatModifier)
        {
            alpha = alpha == 0 ? Symbols.NaturalNoteNames.Length - 1 : alpha + Numbers.FlatModifier;
            accidentals *= Numbers.SharpModifier;
        }

        return CreateNew(PitchClass.CreateNew(alpha), Accidental.CreateNew(accidentals));
    }

    /// <summary>
    /// Calculate the note name that lies at the specified interval
    /// above the given note name.
    /// </summary>
    public NoteName NoteNameAbove(Interval interval)
    {
        int nameOffset = (PitchClassIndex + interval.Degree - 1);
        int nameIndex = nameOffset % Numbers.NotesInOctave;
        int originalValue = ScalePatternsHeptatonic.Diatonic[nameIndex];
        int semitonesOffset = (nameOffset / Numbers.NotesInOctave) * Numbers.SemitonesInOctave;
        int relativeValue = AbsoluteValue + interval.Name.Value;
        int adjustment = (relativeValue - (originalValue + semitonesOffset));
        int accidentals = ((adjustment % Numbers.SemitonesInOctave) + Numbers.SemitonesInOctave) % Numbers.SemitonesInOctave;

        // Since a remainder must always be positive, the modulus will
        // give the inversion of what we actually want here if the note
        // is flat (e.g. E########### for Eb), therefore we compensate.
        if (adjustment < 0)
        {
            accidentals -= Numbers.SemitonesInOctave;
        }
        return CreateNew(nameIndex, accidentals);
    }

    /// <summary>
    /// Calculate the note name that lies at the specified interval
    /// above the given note name.
    /// </summary>
    public NoteName NoteNameAbove(string interval)
        => NoteNameAbove(Interval.FromSymbol(interval));

    /// <summary>
    /// Calculate the interval that exists between this 
    /// note name and a higher note name.
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
            accidental = Accidental.CreateNew(sharps - flats);
        }
        else
        {
            accidental = Accidental.CreateNew(0);
        }

        return CreateNew(PitchClass.CreateNew(naturalNoteName), accidental);
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


