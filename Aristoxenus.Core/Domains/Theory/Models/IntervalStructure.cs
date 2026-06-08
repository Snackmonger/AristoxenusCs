using System.Collections;
using System.Collections.Immutable;
using Aristoxenus.Core.Domains.Theory.Errors;
using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Interfaces;
using Aristoxenus.Core.Shared.Utilities;

namespace Aristoxenus.Core.Domains.Theory.Models;

/// <summary>
/// A structure made up of intervals, which are expressed relative to the 0th
/// interval (unison), which will always be implicitly added to any interval
/// structure object if it is not explicitly added.
/// </summary>
public class IntervalStructure : ValueObject, IReadOnlySet<Interval>, ISymbolSerializable<IntervalStructure>
{
    public readonly HashSet<Interval> Intervals;

    public List<string> IntervalSymbols;
    public Interval? Third;
    public Interval? AlteredFifth;
    public List<Interval> Suspensions;
    public bool HasPerfectFifth;
    public bool HasAlteredFifth;
    public bool IsDominant;
    public bool IsDiminished;
    public bool IsTriad;
    public bool IsTetrad;
    public bool IsHeptatonic;

    public static IntervalStructure Diatonic { get; } = CreateAsHeptatonic(RawScalePatterns.Diatonic);
    public static IntervalStructure Altered { get; } = CreateAsHeptatonic(RawScalePatterns.Altered);

    public IntervalStructure DoubleStructure
    {
        get
        {
            HashSet<Interval> intervals = [.. Intervals];
            foreach (Interval interval in Intervals)
            {
                intervals.Add(Interval.FromExpectedValue(interval.Degree, interval.Value + Numbers.SemitonesInOctave));
            }
            return new(intervals);
        }
    }

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
        IntervalSymbols = NomenclatureHelper.SortIntervalNames([.. Intervals.Select(x => x.Name.ToSymbol())]);
        Suspensions = [];
        IsDominant = IsSupersetOf([RawIntervals.MajorThird, RawIntervals.MinorSeventh]);
        IsDiminished = IsSupersetOf([RawIntervals.MinorThird, RawIntervals.DiminishedFifth, RawIntervals.DiminishedSeventh]);
        IsTriad = Count == 3;
        IsTetrad = Count == 4;
        IsHeptatonic = Count == Numbers.NotesInOctave;
        HasPerfectFifth = false;
        HasAlteredFifth = false;
        foreach ((int Degree, int Accidental) interval in Intervals)
        {
            if (interval == RawIntervals.MajorThird || interval == RawIntervals.MinorThird)
                Third = interval;
            if (interval == RawIntervals.PerfectFifth)
                HasPerfectFifth = true;
            if ((interval == RawIntervals.DiminishedThird || interval == RawIntervals.AugmentedThird)
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
        => CreateNew(intervalSymbols.Select(Interval.FromIntervalName));


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
            throw new AristoxenusException("Intervals must represent a heptatonic structure (have 7 unique intervals, including the unison).");
        }
        intervals = intervals.Order();
        List<Interval> sanitizedIntervals = [
            ..intervals.Select(
                (value, index) => Interval.FromExpectedValue(index + Numbers.IndexOffset, value)
                )
            ];
        return CreateNew(sanitizedIntervals);
    }

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


    /// <summary>
    /// Translate this interval structure into a chord symbol string. Mostly 
    /// used for chords, but this can be a shorthand for scale names as well.
    /// </summary>
    public string ToChordSymbol()
        => SymbolHelper.CreateChordSymbol(
                Intervals.Select(i => (i.Name.Degree, i.Name.Accidental)),
                style: null
            );


    /// <summary>
    /// Parse a chord symbol into its implied intervals.
    /// </summary>
    /// <param name="chordSymbol">A chord symbol, with or without a 
    /// root symbol (i.e. e.g. either "Cmaj7#11" or "maj7#11").</param>
    public static IntervalStructure FromChordSymbol(string chordSymbol)
        => SymbolHelper.ParseChordSymbol(chordSymbol);

    public string ToSymbol()
        => ToChordSymbol();

    public static IntervalStructure FromSymbol(string symbol)
        => FromChordSymbol(symbol);

    public Interval this[int index] => throw new NotImplementedException();

    public static implicit operator HashSet<Interval>(IntervalStructure structure)
        => [.. structure.Intervals];

    public static implicit operator IntervalStructure(HashSet<Interval> structure)
        => new(structure);

    public static implicit operator HashSet<(int, int)>(IntervalStructure structure)
        => [.. structure.Intervals];

    public static implicit operator IntervalStructure(HashSet<(int, int)> structure)
        => new([.. structure]);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        foreach (string interval in NomenclatureHelper.SortIntervalNames([..Intervals.Select(i => i.Name.ToSymbol())]))
        {
            yield return interval;
        }
    }
}
