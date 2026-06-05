using System.Collections;
using System.Collections.Immutable;

using Aristoxenus.Core.Domains.Theory.Atoms;
using Aristoxenus.Core.Domains.Theory.Errors;

using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Interfaces;
using Aristoxenus.Core.Shared.Utilities;

namespace Aristoxenus.Core.Domains.Theory.Harmony;

/// <summary>
/// A structure made up of intervals, which are expressed relative to the 0th
/// interval (unison), which will always be implicitly added to any interval
/// structure object if it is not explicitly added.
/// </summary>
public class IntervalStructure : ValueObject, IReadOnlySet<Interval>, ISymbolSerializable<IntervalStructure>
{
    public readonly ImmutableHashSet<Interval> Intervals;

    public List<ImmutableHashSet<Interval>> Inversions { get; init; }
    public List<int> Values { get; init; }
    public List<string> IntervalSymbols { get; init; }
    public Interval? Third { get; init; }
    public Interval? AlteredFifth { get; init; }
    public List<Interval> Suspensions { get; init; }
    public bool HasPerfectFifth { get; init; }
    public bool HasAlteredFifth { get; init; }
    public bool IsDominant { get; init; }
    public bool IsDiminished { get; init; }
    public bool IsTriad { get; init; }
    public bool IsTetrad { get; init; }
    public bool IsHeptatonic { get; init; }

    public static IntervalStructure Diatonic { get; } = CreateAsHeptatonic(ScalePattern.Diatonic);
    public static IntervalStructure Altered { get; } = CreateAsHeptatonic(ScalePattern.Altered);

    public IntervalStructure(IEnumerable<Interval> intervals)
    {
        List<Interval> sanitized = [];

        foreach (Interval interval in intervals)
        {
            if (sanitized.Contains(interval) || sanitized.Select(i => i.Value).Contains(interval.Value))
                throw new IntervalStructureHasDuplicates(intervals.Select(i => i.Name.ToSymbol()));
            sanitized.Add(interval);
        }
        Intervals = [.. sanitized];
        Inversions = [];
        Values = [.. Intervals.Select(x => x.Value).Order()];
        IntervalSymbols = [.. NomenclatureHelper.SortIntervalNames(Intervals.Select(x => x.Name.ToSymbol()))];
        Suspensions = [];

        IsDominant = IsSupersetOf([Symbols.Three, Symbols.FlatSeven]);
        IsDiminished = IsSupersetOf([Symbols.FlatThree, Symbols.FlatFive, Symbols.DoubleFlatSeven]);
        IsTriad = Count == 3;
        IsTetrad = Count == 4;
        IsHeptatonic = Count == Numbers.NotesInOctave;
        HasPerfectFifth = false;
        HasAlteredFifth = false;
        foreach (Interval interval in Intervals)
        {
            if (interval.Degree == 3 && interval.Accidental is 0 or -1)
                Third = interval;
            if (interval.Degree == 5 && interval.Accidental == 0)
                HasPerfectFifth = true;
            if ((interval.Degree == 3 && interval.Accidental is -2 or 1)
                || interval.Degree == 4
                || interval.Degree == 2)
                Suspensions.Add(interval);
            if (interval.Degree == 5 && interval.Accidental != 0)
                AlteredFifth = interval;
        }
        if (AlteredFifth != null)
            HasAlteredFifth = true;
    }

    /// <summary>
    /// Create a new instance from an enumerable of Intervals.
    /// </summary>
    /// <exception cref="ArgumentException">If the structure contains 
    /// a duplicate or overlapping interval.</exception>
    public static IntervalStructure CreateNew(IEnumerable<Interval> intervals)
    {
        List<Interval> sanitized = [];

        foreach (Interval interval in intervals)
        {
            if (sanitized.Contains(interval) || sanitized.Select(i => i.Value).Contains(interval.Value))
                throw new IntervalStructureHasDuplicates(intervals.Select(i => i.Name.ToSymbol()));
            sanitized.Add(interval);
        }
        return new IntervalStructure(sanitized);
    }

    /// <summary>
    /// Create a new interval structure from a collection of interval symbols.
    /// </summary>
    /// <param name="intervalSymbols"></param>
    /// <returns></returns>
    public static IntervalStructure FromIntervalSymbols(IEnumerable<string> intervalSymbols)
        => CreateNew(intervalSymbols.Select(n => Interval.FromSymbol(n)));


    /// <summary>
    /// Create an interval structure on the assumption that the given 
    /// integers resolve to a heptatonic scale. That is: there must be
    /// exactly seven unique factors (including a unison = 0).
    /// </summary>
    /// <param name="intervals">The integers that wil be considered as intervals.</param>
    /// <exception cref="ArgumentException">If the structure is not a heptatonic octave.</exception>
    public static IntervalStructure CreateAsHeptatonic(IEnumerable<int> intervals)
    {
        HashSet<int> unique = [Numbers.Zero, .. intervals.Select(x => x % Numbers.SemitonesInOctave)];
        if (unique.Count != Numbers.NotesInOctave)
        {
            throw new DomainException("Intervals must represent a heptatonic structure (have 7 unique intervals, including the unison).");
        }
        intervals = intervals.Order();
        List<Interval> sanitizedIntervals = [
            ..intervals.Select(
                (value, index) => Interval.FromExpectedValue(index + Numbers.IndexOffset, value)
                )
            ];
        return CreateNew(sanitizedIntervals);
    }

    // IReadOnlySet<Interval> /////////////////////////////////////////////////
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    public IEnumerator<Interval> GetEnumerator()
        => Intervals.GetEnumerator();

    public int Count
        => Intervals.Count();

    public bool Contains(Interval interval)
        => Intervals.Contains(interval);

    public bool IsProperSubsetOf(IEnumerable<Interval> intervals)
        => IsSubsetOf(intervals) && Count < intervals.Count();

    public bool IsProperSupersetOf(IEnumerable<Interval> intervals)
        => IsSupersetOf(intervals) && Count > intervals.Count();

    public bool IsSubsetOf(IEnumerable<Interval> intervals)
        => Intervals.IsSubsetOf(intervals);

    public bool IsSupersetOf(IEnumerable<Interval> intervals)
        => Intervals.IsSupersetOf(intervals);

    public bool Overlaps(IEnumerable<Interval> intervals)
        => Intervals.Overlaps(intervals);

    public bool SetEquals(IEnumerable<Interval> intervals)
        => Intervals.SetEquals(intervals);

    // As if from IReadOnlySet<string>...

    public bool Contains(string interval)
        => Intervals.Select(i => i.Name.ToSymbol()).Contains(interval);

    public bool IsSubsetOf(IEnumerable<string> intervals)
        => IntervalSymbols.ToHashSet().IsSubsetOf(intervals);

    public bool IsSupersetOf(IEnumerable<string> intervals)
        => IntervalSymbols.ToHashSet().IsSupersetOf(intervals);

    public bool IsProperSubsetOf(IEnumerable<string> intervals)
        => IntervalSymbols.ToHashSet().IsProperSubsetOf(intervals);

    public bool IsProperSupersetOf(IEnumerable<string> intervals)
        => IntervalSymbols.ToHashSet().IsProperSupersetOf(intervals);

    public bool Overlaps(IEnumerable<string> intervals)
        => IntervalSymbols.ToHashSet().Overlaps(intervals);

    public bool SetEquals(IEnumerable<string> intervals)
        => IntervalSymbols.ToHashSet().SetEquals(intervals);

    // Set-like behaviors /////////////////////////////////////////////////////

    /// <summary>
    /// Check whether this interval structure is a superset of the given 
    /// interval structure from the perspective of each inversion/mode
    /// of this structure (e.g. [1, 2, 3, 4, 5, 6, 7] is a modal superset
    /// of [1, b3, 5] because the 2, 3, and 6 modes contain this triad in
    /// tonic position)
    /// </summary>
    public bool IsModalSupersetOf(IEnumerable<Interval> intervals)
    {
        foreach (var mode in Inversions)
        {
            if (mode.IsSupersetOf(intervals))
                return true;
        }
        return false;
    }


    // Chord Symbol Api ///////////////////////////////////////////////////////

    /// <summary>
    /// Translate this interval structure into a chord symbol string. Mostly 
    /// used for chords, but this can be a shorthand for scale names as well.
    /// </summary>
    public string ToChordSymbol()
        => NomenclatureHelper.GetChordSymbolFromIntervals(
                Intervals.Select(i => i.Name.RawValues), 
                style: null
            );
    

    /// <summary>
    /// Parse a chord symbol into its implied intervals.
    /// </summary>
    /// <param name="chordSymbol">A chord symbol, with or without a 
    /// root symbol (i.e. e.g. either "Cmaj7#11" or "maj7#11").</param>
    public static IntervalStructure FromChordSymbol(string chordSymbol)
    {
        var intervals = NomenclatureHelper.GetIntervalsFromChordSymbol(chordSymbol);
        return FromIntervalSymbols(intervals);
    }

    // ISymbolSerializable ////////////////////////////////////////////////////

    public string ToSymbol() => ToChordSymbol();

    public static IntervalStructure FromSymbol(string symbol) => FromChordSymbol(symbol);

    // ValueObject ////////////////////////////////////////////////////////////

    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (string symbol in IntervalSymbols)
        {
            yield return symbol;
        }
    }

    public Interval this[int index] => throw new NotImplementedException();
}
