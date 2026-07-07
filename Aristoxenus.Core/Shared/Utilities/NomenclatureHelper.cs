using Libraries.Arithmology;

using Aristoxenus.Core.Shared.Data;

namespace Aristoxenus.Core.Shared.Utilities;

public static class NomenclatureHelper
{
    // 05-June-2026: Revised logic of ordering. Shouldn't affect anything
    // except very weird scales. Rules are:
    // - Any natural gets its integer.
    // - Sharps get 0.1 added to the natural.
    // - Flats get 0.1 removed from the natural (surface form is the
    // **previous** natural plus a decimal).
    private static Dictionary<string, double> _intervalSymbolSortTable = new() {
        { "1", 1 },
        { "b2", 1.9 },
        { "2", 2 },
        { "#2", 2.1 },
        { "bb3", 2.8 },
        { "b3", 2.9 },
        { "3", 3 },
        { "#3", 3.1 },
        { "##3", 3.2},
        { "b4", 3.9 },
        { "4", 4 },
        { "#4", 4.1},
        { "bb5", 4.8},
        { "b5", 4.9 },
        { "5", 5 },
        { "#5", 5.1 },
        { "##5", 5.2 },
        { "b6", 5.9 },
        { "6", 6 },
        { "#6", 6.1 },
        { "bb7", 6.8 },
        { "b7", 6.9 },
        { "7", 7 },
        { "8", 8 },
        { "b9", 8.9 },
        { "9", 9 },
        { "#9", 9.1 },
        { "bb10", 9.8 },
        { "b10", 9.9 },
        { "10", 10 },
        { "#10", 10.1 },
        { "##10", 10.2 },
        { "b11", 10.9 },
        { "11", 11 },
        { "#11", 11.1 },
        { "bb12", 11.8 },
        { "b12", 11.9 },
        { "12", 12 },
        { "#12", 12.1 },
        { "##12", 12.2 },
        { "b13", 12.9 },
        { "13", 13 },
        { "#13", 13.1 },
        { "bb14", 13.8 },
        { "b14", 13.9 },
        { "14", 14 },
        { "15", 15 }
    };

    /// <summary>
    /// Create a new interval name tuple with the degree named as given, plus 
    /// as many accidentals as necessary to provide the expected value.
    /// </summary>
    /// <param name="expectedDegree">The degree you want the interval to be (e.g. 5)</param>
    /// <param name="expectedValue">The value you want the interval to have (e.g. 6)</param>
    /// <remarks>Note that any value greater than two octaves will be reduced
    /// to fit within two octaves.</remarks>
    /// <returns>An interval name with the desired degree and value (e.g. b5 == (5, 6))</returns>
    public static (int, int) GetIntervalNameFromExpectedValue(int expectedDegree, int expectedValue)
    {
        expectedValue = Math.Abs(expectedValue);
        expectedDegree = ValidationHelper.NormalizeDegree(expectedDegree);
        if (expectedValue > (Numbers.SemitonesInDoubleOctave))
            expectedValue = (expectedValue % Numbers.SemitonesInOctave) + Numbers.SemitonesInOctave;
        int index = expectedDegree - Numbers.IndexOffset;
        List<int> doubleOctave = PatternHelper.GetDoubleOctave(RawScalePatterns.Diatonic);
        int normalValue = doubleOctave[index];
        int accidental = expectedValue - normalValue;
        return (expectedDegree, accidental);
    }

    /// <summary>
    /// Get the absolute value of the interval from its name. 
    /// NB: Interval names cannot be recovered from absolute values, since 
    /// multiple names can have the same value (e.g. b5 and #4 have a value 
    /// of 6).
    /// </summary>
    public static int GetIntervalValueFromName((int degree, int accidentals) intervalName)
    {
        List<int> doubleOctave = PatternHelper.GetDoubleOctave(RawScalePatterns.Diatonic);
        return doubleOctave[intervalName.degree - Numbers.IndexOffset] + intervalName.accidentals;
    }

    /// <summary>
    /// Calculate the name of the interval that exists between a lower note 
    /// name and a higher note name.
    /// NB: This calculation is limted to one octave.
    /// </summary>
    public static (int degree, int accidentals) GetIntervalNameBetweenNoteNames((int nameIndex, int accidentals) lowerNote, (int nameIndex, int accidentals) higherNote)
    {
        var diatonicScaleFromLowerNote = CalculateHeptatonicNames(lowerNote, RawScalePatterns.Diatonic);
        int degree = diatonicScaleFromLowerNote.FindIndex(it => it.nameIndex == higherNote.nameIndex) + Numbers.IndexOffset;
        (int _, int accidentals) diatonicInterval = diatonicScaleFromLowerNote[degree - Numbers.IndexOffset];
        int expectedValue = RawScalePatterns.Diatonic[degree - Numbers.IndexOffset] + (higherNote.accidentals - diatonicInterval.accidentals);
        (degree, int accidentals) = GetIntervalNameFromExpectedValue(degree, expectedValue);

        return (degree, accidentals);
    }

    public static string GetAccidentalName(int Value)
    {
        return Value switch
        {
            0 => string.Empty,
            1 => Keywords.Sharp,
            -1 => Keywords.Flat,
            > 0 => string.Join(Symbols.Whitespace, Arithmology.Uple(Value), Keywords.Sharp),
            _ => string.Join(Symbols.Whitespace, Arithmology.Uple(Math.Abs(Value)), Keywords.Flat)
        };
    }

    public static (int, int) GetNoteNameAbove((int PitchClass, int Accidental) noteName, (int Degree, int Accidental) interval)
    {
        int intervalValue = RawScalePatterns
            .Diatonic[interval.Degree - Numbers.IndexOffset] + interval.Accidental;

        List<(int alphabeticIndex, int accidentals)> diatonicScaleFromThisTonic
            = CalculateHeptatonicNames(noteName, RawScalePatterns.Diatonic);

        int diatonicValueAtIntervalDegree = RawScalePatterns
            .Diatonic[(interval.Degree - Numbers.IndexOffset) % Numbers.NotesInOctave];

        (int alphabeticIndex, int accidentals) newName
            = diatonicScaleFromThisTonic[interval.Degree - Numbers.IndexOffset];

        if (diatonicValueAtIntervalDegree != intervalValue)
        {
            if (diatonicValueAtIntervalDegree > intervalValue)
            {
                newName.accidentals -= diatonicValueAtIntervalDegree - intervalValue;
            }
            if (diatonicValueAtIntervalDegree < intervalValue)
            {
                newName.accidentals += intervalValue - diatonicValueAtIntervalDegree;
            }
        }
        newName.accidentals %= Numbers.SemitonesInOctave;

        return (newName.alphabeticIndex, newName.accidentals);
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
    /// </remarks>
    /// <param name="nameIndex">The name's index in the string "CDEFGAB".</param>
    /// <param name="accidentals">A positive or negative integer representing
    /// the number of sharps or flats.</param>
    /// <returns></returns>
    public static (int nameIndex, int accidentals) GetSimplifiedName(int nameIndex, int accidentals)
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
    public static (int nameIndex, int accidentals) FlipBinomial(int nameIndex, int accidentals)
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
        // B, C, E, F are flipped to Cb, B#, etc. in 
        if (accidentals == Numbers.Zero)
        {
            return nameIndex switch
            {
                B_C or
                E_F =>
                (
                    nameIndex: (nameIndex + Numbers.SharpModifier) % Numbers.NotesInOctave,
                    accidentals: Numbers.FlatModifier
                ),
                E_F + Numbers.SharpModifier or
                (B_C + Numbers.SharpModifier) % Numbers.NotesInOctave =>
                (
                        nameIndex: (nameIndex + Numbers.NotesInOctave + Numbers.FlatModifier) % Numbers.NotesInOctave,
                        accidentals: Numbers.SharpModifier
                ),
                _ => (nameIndex, accidentals)
            };
        }
        if (accidentals == Numbers.SharpModifier)
        {
            nameIndex = (nameIndex + Numbers.SharpModifier) % Numbers.NotesInOctave;
            accidentals = Numbers.FlatModifier;
        }
        else if (accidentals == Numbers.FlatModifier)
        {
            if (nameIndex == Numbers.Zero)
                nameIndex = Numbers.NotesInOctave + Numbers.FlatModifier;
            else
                nameIndex -= Numbers.SharpModifier;

            accidentals = Numbers.SharpModifier;
        }

        return (nameIndex, accidentals);

    }

    /// <summary>
    /// Given a note name and a heptatonic scale pattern, return the names 
    /// of all note names in the scale.
    /// </summary>
    public static List<(int nameIndex, int accidentals)> CalculateHeptatonicNames((int nameIndex, int accidentals) keynote, IEnumerable<int> pattern)
    {
        pattern = [Numbers.Zero, .. pattern];
        List<int> intervals = [.. pattern.ToHashSet().Order()];
        List<(int, int)> results = [];
        int keynoteAbsoluteValue = RawScalePatterns.Diatonic[keynote.nameIndex] + keynote.accidentals;
        int newNameIndex,
            tonesOffset,
            originalValue,
            relativeValue,
            accidentals;

        for (int i = Numbers.Zero; i < Numbers.NotesInOctave; i++)
        {
            newNameIndex = (i + keynote.nameIndex) % Numbers.NotesInOctave;
            tonesOffset = (int)Math.Floor((double)(i + keynote.nameIndex) / Numbers.NotesInOctave) * Numbers.SemitonesInOctave;
            originalValue = RawScalePatterns.Diatonic[newNameIndex];
            relativeValue = keynoteAbsoluteValue + intervals[i];
            accidentals = relativeValue - (originalValue + tonesOffset);
            results.Add((newNameIndex, accidentals));
        }
        return results;
    }

    public static List<string> SortIntervalNames(IEnumerable<string> intervals)
    {
        var i = intervals.ToList();
        i.Sort((x, y) => _intervalSymbolSortTable[x].CompareTo(_intervalSymbolSortTable[y]));
        return [.. i];
    }

    public static string GetIntervalQuality(int degree, int value)
    {
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
        return quality ?? throw new InvalidOperationException(
            $"{nameof(NomenclatureHelper)}.{nameof(GetIntervalQuality)}: Unrecognized interval quality: degree={degree}, value={value}");
    }

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

    public static bool IsDominant(IEnumerable<(int, int)> intervals)
        => intervals.ToHashSet().IsSupersetOf([(3, 0), (7, -1)]);
    public static bool IsDiminished(IEnumerable<(int, int)> intervals)
        => intervals.ToHashSet().IsSupersetOf([(3, -1), (5, -1), (7, -2)]);
    public static bool HasPerfectFifth(IEnumerable<(int, int)> intervals)
        => intervals.Contains((5, 0));
    public static bool HasAlteredFifth(IEnumerable<(int Degree, int Accidentals)> intervals)
        => intervals.Any(it => it.Degree == 5 && it.Accidentals != 0);


}
