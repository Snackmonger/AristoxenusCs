namespace Aristoxenus.Core.Domain.Theory;

using Aristoxenus.Core.Constants;
using Aristoxenus.Core.SharedKernel.Bases;
using Aristoxenus.Core.SharedKernel.Interfaces;
using Aristoxenus.Core.Domain.Exceptions;
using System.Text.RegularExpressions;


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

    // TODO: Finalize the interval structure API and add any other useful methods here.

    // Constructors, Factories ////////////////////////////////////////////////
    private ChordName(NoteName root, IntervalStructure intervals)
    {
        Root = root;
        Intervals = intervals;
        IsDiminished = Intervals.IsDiminished;
        IsDominant = Intervals.IsDominant;
        HasPerfectFifth = Intervals.HasPerfectFifth;
        HasAlteredFifth = Intervals.HasAlteredFifth;
    }

    public static ChordName CreateNew(NoteName noteName, IntervalStructure intervals)
        => new(noteName, intervals);

    public static ChordName CreateNew(string rootNoteSymbol, IntervalStructure intervals)
        => CreateNew(NoteName.FromSymbol(rootNoteSymbol), intervals);


    // ISymbolSerializable ////////////////////////////////////////////////////
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
        return CreateNew(NoteName.FromSymbol(root), IntervalStructure.FromChordSymbol(chordSymbol));
    }

    string ISymbolSerializable<ChordName>.ToSymbol()
        => ToSymbol();

    static ChordName ISymbolSerializable<ChordName>.FromSymbol(string symbol)
        => FromSymbol(symbol);

    // ValueObject ////////////////////////////////////////////////////////////
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ToSymbol();
    }
 
    // Casts //////////////////////////////////////////////////////////////////

    public static implicit operator string(ChordName chordName)
    => chordName.ToSymbol();

    public static implicit operator ChordName(string chordName)
        => FromSymbol(chordName);
}
