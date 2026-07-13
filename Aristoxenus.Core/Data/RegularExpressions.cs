using System.Text.RegularExpressions;

namespace Aristoxenus.Core.Data;

/// <summary>
/// Regular expressions used to parse and validate text.
/// </summary>
public partial class RegularExpressions
{
    public static readonly Regex NoteName = NoteNameRegex();
    public static readonly Regex ScaleDegreeSymbol = ScaleDegreeRegex();
    public static readonly Regex SharpSymbol = SharpSymbolRegex();
    public static readonly Regex FlatSymbol = FlatSymbolRegex();
    public static readonly Regex PitchSymbol = PitchSymbolRegex();
    public static readonly Regex ChordSymbol = ChordSymbolRegex();
    public static readonly Regex RootlessChordSymbol = RootlessChordSymbolRegex();

    [GeneratedRegex(RawRegExStrings.NoteName)]
    private static partial Regex NoteNameRegex();

    [GeneratedRegex($@"{RawRegExStrings.NoteName}([0-8])")]
    private static partial Regex PitchSymbolRegex();

    [GeneratedRegex(RawRegExStrings.IntervalSymbol)]
    private static partial Regex ScaleDegreeRegex();

    [GeneratedRegex(Symbols.Sharp)]
    private static partial Regex SharpSymbolRegex();

    [GeneratedRegex(Symbols.Flat)]
    private static partial Regex FlatSymbolRegex();

    [GeneratedRegex($@"^{RawRegExStrings.ChordRootName}{RawRegExStrings.ChordMainSymbol}?{RawRegExStrings.ChordExtension}?{RawRegExStrings.ChordModification}?$")]
    private static partial Regex ChordSymbolRegex();

    [GeneratedRegex($@"^{RawRegExStrings.ChordMainSymbol}?{RawRegExStrings.ChordExtension}?{RawRegExStrings.ChordModification}?$")]
    private static partial Regex RootlessChordSymbolRegex();
}
