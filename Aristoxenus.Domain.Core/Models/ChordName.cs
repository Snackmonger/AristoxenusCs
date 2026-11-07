using System.Text.RegularExpressions;

using Aristoxenus.Domain.Core.Bases;
using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Errors;
using Aristoxenus.Domain.Core.Interfaces;

namespace Aristoxenus.Domain.Core.Models;


/// <summary>
/// Representation of a chord name, which implies a generic interval 
/// structure over a given root note (but does not account for concrete
/// voicings).
/// </summary>
public class ChordName(NoteName root, IntervalStructure intervals) : ValueObject, ISymbolSerializable<ChordName>
{

    /// <summary>
    /// The note name that serves as the root of the chord.
    /// </summary>
    public readonly NoteName Root = root;

    /// <summary>
    /// The intervals that exist above the chord root.
    /// </summary>
    public readonly IntervalStructure Intervals = intervals;

    /// <summary>
    /// Flag that indicates whether the chord is diminished.
    /// </summary>
    public  bool IsDiminished => Intervals.IsDiminished;

    /// <summary>
    /// Flag that indicates whether the chord is dominant.
    /// </summary>
    public bool IsDominant => Intervals.IsDominant;

    /// <summary>
    /// Flag that indicates whether the chord has a perfect fifth.
    /// </summary>
    public bool HasPerfectFifth => Intervals.HasPerfectFifth;

    /// <summary>
    /// Flag that indicates whether the chord has an altered fifth.
    /// </summary>
    public bool HasAlteredFifth => Intervals.HasAlteredFifth;

    /// <summary>
    /// Get the note names that represent the intervals of the chord, relative
    /// to the root note. 
    /// NB: The note names do not distinguish octaves, so, e.g. 4 and 11 are 
    /// the same (but their position is determined by the interval structure).
    /// </summary>
    public List<NoteName> GetNoteNames()
        => [.. Intervals.Select(Root.NoteNameAbove)];

    public Chord Reify(ChordVoicing voicing, int inversion, int octave)
        => new(this, voicing, inversion, octave);

    public Chord Reify(int inversion, int octave)
    => new(this, ChordVoicing.RootPosition, inversion, octave);

    public Chord Reify(int octave)
    => new(this, ChordVoicing.RootPosition, 0, octave);

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
