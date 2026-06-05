using System.Text.RegularExpressions;

namespace Aristoxenus.Core.Shared.Data;


/// <summary>
/// Regular expressions used to parse and validate text.
/// </summary>
public partial class RegularExpressions
{
    public static readonly Regex NoteName = NoteNameRegex();
    public static readonly Regex IntervalSymbol = IntervalSymbolRegex();
    public static readonly Regex SharpSymbol = SharpSymbolRegex();
    public static readonly Regex FlatSymbol = FlatSymbolRegex();
    public static readonly Regex PitchSymbol = PitchSymbolRegex();
    public static readonly Regex ChordSymbol = ChordSymbolRegex();
    public static readonly Regex RootlessChordSymbol = RootlessChordSymbolRegex();

    [GeneratedRegex(RegExString.NoteName)]
    private static partial Regex NoteNameRegex();

    [GeneratedRegex($@"{RegExString.NoteName}([0-8])")]
    private static partial Regex PitchSymbolRegex();

    [GeneratedRegex(RegExString.IntervalSymbol)]
    private static partial Regex IntervalSymbolRegex();

    [GeneratedRegex(Symbols.Sharp)]
    private static partial Regex SharpSymbolRegex();

    [GeneratedRegex(Symbols.Flat)]
    private static partial Regex FlatSymbolRegex();

    [GeneratedRegex($@"^{RegExString.ChordRootName}{RegExString.ChordMainSymbol}?{RegExString.ChordExtension}?{RegExString.ChordModification}?$")]
    private static partial Regex ChordSymbolRegex();

    [GeneratedRegex($@"^{RegExString.ChordMainSymbol}?{RegExString.ChordExtension}?{RegExString.ChordModification}?$")]
    private static partial Regex RootlessChordSymbolRegex();
}
