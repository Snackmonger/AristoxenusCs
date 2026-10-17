using System.Collections;
using System.Collections.Immutable;

using Aristoxenus.Core.Bases;
using Aristoxenus.Core.Data;
using Aristoxenus.Core.Errors;
using Aristoxenus.Core.Interfaces;
using Aristoxenus.Core.Utilities;

namespace Aristoxenus.Core.Models;

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

    public static IntervalStructure Diatonic { get; } = CreateAsScale(RawScalePatterns.Diatonic);
    public static IntervalStructure Altered { get; } = CreateAsScale(RawScalePatterns.Altered);

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

    public IntervalStructure ReciprocalStructure 
        => new(Intervals.Select(it => it.Reciprocal));

    public IntervalStructure(IEnumerable<Interval> intervals)
    {
        List<Interval> sanitized = [];


        foreach (Interval interval in intervals)
        {
            if (sanitized.Contains(interval) || sanitized.Select(i => i.Value).Contains(interval.Value))
                throw new IntervalStructureHasDuplicates(intervals.Where(it => it.Value == interval.Value).Select(i => i.Name.ToSymbol()));
            sanitized.Add(interval);
        }
        Intervals = [.. sanitized];
        IntervalSymbols = [.. NomenclatureHelper
            .SortDegreeNames([.. Intervals.Select(x => x.Name)])
            .Select(s => SymbolHelper.CreateScaleDegreeSymbol(s))
        ];
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
            if (interval == RawIntervals.MajorThird
                || interval == RawIntervals.MinorThird)
                Third = interval;

            if (interval == RawIntervals.PerfectFifth)
                HasPerfectFifth = true;

            if (interval == RawIntervals.DiminishedThird
                || interval == RawIntervals.AugmentedThird
                || interval.Degree == 4
                || interval.Degree == 2)
                Suspensions.Add(interval);

            if (interval.Degree == 5
                && interval.Accidental != 0)
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
    /// Create a new interval structure from a collection of scale degree symbols.
    /// </summary>
    /// <param name="scaleDegreeSymbols"></param>
    /// <returns></returns>
    public static IntervalStructure FromScaleDegreeSymbols(IEnumerable<string> scaleDegreeSymbols)
        => CreateNew(scaleDegreeSymbols.Select(Interval.FromScaleDegreeSymbol));

    /// <summary>
    /// Treat the collection of integers as a single scale. If the scale is
    /// heptatonic, it will have one interval name for each of CDEFGAB. 
    /// Otherwise, the scale will have names derived from the diatonic if
    /// the value exists in that scale, or the altered if not.
    /// </summary>
    public static IntervalStructure CreateAsScale(IEnumerable<int> intervals)
    {
        HashSet<int> uniqueSingleOctaveValues = [Numbers.Zero, .. intervals.Select(x => x % Numbers.SemitonesInOctave)];
        List<Interval> scaleDegrees = [];
        if (uniqueSingleOctaveValues.Count == Numbers.NotesInOctave)
        {
            scaleDegrees = [
                ..uniqueSingleOctaveValues
                .Order()
                .Select((value, index) => Interval.FromExpectedValue(index + Numbers.IndexOffset, value))
            ];
        }
        else
        {
            foreach (int interval in uniqueSingleOctaveValues)
            {
                if (RawScalePatterns.Diatonic.Contains(interval))
                {
                    scaleDegrees.Add(Diatonic.First(it => it.Value == interval));
                }
                else
                {
                    scaleDegrees.Add(Altered.First(it => it.Value == interval));
                }
            }
        }
        return CreateNew(scaleDegrees);
    }

    /// <summary>
    /// Attempts to identify the scale represented by this interval structure.
    /// </summary>
    public bool CanIdentifyScale(out string? name, out int? mode)
    {
        name = default;
        mode = default;
        if (PatternHelper.TryIdentifyScale(Intervals.Select(it => it.Value), out name, out mode))
            return true;
        return false;
    }

    public IEnumerable<Interval> Select(Func<Interval, bool> predicate)
        => Intervals.Where(predicate);

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
    /// Pretty print a list of the name symbol of the intervals in the 
    /// structure.
    /// </summary>
    public string PrettyPrintSymbols()
        => string.Join(", ", Intervals.Select(i => i.Name.ToSymbol()));


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

    /// <summary>
    /// Return a string symbol representing this interval structure.
    /// </summary>
    public string ToSymbol() => ToChordSymbol();

    /// <summary>
    /// Parse a string symbol into an interval structure object.
    /// </summary>
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

    public static implicit operator List<Interval>(IntervalStructure structure)
        => [.. structure.Intervals];

    public static implicit operator IntervalStructure(List<Interval> structure)
    => new(structure);


    protected override IEnumerable<object?> GetEqualityComponents()
    {
        foreach ((int, int) interval in NomenclatureHelper
            .SortIntervalNames(Intervals.Select(it => (it.Name.Degree, it.Name.Accidental)))
        )
        {
            yield return interval;
        }
    }
}
