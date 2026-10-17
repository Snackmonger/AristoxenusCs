using System.Text.RegularExpressions;
using Aristoxenus.Core.Bases;
using Aristoxenus.Core.Data;
using Aristoxenus.Core.Errors;
using Aristoxenus.Core.Interfaces;

namespace Aristoxenus.Core.Models;


/// <summary>
/// Representation of a chord name, which implies a generic interval 
/// structure over a given root note (but does not account for concrete
/// voicings).
/// </summary>
public class ChordName : ValueObject, ISymbolSerializable<ChordName>
{

    /// <summary>
    /// The note name that serves as the root of the chord.
    /// </summary>
    public readonly NoteName Root;

    /// <summary>
    /// The intervals that exist above the chord root.
    /// </summary>
    public readonly IntervalStructure Intervals;

    /// <summary>
    /// Flag that indicates whether the chord is diminished.
    /// </summary>
    public readonly bool IsDiminished ;
    /// <summary>
    /// Flag that indicates whether the chord is dominant.
    /// </summary>
    public readonly bool IsDominant;
    /// <summary>
    /// Flag that indicates whether the chord has a perfect fifth.
    /// </summary>
    public readonly bool HasPerfectFifth;
    /// <summary>
    /// Flag that indicates whether the chord has an altered fifth.
    /// </summary>
    public readonly bool HasAlteredFifth;

    public ChordName(NoteName root, IntervalStructure intervals)
    {
        Root = root;
        Intervals = intervals;
        IsDiminished = Intervals.IsDiminished;
        IsDominant = Intervals.IsDominant;
        HasPerfectFifth = Intervals.HasPerfectFifth;
        HasAlteredFifth = Intervals.HasAlteredFifth;
    }

    /// <summary>
    /// Get the note names that represent the intervals of the chord, relative
    /// to the root note. 
    /// NB: The note names do not distinguish octaves, so, e.g. 4 and 11 are 
    /// the same (but their position is determined by the interval structure).
    /// </summary>
    /// <returns></returns>
    public List<NoteName> GetNoteNames()
        => [.. Intervals.Select(Root.NoteNameAbove)];

    public Chord Reify(ChordConfiguration configuration, int octave)
    {
        throw new NotImplementedException();
    }


    public string ToSymbol()
        => Root.ToSymbol() + Intervals.ToChordSymbol();

    public static ChordName FromSymbol(string chordSymbol)
    {
        Match match = RegularExpressions.ChordSymbol.Match(chordSymbol);
        if (!match.Success)
        {
            throw new InvalidAsSymbol(chordSymbol, nameof(chordSymbol));
        }
        string root = match.Groups.GetValueOrDefault(Keywords.RootName)!.Value;
        return new(NoteName.FromSymbol(root), IntervalStructure.FromChordSymbol(chordSymbol));
    }

    string ISymbolSerializable<ChordName>.ToSymbol()
        => ToSymbol();

    static ChordName ISymbolSerializable<ChordName>.FromSymbol(string symbol)
        => FromSymbol(symbol);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ToSymbol();
    }

    public static implicit operator string(ChordName chordName)
    => chordName.ToSymbol();

    public static implicit operator ChordName(string chordName)
        => FromSymbol(chordName);
}
