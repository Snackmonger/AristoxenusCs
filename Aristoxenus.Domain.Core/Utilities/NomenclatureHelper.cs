using Libraries.Arithmology;
using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Errors;

namespace Aristoxenus.Domain.Core.Utilities;

using NoteName = (int PitchClass, int Accidental);
using DegreeName = (int Degree, int Accidental);

public static class NomenclatureHelper
{

    /**
     * Get a sort value for the given interval name, which is a decimal 
     * representation of the interval's degree and accidental.
     * 
     * E.g. 5 == 5.0, b5 == 4.9, #5 == 5.1, ##5 == 5.2, etc.
     */
    // 05-June-2026: Revised logic of ordering. Shouldn't affect anything
    // except very weird scales. Rules are:
    // - Any natural gets its integer.
    // - Sharps get 0.1 added to the natural.
    // - Flats get 0.1 removed from the natural.
    // - Naturally, this system means that there would be collisions for 
    // extreme accidentals more than +/- 5 steps, but in practice the intervals
    // will be derived from chords and scales in context, so such accidentals
    // are not really expected and accounted for on an ad hoc basis.
    public static double GetIntervalSortValue(DegreeName interval)
        => interval.Degree + (interval.Accidental * 0.1);

    /// <summary>
    /// Create a new interval degree name with the degree named as given, plus 
    /// as many accidentals as necessary to provide the expected value.
    /// </summary>
    /// <param name="expectedDegree">The degree you want the interval to be (e.g. 5)</param>
    /// <param name="expectedValue">The value you want the interval to have (e.g. 6)</param>
    /// <remarks>Note that any value greater than two octaves will be reduced
    /// to fit within two octaves.</remarks>
    /// <returns>An interval name with the desired degree and value (e.g. b5 == (5, 6))</returns>
    public static DegreeName GetIntervalDegreeNameFromExpectedValue(int expectedDegree, int expectedValue)
    {
        expectedValue = Math.Abs(expectedValue);
        expectedDegree = NormalizationHelper.NormalizeDegree(expectedDegree);
        int index = (expectedDegree - Numbers.IndexOffset) % Numbers.NotesInOctave;
        int[] diatonicScale = RawScalePatterns.Diatonic;
        int normalValue = diatonicScale[index];
        int accidental = (expectedValue % Numbers.SemitonesInOctave) - normalValue;
        return (expectedDegree, accidental);
    }

    /// <summary>
    /// Get the absolute value of the interval from its degree name. 
    /// NB: Degree names cannot be recovered from absolute values, since 
    /// multiple names can have the same value (e.g. b5 and #4 have a value 
    /// of 6).
    /// </summary>
    public static int GetIntervalValueFromDegreeName(DegreeName intervalName)
    {
        int degree;
        int naturalValue;
        int scalingFactor = 0;
        if (intervalName.Degree > Numbers.NotesInOctave)
        {
            degree = intervalName.Degree % Numbers.NotesInOctave;
            scalingFactor = intervalName.Degree / Numbers.NotesInOctave;
        }
        else
            degree = intervalName.Degree;
        naturalValue = RawScalePatterns.Diatonic[degree - Numbers.IndexOffset] + (scalingFactor * Numbers.SemitonesInOctave);
        return naturalValue + intervalName.Accidental;
    }


    /// <summary>
    /// Calculate the degree name of the interval that exists between a lower 
    /// note name and a higher note name.
    /// NB: This calculation is fundamentally limted to one octave since it
    /// handles bare note names (e.g. E, G#, Bb) without reference to their
    /// position in absolute pitch space. 
    /// </summary>
    public static DegreeName GetIntervalNameBetweenNoteNames(NoteName lowerNote, NoteName higherNote)
    {
        List<NoteName> diatonicScaleFromLowerNote = CalculateHeptatonicNames(lowerNote, RawScalePatterns.Diatonic);
        int degree = diatonicScaleFromLowerNote.FindIndex(it => it.PitchClass == higherNote.PitchClass) + Numbers.IndexOffset;
        DegreeName diatonicInterval = diatonicScaleFromLowerNote[degree - Numbers.IndexOffset];
        int expectedValue = RawScalePatterns.Diatonic[degree - Numbers.IndexOffset] + (higherNote.Accidental - diatonicInterval.Accidental);
        (degree, int accidentals) = GetIntervalDegreeNameFromExpectedValue(degree, expectedValue);

        return (degree, accidentals);
    }


    /// <summary>
    /// Get the name of the note that is situated at the interval with the
    /// given <paramref name="degreeName"/> above the given 
    /// <paramref name="noteName"/>
    /// </summary>
    public static NoteName GetNoteNameAbove(NoteName noteName, DegreeName degreeName)
    {
        int intervalValue = RawScalePatterns
            .Diatonic[degreeName.Degree - Numbers.IndexOffset] + degreeName.Accidental;

        List<(int alphabeticIndex, int accidentals)> diatonicScaleFromThisTonic
            = CalculateHeptatonicNames(noteName, RawScalePatterns.Diatonic);

        int diatonicValueAtIntervalDegree = RawScalePatterns
            .Diatonic[(degreeName.Degree - Numbers.IndexOffset) % Numbers.NotesInOctave];

        NoteName newName = diatonicScaleFromThisTonic[degreeName.Degree - Numbers.IndexOffset];

        if (diatonicValueAtIntervalDegree != intervalValue)
        {
            if (diatonicValueAtIntervalDegree > intervalValue)
            {
                newName.Accidental -= diatonicValueAtIntervalDegree - intervalValue;
            }
            if (diatonicValueAtIntervalDegree < intervalValue)
            {
                newName.Accidental += intervalValue - diatonicValueAtIntervalDegree;
            }
        }
        newName.Accidental %= Numbers.SemitonesInOctave;

        return newName;
    }



    /// <summary>
    /// Return the enharmonically simplified form of the given note name. 
    /// </summary>
    /// <remarks>
    /// A note name is in its simplest form when it has the fewest possible
    /// accidentals while maintaining the same pitch value. 
    /// 
    /// E.g. Cb can remove one accidental to become the note B while 
    /// maintaining the same value. Therefore, the simplest form of Cb is B.
    /// 
    /// D## can remove two accidentals to become the note E while maintaining 
    /// the same value. therefore, the simplest form of D## is E.
    /// 
    /// Eb and D# cannot remove any accidentals without changing their values, 
    /// therefore they are already in their simplest form. 
    /// 
    /// C and B have zero accidentals, therefore they are already in their 
    /// simplest form.
    public static NoteName GetSimplifiedName(int nameIndex, int accidentals)
    {
        if (accidentals == Numbers.Zero)
            return (nameIndex, accidentals);

        int offsetValue = RawScalePatterns.Diatonic[nameIndex] + accidentals;

        if (offsetValue < Numbers.Zero)
            offsetValue = Numbers.SemitonesInOctave + offsetValue;

        else if (offsetValue >= Numbers.SemitonesInOctave)
            offsetValue %= Numbers.SemitonesInOctave;

        return RawScalePatterns.Diatonic.Contains(offsetValue)
            ? (RawScalePatterns.Diatonic.IndexOf(offsetValue), Numbers.Zero)
            : accidentals > Numbers.Zero
                ? (RawScalePatterns.Diatonic.IndexOf(offsetValue + Numbers.FlatModifier), Numbers.SharpModifier)
                : (RawScalePatterns.Diatonic.IndexOf(offsetValue + Numbers.SharpModifier), Numbers.FlatModifier);
    }

    /// <summary>
    /// For any note for which two names exist, return the other note name with
    /// the same relative value (e.g. Bb > A#, Db > C#, C > B#, Cb > B, etc.)
    /// </summary>
    public static NoteName FlipBinomial(int pitchClass, int accidentals)
    {
        const int B_C = 6;
        const int E_F = 2;
        if (accidentals
            is > Numbers.SharpModifier
            or < Numbers.FlatModifier
        )
        {
            throw new InvalidOperationException($"{nameof(NomenclatureHelper)}.{nameof(FlipBinomial)}: Cannot process names with more than one accidental.");
        }
        // Naturals
        // A, D, G are returned unchanged
        // B, C, E, F are flipped to Cb, B#, etc. 
        if (accidentals == Numbers.Zero)
        {
            return pitchClass switch
            {
                B_C or E_F
                    => ((pitchClass + Numbers.SharpModifier) % Numbers.NotesInOctave, Numbers.FlatModifier),

                E_F + Numbers.SharpModifier or (B_C + Numbers.SharpModifier) % Numbers.NotesInOctave
                    => ((pitchClass + Numbers.NotesInOctave + Numbers.FlatModifier) % Numbers.NotesInOctave, Numbers.SharpModifier),
                _
                    => (pitchClass, accidentals)
            };
        }
        if (accidentals == Numbers.SharpModifier)
        {
            pitchClass = (pitchClass + Numbers.SharpModifier) % Numbers.NotesInOctave;
            if (pitchClass == (B_C + Numbers.IndexOffset) % Numbers.NotesInOctave || pitchClass == E_F + Numbers.IndexOffset)
                accidentals = Numbers.Zero;
            else
                accidentals = Numbers.FlatModifier;
        }
        else if (accidentals == Numbers.FlatModifier)
        {
            if (pitchClass == Numbers.Zero)
                pitchClass = Numbers.NotesInOctave + Numbers.FlatModifier;
            else
                pitchClass += Numbers.FlatModifier;

            if (pitchClass == B_C || pitchClass == E_F)
                accidentals = Numbers.Zero;
            else
                accidentals = Numbers.SharpModifier;
        }

        return (pitchClass, accidentals);

    }

    /// <summary>
    /// Given a note name and a heptatonic scale pattern, return the names 
    /// of all note names in the scale.
    /// </summary>
    public static List<NoteName> CalculateHeptatonicNames(NoteName keynote, IEnumerable<int> pattern)
    {
        pattern = [Numbers.Zero, .. pattern];
        List<int> intervals = [.. pattern.ToHashSet().Order()];
        List<NoteName> results = [];
        int keynoteAbsoluteValue = RawScalePatterns.Diatonic[keynote.PitchClass] + keynote.Accidental;
        int newNameIndex,
            tonesOffset,
            originalValue,
            relativeValue,
            accidentals;

        for (int i = Numbers.Zero; i < Numbers.NotesInOctave; i++)
        {
            newNameIndex = (i + keynote.PitchClass) % Numbers.NotesInOctave;
            tonesOffset = (int)Math.Floor((double)(i + keynote.PitchClass) / Numbers.NotesInOctave) * Numbers.SemitonesInOctave;
            originalValue = RawScalePatterns.Diatonic[newNameIndex];
            relativeValue = keynoteAbsoluteValue + intervals[i];
            accidentals = relativeValue - (originalValue + tonesOffset);
            results.Add((newNameIndex, accidentals));
        }
        return results;
    }

    /// <summary>
    /// Sort the given scale degree names.
    /// </summary>
    public static List<DegreeName> SortIntervalNames(IEnumerable<DegreeName> scaleDegrees)
    {
        var d = scaleDegrees.ToList();
        d.Sort((left, right) => GetIntervalSortValue(left).CompareTo(GetIntervalSortValue(right)));
        return d;
    }


    public static List<DegreeName> SortDegreeNames(IEnumerable<DegreeName> degreeNames)
    {
        List<DegreeName> result = [.. degreeNames];
        result.Sort(CompareDegreeNames);
        return result;
    }


    public static int CompareDegreeNames(DegreeName left, DegreeName right)
    {
        double lValue = left.Degree + (left.Accidental * 0.1);
        double rValue = right.Degree + (right.Accidental * 0.1);
        return lValue.CompareTo(rValue);
    }

    /// <summary>
    /// Get a string representing the quality of the interval with the given 
    /// ascending scale degree and value.
    /// </summary>
    /// <exception cref="IntervalQualityUnrecognized"></exception>
    public static string GetIntervalQuality(int degree, int value)
    {
        value = value % (Numbers.SemitonesInDoubleOctave + 1);
        // Ugly solution but it works. 
        string? quality = degree switch
        {
            1 => value switch
            {
                0 => Keywords.Perfect,
                _ => null
            },
            2 => value switch
            {
                1 => Keywords.Minor,
                2 => Keywords.Major,
                3 => Keywords.Augmented,
                4 => Keywords.Double + Symbols.Whitespace + Keywords.Augmented,
                _ => null
            },
            3 => value switch
            {
                2 => Keywords.Diminished,
                3 => Keywords.Minor,
                4 => Keywords.Major,
                5 => Keywords.Augmented,
                6 => Keywords.Double + Symbols.Whitespace + Keywords.Augmented,
                _ => null
            },
            4 => value switch
            {
                3 => Keywords.Double + Symbols.Whitespace + Keywords.Diminished,
                4 => Keywords.Diminished,
                5 => Keywords.Perfect,
                6 => Keywords.Augmented,
                7 => Keywords.Double + Symbols.Whitespace + Keywords.Augmented,
                _ => null
            },
            5 => value switch
            {
                5 => Keywords.Double + Symbols.Whitespace + Keywords.Diminished,
                6 => Keywords.Diminished,
                7 => Keywords.Perfect,
                8 => Keywords.Augmented,
                9 => Keywords.Double + Symbols.Whitespace + Keywords.Augmented,
                _ => null
            },
            6 => value switch
            {
                6 => Keywords.Double + Symbols.Whitespace + Keywords.Diminished,
                7 => Keywords.Diminished,
                8 => Keywords.Minor,
                9 => Keywords.Major,
                10 => Keywords.Augmented,
                _ => null
            },
            7 => value switch
            {
                9 => Keywords.Diminished,
                10 => Keywords.Minor,
                11 => Keywords.Major,
                _ => null
            },
            8 => value switch
            {
                Numbers.SemitonesInOctave + 0 => Keywords.Perfect,
                _ => null
            },
            9 => value switch
            {
                Numbers.SemitonesInOctave + 1 => Keywords.Minor,
                Numbers.SemitonesInOctave + 2 => Keywords.Major,
                Numbers.SemitonesInOctave + 3 => Keywords.Augmented,
                Numbers.SemitonesInOctave + 4 => Keywords.Double + Symbols.Whitespace + Keywords.Augmented,
                _ => null
            },
            10 => value switch
            {
                Numbers.SemitonesInOctave + 2 => Keywords.Diminished,
                Numbers.SemitonesInOctave + 3 => Keywords.Minor,
                Numbers.SemitonesInOctave + 4 => Keywords.Major,
                Numbers.SemitonesInOctave + 5 => Keywords.Augmented,
                Numbers.SemitonesInOctave + 6 => Keywords.Double + Symbols.Whitespace + Keywords.Augmented,
                _ => null
            },
            11 => value switch
            {
                Numbers.SemitonesInOctave + 3 => Keywords.Double + Symbols.Whitespace + Keywords.Diminished,
                Numbers.SemitonesInOctave + 4 => Keywords.Diminished,
                Numbers.SemitonesInOctave + 5 => Keywords.Perfect,
                Numbers.SemitonesInOctave + 6 => Keywords.Augmented,
                Numbers.SemitonesInOctave + 7 => Keywords.Double + Symbols.Whitespace + Keywords.Augmented,
                _ => null
            },
            12 => value switch
            {
                Numbers.SemitonesInOctave + 5 => Keywords.Double + Symbols.Whitespace + Keywords.Diminished,
                Numbers.SemitonesInOctave + 6 => Keywords.Diminished,
                Numbers.SemitonesInOctave + 7 => Keywords.Perfect,
                Numbers.SemitonesInOctave + 8 => Keywords.Augmented,
                Numbers.SemitonesInOctave + 9 => Keywords.Double + Symbols.Whitespace + Keywords.Augmented,
                _ => null
            },
            13 => value switch
            {
                Numbers.SemitonesInOctave + 6 => Keywords.Double + Symbols.Whitespace + Keywords.Diminished,
                Numbers.SemitonesInOctave + 7 => Keywords.Diminished,
                Numbers.SemitonesInOctave + 8 => Keywords.Minor,
                Numbers.SemitonesInOctave + 9 => Keywords.Major,
                Numbers.SemitonesInOctave + 10 => Keywords.Augmented,
                _ => null
            },
            14 => value switch
            {
                Numbers.SemitonesInOctave + 9 => Keywords.Diminished,
                Numbers.SemitonesInOctave + 10 => Keywords.Minor,
                Numbers.SemitonesInOctave + 11 => Keywords.Major,
                _ => null
            },
            _ => null
        };
        // TODO: This exception is meant to be unreachable, but we have not
        // permuted all possible intervals that can be derived from our set
        // of legal scale forms.
        //
        // So, next step is to decide which scales will be legal, then
        // permute all their chords through every voicing and inversion,
        // and see where the error pops up and add those cases to the
        // switch. 
        //
        // Most normal scales and chords will never generate intervals
        // that raise this exception, so it's not an enormous problem
        // for most music.
        return quality ?? throw new IntervalQualityUnrecognized(degree, value);
    }

    /// <summary>
    /// Return the name of the pitch that exists at the given <paramref name="frequency"/>, 
    /// describing it with sharps if <paramref name="useSharps"/> is true,
    /// otherwise with flats.
    public static ((int, int), int) NamePitchAtFrequency(double frequency, bool useSharps = true)
    {
        int octaves = (int)FrequencyHelper
            .GetOctavesBetweenFrequencies(
                Numbers.MinFrequency,
                frequency
            );
        int identity = FrequencyHelper
            .GetSemitonesBetwenFrequencies(
                Numbers.MinFrequency,
                frequency
            ) % Numbers.SemitonesInOctave;
        int nameIndex;
        int accidentals = Numbers.Zero;
        if (RawScalePatterns.Diatonic.Contains(identity))
        {
            nameIndex = RawScalePatterns.Diatonic.IndexOf(identity);
        }
        else if (useSharps)
        {
            nameIndex = RawScalePatterns.Diatonic.IndexOf(identity + Numbers.FlatModifier);
            accidentals = Numbers.SharpModifier;
        }
        else
        {
            nameIndex = RawScalePatterns.Diatonic.IndexOf(identity + Numbers.SharpModifier);
            accidentals = Numbers.FlatModifier;
        }

        return ((nameIndex, accidentals), octaves);
    }

    /// <summary>
    /// Return the reciprocal of the given interval, proportional to the 
    /// overall width of the interval itself.
    /// </summary>
    /// <remarks>
    /// Examples of reciprocal widths:
    /// 
    /// note        reciprocal          total width 
    /// ----        ----------          -----------
    /// major 3rd   -> minor 6th.       octave (major 3rd + minor 6th)
    /// major 10th  -> minor 13th       three octaves (major 10th + minor 13th)
    /// major 17th  -> minor 20th       five octaves (major 17th + minor 20th)
    /// </remarks>
    public static (int Degree, int Accidental) GetReciprocalInterval((int Degree, int Accidental) interval)
    {
        int value = GetIntervalValueFromDegreeName(interval);
        int reciprocalDegree = 9 - interval.Degree % Numbers.NotesInOctave
            + (interval.Degree / Numbers.NotesInOctave * Numbers.NotesInOctave);
        int reciprocalValue = Numbers.SemitonesInOctave - value % Numbers.SemitonesInOctave
            + (value / Numbers.SemitonesInOctave * Numbers.SemitonesInOctave);
        return GetIntervalDegreeNameFromExpectedValue(reciprocalDegree, reciprocalValue);
    }

    /// <summary>
    /// Return true if the given collection of interval names is or contains
    /// a dominant chord.
    /// </summary>
    public static bool IsDominant(IEnumerable<(int, int)> intervals)
        => intervals.ToHashSet().IsSupersetOf([(3, 0), (7, -1)]);

    /// <summary>
    /// Return true if the given collection of interval names is or contains
    /// a diminished chord.
    /// </summary>
    public static bool IsDiminished(IEnumerable<(int, int)> intervals)
        => intervals.ToHashSet().IsSupersetOf([(3, -1), (5, -1), (7, -2)]);

    /// <summary>
    /// Return true if the given collection of interval names is or contains
    /// a perfect fifth.
    /// </summary>
    public static bool HasPerfectFifth(IEnumerable<(int, int)> intervals)
        => intervals.Contains((5, 0));

    /// <summary>
    /// Return true if the given collection of interval names is or contains
    /// an altered fifth.
    /// </summary>
    public static bool HasAlteredFifth(IEnumerable<(int Degree, int Accidentals)> intervals)
        => intervals.Any(it => it.Degree == 5 && it.Accidentals != 0);


}
