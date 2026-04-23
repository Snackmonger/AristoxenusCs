namespace Aristoxenus.Core.Utilities;

using Aristoxenus.Core.Constants;
using Aristoxenus.Core.Domain.Theory;

internal static class NomenclatureHelper
{
    internal static Dictionary<string, double> StringSortValues = new() {
        { "1", 0 },
        { "b2", 1 },
        { "2", 2 },
        { "bb3", 3 },
        { "#2", 4 },
        { "b3", 5 },
        { "3", 6 },
        { "b4", 7 },
        { "#3", 8 },
        { "4", 9},
        { "##3", 9.5},
        { "bb5", 10},
        { "#4", 11},
        { "b5", 12 },
        { "5", 13 },
        { "#5", 14 },
        { "b6", 15 },
        { "##5", 16 },
        { "6", 17 },
        { "bb7", 18 },
        { "#6", 19},
        { "b7", 20 },
        { "7", 21 },
        { "8", 21.5 },
        { "b9", 22 },
        { "9", 23 },
        { "bb10", 23.1 },
        { "#9", 24 },
        { "b10", 24.1 },
        { "10", 24.5 },
        { "#10", 24.6 },
        { "b11", 24.7 },
        { "##10", 24.8 },
        { "11", 25 },
        { "bb12", 25.1},
        { "#11", 26},
        { "b12", 26.1 },
        { "12", 26.2 },
        { "#12", 26.3 },
        { "b13", 27},
        { "##12", 27.1 },
        { "13", 28 },
        { "bb14", 29 },
        { "#13", 30},
        { "b14", 31 },
        { "14", 32 },
        { "15", 33 }
    };
    public static (int nameIndex, int accidentals) GetSimplifiedName(int nameIndex, int accidentals)
    {
        if (accidentals == 0)
            return (nameIndex, accidentals);

        int offsetValue = (ScalePatternsHeptatonic.Diatonic[nameIndex]
                + accidentals)
                % Numbers.SemitonesInOctave;

        return ScalePatternsHeptatonic.Diatonic.Contains(offsetValue)
            ? (ScalePatternsHeptatonic.Diatonic.IndexOf(offsetValue) + Numbers.IndexOffset, Numbers.Zero)
            : accidentals > 0
                ? (ScalePatternsHeptatonic.Diatonic.IndexOf(offsetValue + Numbers.FlatModifier) + Numbers.IndexOffset, Numbers.SharpModifier)
                : (ScalePatternsHeptatonic.Diatonic.IndexOf(offsetValue + Numbers.SharpModifier) + Numbers.IndexOffset, Numbers.FlatModifier);

    }

    public static List<string> SortIntervalNames(IEnumerable<string> intervals)
    {
        var i = intervals.ToList();
        i.Sort((x, y) => StringSortValues[x].CompareTo(StringSortValues[y]));
        return [.. i];
    }

    public static string GetQuality(int degree, int value)
    {
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
                12 + 0 => Keywords.Perfect,
                _ => null
            },
            9 => value switch
            {
                12 + 1 => Keywords.Minor,
                12 + 2 => Keywords.Major,
                12 + 3 => Keywords.Augmented,
                12 + 4 => Keywords.Double + Symbols.Whitespace + Keywords.Augmented,
                _ => null
            },
            10 => value switch
            {
                12 + 2 => Keywords.Diminished,
                12 + 3 => Keywords.Minor,
                12 + 4 => Keywords.Major,
                12 + 5 => Keywords.Augmented,
                12 + 6 => Keywords.Double + Symbols.Whitespace + Keywords.Augmented,
                _ => null
            },
            11 => value switch
            {
                12 + 3 => Keywords.Double + Symbols.Whitespace + Keywords.Diminished,
                12 + 4 => Keywords.Diminished,
                12 + 5 => Keywords.Perfect,
                12 + 6 => Keywords.Augmented,
                12 + 7 => Keywords.Double + Symbols.Whitespace + Keywords.Augmented,
                _ => null
            },
            12 => value switch
            {
                12 + 5 => Keywords.Double + Symbols.Whitespace + Keywords.Diminished,
                12 + 6 => Keywords.Diminished,
                12 + 7 => Keywords.Perfect,
                12 + 8 => Keywords.Augmented,
                12 + 9 => Keywords.Double + Symbols.Whitespace + Keywords.Augmented,
                _ => null
            },
            13 => value switch
            {
                12 + 6 => Keywords.Double + Symbols.Whitespace + Keywords.Diminished,
                12 + 7 => Keywords.Diminished,
                12 + 8 => Keywords.Minor,
                12 + 9 => Keywords.Major,
                12 + 10 => Keywords.Augmented,
                _ => null
            },
            14 => value switch
            {
                12 + 9 => Keywords.Diminished,
                12 + 10 => Keywords.Minor,
                12 + 11 => Keywords.Major,
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
            $"Unrecognized interval quality: degree={degree}, value={value}");
    }

}
