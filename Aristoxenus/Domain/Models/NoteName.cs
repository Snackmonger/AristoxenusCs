namespace Aristoxenus.Domain.Models;

using Aristoxenus.Constants;
using Aristoxenus.Domain.Exceptions;
using Aristoxenus.SharedKernel.Bases;
using Aristoxenus.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// Representation of a musical note name, consisting of a pitch class and an
/// accidental, which represents a position within an abstract octave.
/// </summary>
public class NoteName : ValueObject, ISymbolSerializable<NoteName>
{
    public readonly PitchClass PitchClass;
    public readonly Accidental Accidental;

    public string Symbol
        => new StringBuilder()
                .Append(PitchClass.Symbol)
                .Append(Accidental.Symbol)
                .ToString();
    public int AlphabeticIndex
        => PitchClass.Value;
    public bool IsEnharmonicallyNatural
        => SimplifiedName.Accidental.Value == 0;
    public int AbsoluteValue
        => (RawScalePatterns.Diatonic[AlphabeticIndex] + Accidental.Value) % Numbers.SemitonesInOctave;
    public string MusicXMLStep
        => PitchClass.Symbol;
    public NoteName SimplifiedName
    {
        get
        {
            if (Accidental.Value == 0)
            {
                return this;
            }
            int i;
            int offsetValue = (RawScalePatterns.Diatonic[AlphabeticIndex] + Accidental.Value) % Numbers.SemitonesInOctave;
            if (RawScalePatterns.Diatonic.Contains(offsetValue))
            {
                i = RawScalePatterns.Diatonic.IndexOf(offsetValue);
                string name = Symbols.NaturalNoteNames[i].ToString();
                return CreateNew(
                    PitchClass.CreateNew(name),
                    Accidental.CreateNew(0)
                );
            }
            if (Accidental.Value > 0)
            {
                offsetValue -= 1;
                i = RawScalePatterns.Diatonic.IndexOf(offsetValue);
                return CreateNew(PitchClass.CreateNew(Symbols.NaturalNoteNames[i].ToString()), Accidental.CreateNew(1));

            }
            offsetValue += 1;
            i = RawScalePatterns.Diatonic.IndexOf(offsetValue);
            return CreateNew(PitchClass.CreateNew(Symbols.NaturalNoteNames[i].ToString()), Accidental.CreateNew(-1));
        }
    }

    private NoteName(PitchClass pitchClass, Accidental accidental)
    {
        PitchClass = pitchClass;
        Accidental = accidental;
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
    {
        return FromSymbol(noteNameSymbol);
    }

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



    /// <summary>
    /// Returns a new note name with the accidental flipped between sharp and 
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
        int alpha = AlphabeticIndex;
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
        int nameOffset = (AlphabeticIndex + interval.Degree - 1);
        int nameIndex = nameOffset % Numbers.NotesInOctave;
        int originalValue = RawScalePatterns.Diatonic[nameIndex];
        int semitonesOffset = (nameOffset / Numbers.NotesInOctave) * Numbers.SemitonesInOctave;
        int relativeValue = AbsoluteValue + interval.Name.Value;
        int adjustment = (relativeValue - (originalValue + semitonesOffset));
        int accidentals = (adjustment % Numbers.SemitonesInOctave + Numbers.SemitonesInOctave) % Numbers.SemitonesInOctave;

        // Since a remainder must always be positive, the modulus will
        // give the inversion of what we actually want here if the note
        // is flat (e.g. E########### for Eb), therefore we compensate.
        if (adjustment < 0)
        {
            accidentals -= Numbers.SemitonesInOctave;
        }
        return CreateNew(nameIndex, accidentals);
    }

    public NoteName NoteNameAbove(string interval)
    {
        return NoteNameAbove(Interval.FromSymbol(interval));
    }

    public Interval IntervalAbove(NoteName highNote)
    {
        int degree = AlphabeticIndex > highNote.AlphabeticIndex ? highNote.AlphabeticIndex + AlphabeticIndex : AlphabeticIndex - highNote.AlphabeticIndex;
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

    public Interval IntervalAbove(string highNote)
    {
        return IntervalAbove(FromSymbol(highNote));
    }

    public Interval IntervalBelow(NoteName lowNote)
    {
        return lowNote.IntervalAbove(this);
    }

    public Interval IntervalBelow(string lowNote)
    {
        return IntervalBelow(FromSymbol(lowNote));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Symbol;
    }
}


