namespace Aristoxenus.Domain.Models;

using Aristoxenus.Constants;
using Aristoxenus.Domain.Exceptions;
using Aristoxenus.SharedKernel.Bases;
using Aristoxenus.SharedKernel.Interfaces;
using System.Text.RegularExpressions;


/// <summary>
/// Representation of a chord name, which implies a generic interval 
/// structure over a given root note (but does not account for concrete
/// voicings).
/// </summary>
public class ChordName : ValueObject, ISymbolSerializable<ChordName>
{
    public readonly NoteName Root;
    public readonly IntervalStructure Intervals;

    /// <summary>
    /// Flag that indicates whether the chord is diminished.
    /// </summary>
    public bool IsDiminished => Intervals.IsDiminished;
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

    private ChordName(NoteName root, IntervalStructure intervals)
    {
        Root = root;
        Intervals = intervals;
    }

    public static ChordName CreateNew(NoteName noteName, IntervalStructure intervals) 
        => new(noteName, intervals);

    public static ChordName CreateNew(string rootNoteSymbol, IntervalStructure intervals) 
        => CreateNew(NoteName.FromSymbol(rootNoteSymbol), intervals);

    public string Symbol 
        => Root.Symbol + Intervals.ToChordSymbol();

    public static ChordName FromSymbol(string chordSymbol)
    {
        Match match = RegularExpressions.ChordSymbol.Match(chordSymbol);
        if (!match.Success)
        {
            throw new InvalidAsSymbol(chordSymbol, nameof(chordSymbol));
        }
        string root = match.Groups.GetValueOrDefault(Keywords.RootName)!.Value;
        return CreateNew(NoteName.FromSymbol(root), IntervalStructure.FromChordSymbol(chordSymbol));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Symbol;
    }

}
