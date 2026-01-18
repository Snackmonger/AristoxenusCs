namespace Aristoxenus.Domain.Models;

using System.Collections;
using System.Text.RegularExpressions;

using Aristoxenus.Constants;
using Aristoxenus.Domain.Exceptions;
using Aristoxenus.SharedKernel.Bases;
using Aristoxenus.Utilities;


/// <summary>
/// A set of intervals relative to a pitch at the unison.
/// </summary>
public class IntervalStructure : ValueObject, IReadOnlySet<Interval>
{

    public readonly HashSet<Interval> Intervals;

    /// <summary>
    /// The raw integer values of the intervals in this structure, which 
    /// should not be used to test for equality because they do not 
    /// recognize scale degrees (e.g. #4 and b5 both == 6).
    /// </summary>
    public List<int> Values
        => [.. Intervals.Select(x => x.Value).Order()];

    /// <summary>
    /// An ordered list of all intervals in this structure, represented
    /// as interval symbols.
    /// </summary>
    public List<string> IntervalSymbols
        => [.. IntervalSortHelper.SortIntervalNames(Intervals.Select(x => x.Name.Symbol))];

    /// <summary>
    /// The number of factors (unique pitch identities) in the set.
    /// For example, b9 and b2 are different intervals, but they are the same
    /// factor of the interval set (therefore, this number may be different 
    /// than the integer returned from the `Count` method).
    /// </summary>
    public int NumberOfFactors
        => Intervals.Select(x => x.SingleOctaveValue).ToHashSet().Count;

    public string Symbol
        => ToChordSymbol();

    private IntervalStructure(IEnumerable<Interval> intervals)
    {
        Intervals = [.. intervals];
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
            {
                throw new IntervalStructureHasDuplicates(intervals.Select(i => i.Name.Symbol));
            }
            sanitized.Add(interval);
        }
        return new IntervalStructure(sanitized);
    }

    /// <summary>
    /// Create a new interval structure from a collection of interval symbols.
    /// </summary>
    /// <param name="intervalNames"></param>
    /// <returns></returns>
    public static IntervalStructure CreateNew(IEnumerable<string> intervalNames)
        => CreateNew(intervalNames.Select(n => Interval.FromSymbol(n)));

    /// <summary>
    /// Create an interval structure on the assumption that the given 
    /// integers resolve to a heptatonic scale. That is: there must be
    /// exactly seven unique factors (including a unison = 0).
    /// </summary>
    /// <param name="intervals">The integers that wil be considered as intervals.</param>
    /// <exception cref="ArgumentException">If the structure is not a heptatonic octave.</exception>
    public static IntervalStructure CreateAsHeptatonic(IEnumerable<int> intervals)
    {
        HashSet<int> unique = [0, .. intervals.Select(x => x % Numbers.SemitonesInOctave)];
        if (unique.Count != 7)
        {
            throw new DomainException("Intervals must represent a heptatonic structure (have 7 unique intervals, including the unison).");
        }
        intervals = intervals.Order();
        List<Interval> sanitizedIntervals = [
            ..intervals.Select(
                (value, index) => Interval.FromExpectedValue(index + 1, value)
                )
            ];
        return CreateNew(sanitizedIntervals);
    }

    public int Count
        => Intervals.Count();

    #region ReadOnlySet<Interval> implementations

    public bool Contains(Interval interval)
        => Intervals.Contains(interval);

    public bool Contains(IntervalName interval)
        => Intervals.Select(i => i.Name).Contains(interval);

    public IEnumerator<Interval> GetEnumerator()
        => Intervals.GetEnumerator();

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

    #endregion

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    #region IReadOnlySet<string> pseudo-methods
    public bool Contains(string interval)
        => Intervals.Select(i => i.Name.Symbol).Contains(interval);

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
    #endregion

    #region Copy-on-write mutations
    public IntervalStructure Remove(Interval interval)
    {
        List<Interval> _intervals = [];
        foreach (Interval i in Intervals)
        {
            if (!i.Equals(interval) || interval.Value == 0)
            {
                _intervals.Add(i);
            }
        }
        return CreateNew(_intervals);
    }

    public IntervalStructure Remove(string interval)
    {
        List<Interval> _intervals = [];
        foreach (Interval i in Intervals)
        {
            if (i.Name.Symbol != interval || i.Value == 0)
            {
                _intervals.Add(i);
            }
        }
        return CreateNew(_intervals);
    }

    public IntervalStructure Remove(params string[] intervals)
    {
        List<Interval> _intervals = [];
        foreach (Interval i in Intervals)
        {
            if (!intervals.Contains(i.Name.Symbol) || i.Value == 0)
            {
                _intervals.Add(i);
            }
        }
        return CreateNew(_intervals);
    }

    public IntervalStructure Add(Interval interval)
        => CreateNew([interval, .. Intervals]);


    public IntervalStructure Add(string interval)
        => CreateNew([Interval.FromSymbol(interval), .. Intervals]);

    #endregion

    /// <summary>
    /// Return the third of the structure or null if none exists.
    /// </summary>
    public Interval? Third
    {
        get
        {
            foreach (Interval interval in Intervals)
            {
                if ((new[] {
                    Symbols.Three,
                    Symbols.FlatThree
                }).Contains(interval.Name.Symbol))
                {
                    return interval;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Return the altered fifth of the structure, or null if none exists.
    /// </summary>
    public Interval? AlteredFifth
    {
        get
        {
            foreach (Interval interval in Intervals)
            {
                if ((new[] {
                    Symbols.FlatFive,
                    Symbols.SharpFive,
                    Symbols.DoubleFlatFive,
                    Symbols.DoubleSharpFive
                }).Contains(interval.Name.Symbol))
                {
                    return interval;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Return the suspension of the structure, or null if none exists.
    /// </summary>
    public List<Interval> Suspension
    {
        get
        {
            List<Interval> candidates = [];
            foreach (Interval interval in Intervals)
            {

                foreach (string x in new[] {
                    Symbols.SharpThree,
                    Symbols.DoubleFlatThree,
                    Symbols.Two,
                    Symbols.Four,
                    Symbols.SharpTwo,
                    Symbols.FlatTwo,
                    Symbols.SharpFour,
                    Symbols.FlatFour
                })
                {
                    if (interval.Name.Symbol == x)
                    {
                        candidates.Add(interval);
                    }
                }
            }
            return candidates;
        }
    }

    /// <summary>
    /// Flag indicating whether this structure contains a perfect fifth.
    /// </summary>
    public bool HasPerfectFifth
        => Contains(Symbols.Five);

    /// <summary>
    /// Flag indicating whether this structure contains an altered
    /// (i.e. non-perfect) fifth.
    /// </summary>
    public bool HasAlteredFifth
        => Contains(Symbols.FlatFive)
        || Contains(Symbols.SharpFive)
        || Contains(Symbols.DoubleFlatFive)
        || Contains(Symbols.DoubleSharpFive);

    /// <summary>
    /// Flag indicating whether this structure contains a major third and 
    /// dominant seven above the unison.
    /// </summary>
    public bool IsDominant
        => IsSupersetOf([Symbols.Three, Symbols.FlatSeven]);

    /// <summary>
    /// Flag indicating whether this structure contains a minor third,
    /// diminished fifth, and diminished seventh above the unison.
    /// </summary>
    public bool IsDiminished
        => IsSupersetOf([Symbols.FlatThree, Symbols.FlatFive, Symbols.DoubleFlatSeven]);

    /// <summary>
    /// Flag indicating whether this structure is a triad.
    /// </summary>
    public bool IsTriad
        => Count == 3;

    /// <summary>
    /// Flag indicating whether this structure is a tetrad.
    /// </summary>
    public bool IsTetrad
        => Count == 4;

    /// <summary>
    /// Flag indicating whether this structure is heptatonic.
    /// </summary>
    public bool IsHeptatonic
        => Count == 7;

    /// <summary>
    /// Translate this interval structure into a chord symbol string.
    /// </summary>
    /// <param name="style">Replacement symbols for the basic chord symbol 
    /// elements, allowing you to turn "maj" into "Δ", "M", etc.
    /// <note>
    /// Only major, minor, and diminished symbols can be configured (augmented
    /// and diminished triads and their derivatives are always written as 
    /// "maj#5" and "minb5", or "maj7#5", "min7b5", etc.).
    /// </note></param>
    /// <returns></returns>
    public string ToChordSymbol((string? MajorSymbol, string? MinorSymbol, string? DiminishedSymbol)? style = null)
    {
        // These values represent the default symbols that will be used 
        // for basic chord categories: Cmaj vs CM vs CΔ, etc. We don't 
        // check these, so it's possible to make chord symbols that our
        // domain won't actually recognize.
        string MajorSymbol = style?.MajorSymbol ?? Symbols.Major;
        string MinorSymbol = style?.MinorSymbol ?? Symbols.Minor;
        string DiminishedSymbol = style?.DiminishedSymbol ?? Symbols.Diminished;


        // The total set of suffixes that might be added to a note name
        // to form a chord symbol. Only a subset of suffixes can be used
        // in the same symbol. The order is designed to make chord symbols
        // (including those for exotic chords) easier to read.
        string normal3 = string.Empty;
        string primary = string.Empty;
        string secondary = string.Empty;
        string sus = string.Empty;
        string alt5 = string.Empty;
        string alt7 = string.Empty;
        string add = string.Empty;
        string no5 = string.Empty;
        string no3 = string.Empty;
        string extensions = string.Empty;

        IntervalStructure intervals = CreateNew(Intervals);

        ///////////////////////////////////////////////////////////////////////
        // Diminished Chord: Special structure on all notes.
        ///////////////////////////////////////////////////////////////////////
        if (IsDiminished)
        {
            normal3 = DiminishedSymbol;
            primary = Symbols.Seven;
            intervals = intervals.Remove(Symbols.DoubleFlatSeven, Symbols.FlatFive, Symbols.FlatThree);
        }
        // If not a diminished chord, then any altered notes have to
        // be specially indicated in the symbol:
        else
        {
            // Exotic chords are allowed to have bb7 in our system, but
            // since the bb accidental might conflict with the note name
            // in a 7th chord (e.g. 'Bbb7'), we compel it to take the
            // alt7 slot instead of the primary 7 slot (e.g. Bbmajbb7,
            // Ebbminbb7).
            if (intervals.Contains(Symbols.DoubleFlatSeven))
            {
                alt7 = Symbols.DoubleFlatSeven;
                intervals = intervals.Remove(Symbols.DoubleFlatSeven);
            }
            // A non-diminished chord with an altered 5th must always have
            // an explicit alt5 symbol.
            if (AlteredFifth is Interval fifth)
            {
                alt5 = fifth.Name.Symbol;
                intervals = intervals.Remove(fifth);
            }
            // A chord with no 5th must have an explicit no5 symbol.
            else if (!HasPerfectFifth)
            {
                no5 = Symbols.No + Symbols.Five;
            }
            // The fifth is implied in any other chord and has no symbol.
            else
            {
                intervals = intervals.Remove(Symbols.Five);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Dominant Chord: The 7 symbol is used to represent a b7 interval.
        ///////////////////////////////////////////////////////////////////////
        if (IsDominant)
        {
            primary = Symbols.Seven;
            intervals = intervals.Remove(Symbols.Three, Symbols.FlatSeven);
        }

        ///////////////////////////////////////////////////////////////////////
        // Chords with a third: We always write 'maj' or 'min' (or whatever
        // default the user chose) to indicate the third of a chord, unless
        // some other symbol already implies it (i.e. diminished and dominant).
        ///////////////////////////////////////////////////////////////////////
        if (Third is Interval n)
        {
            // Dominant and diminished chords do not have a symbol
            // for thier third.
            if (!(IsDiminished || IsDominant))
            {
                // Major chord:
                if (n.Name.Symbol == Symbols.Three)
                {
                    normal3 = MajorSymbol;

                    // Major 7: The 7 symbol is used to represent a natural
                    // 7 interval.
                    if (intervals.Contains(Symbols.Seven))
                    {
                        primary = Symbols.Seven;
                        intervals = intervals.Remove(Symbols.Seven);
                    }
                }

                // Minor chord:
                else if (n.Name.Symbol == Symbols.FlatThree)
                {
                    normal3 = MinorSymbol;

                    // Minor major 7: the natural 7 is symbolized with a
                    // special symbol.
                    if (intervals.Contains(Symbols.Seven))
                    {
                        primary = MajorSymbol + Symbols.Seven;
                        intervals = intervals.Remove(Symbols.Seven);
                    }

                    // Minor 7: The 7 symbol is used to represent a b7 interval.
                    if (intervals.Contains(Symbols.FlatSeven))
                    {
                        primary = Symbols.Seven;
                        intervals = intervals.Remove(Symbols.FlatSeven);
                    }
                }
                intervals = intervals.Remove(n);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Sus Chords: We allow for a few unusual chords that give
        // enharmonic sus spellings, so that (e.g.) a sus2 might be
        // spelled susbb3 depending on the underlying scale. We categorize
        // #3 and bb3 as 'sus' chords a) because they cannot reasonably be
        // labeled major or minor, b) so that their accidental cannot stand
        // next to the root note (i.e. e.g. Dsusbb3 is less ambiguous than
        // Dbb3).
        ///////////////////////////////////////////////////////////////////////
        else if (Suspension.Count > 0)
        {
            // If there's more than one candidate for a sus note, just
            // take the first one and treat the extras as 'add' notes,
            // (e.g. 1, 2, 4, 5 -> sus2add4).
            Interval main = Suspension[0];
            sus = Symbols.Sus + main.Name.Symbol;
            var candidates = Suspension[1..];
            intervals = intervals.Remove(main);
            foreach (Interval x in candidates)
            {
                add += Symbols.Add + x.Name.Symbol;
                intervals = intervals.Remove(x);
            }
            if (intervals.Contains(Symbols.Seven))
            {
                primary = MajorSymbol + Symbols.Seven;
                intervals = intervals.Remove(Symbols.Seven);
            }
            else if (intervals.Contains(Symbols.FlatSeven))
            {
                primary = Symbols.Seven;
                intervals = intervals.Remove(Symbols.FlatSeven);
            }
        }
        // Any chord without a 3 and without a sus must have a no3 symbol
        else
        {
            no3 = Symbols.No + Symbols.Three;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Primary Extension: Our system treats primary extension symbols as
        // implying ALL previous extensions in the series, i.e. a 13 chord
        // includes 7, 9, 11, 13. If this series is interrupted by a missing or
        // altered note, then the primary extension is the last continuous
        // extension, and any following natural extensions are treated as
        // additions (e.g. C9#11add13).
        ///////////////////////////////////////////////////////////////////////////
        string[] NaturalExtensions = [Symbols.Nine, Symbols.Eleven, Symbols.Thirteen];
        string largest = string.Empty;
        if (primary != string.Empty)
        {
            List<string> checkedExtensions = [];
            foreach ((string extension, int index) in NaturalExtensions.Select((string e, int i) => (e, i)))
            {
                string? prev = index > 0 ? NaturalExtensions[index - 1] : null;
                if (intervals.Contains(extension))
                {
                    if (prev is null || checkedExtensions.Contains(prev))
                    {
                        largest = extension;
                        intervals = intervals.Remove(extension);
                        checkedExtensions.Add(extension);
                    }
                    else
                    {
                        add += Symbols.Add + extension;
                        intervals = intervals.Remove(extension);
                        checkedExtensions.Add(extension);
                    }
                }
            }
            if (largest != string.Empty)
            {
                primary = primary.Replace(Symbols.Seven, largest);
            }
        }
        ///////////////////////////////////////////////////////////////////////
        // Secondary Extension: For chords where a 6 replaces a 7. In this 
        // case, only the natural 6 is ever used; any other type of 6 will be
        // written as an addition. In cases where a primary suffix also
        // exists, the secondary suffix becomes an addition.
        ///////////////////////////////////////////////////////////////////////
        if (intervals.Contains(Symbols.Six))
        {
            intervals = intervals.Remove(Symbols.Six);

            if (primary != string.Empty)
            {
                add += Symbols.Add + Symbols.Six;
            }
            else
            {
                secondary = Symbols.Six;
            }
        }
        ///////////////////////////////////////////////////////////////////////
        // Tertiary Extensions 
        ///////////////////////////////////////////////////////////////////////
        // Added Extensions: We characterize some extensions as "add" type
        // symbols to make them easier to read (e.g. C7add13 vs C713)
        foreach (Interval extension in intervals.Where(i => i.Value > 0))
        {
            if (extension.Accidental == 0)
            {
                add += Symbols.Add + extension.Name.Symbol;
                intervals = intervals.Remove(extension);
            }
        }
        // Altered Extensions: Any extension with an accidental does not need
        // an "add" symbol and if anything is harder to read if it has one
        // (e.g. C7#11 vs C7add#11).
        extensions = string.Join(string.Empty, intervals.IntervalSymbols.Where(s => s != "1"));

        ///////////////////////////////////////////////////////////////////////
        // Final Symbol:
        // Most suffixes will be empty strings in any given chord, but the 
        // sequence helps to ensure that they will be easy to read when they
        // do appear
        ///////////////////////////////////////////////////////////////////////
        string[] symbols = [
                    normal3,
                    primary,
                    secondary,
                    sus,
                    no3,
                    no5,
                    alt5,
                    alt7,
                    extensions,
                    add
            ];
        return string.Join(string.Empty, symbols);
    }

    public static IntervalStructure FromChordSymbol(string chordSymbol)
    {
        IntervalStructure intervals = new([Interval.CreateNew(1, 0), Interval.CreateNew(5, 0)]);
        // Regex sorts elements into four categories.
        //
        // 1) The root symbol defines the note name or Roman interval relative to
        // which the chord's structure is understood (not used in this method).
        // 2) The main symbol generally defines the third and may imply something
        // about the 7, e.g. "maj", "min", "dim"
        // 3) The extension symbol defines the 7th or any natural note that is
        // allowed to replace it, i.e. "7", "9", "11", "13"
        // 4) The modification symbol(s) include any other interval symbol,
        // or an add, no, or sus, e.g. "sus2", "add2", "#11", "no5"
        //
        // After this, we have most information about a chord, and just need to
        // check edge cases and transform symbols into interval names.
        Match match = RegularExpressions.ChordSymbol.Match(chordSymbol);
        if (!match.Success)
        {
            match = RegularExpressions.RootlessChordSymbol.Match(chordSymbol);
            if (!match.Success)
            {
                throw new InvalidAsSymbol(chordSymbol, nameof(chordSymbol));
            }
        }
        string main = match.Groups.GetValueOrDefault(Keywords.Main)?.Value ?? string.Empty;
        string extension = match.Groups.GetValueOrDefault(Keywords.Extension)?.Value ?? string.Empty;
        string modifications = match.Groups.GetValueOrDefault(Keywords.Modification)?.Value ?? string.Empty;
        List<string> extensionSeries = [Symbols.Seven, Symbols.Nine, Symbols.Eleven, Symbols.Thirteen];
        List<string> susIntervals = [Symbols.DoubleFlatThree, Symbols.SharpThree, Symbols.Four, Symbols.Two];

        // Bare chords imply major,
        // e.g. "C", "Ab", "G"
        if (main == string.Empty)
        {
            intervals = intervals.Add(Symbols.Three);

            // Bare chord plus extension implies b7,
            // e.g. "C7", "A11", "Gb13"
            if (extensionSeries.Contains(extension))
            {
                int i = extensionSeries.IndexOf(extension) + 1;
                foreach (string interval in extensionSeries[..i])
                {
                    intervals = intervals.Add(
                        interval == Symbols.Seven
                            ? Symbols.FlatSeven
                            : interval
                    );
                }
                extension = string.Empty;
            }
        }
        // Explicit major chord,
        // e.g. "Cmaj", "BbM", 'F#Δ'
        else if (Symbols.RecognizedMajorSymbols.Contains(main))
        {
            intervals = intervals.Add(Symbols.Three);

            // Explicit major plus extension implies natural 7,
            // e.g. 'Cmaj7', 'AM7', 'F#Δ7'
            if (extensionSeries.Contains(extension))
            {
                int i = extensionSeries.IndexOf(extension) + 1;
                foreach (string interval in extensionSeries[..i])
                {
                    intervals = intervals.Add(interval);
                }
                extension = string.Empty;
            }
        }
        // Diminished chords, e.g. 'Cdim', 'Ao'
        else if (Symbols.RecognizedDiminishedSymbols.Contains(main))
        {
            intervals = intervals
                            .Add(Symbols.FlatThree)
                            .Add(Symbols.FlatFive)
                            .Remove(Symbols.Five);

            // Diminished 7 implies bb7, e.g. 'Cdim7', 'Ao9'
            if (extensionSeries.Contains(extension))
            {
                int i = extensionSeries.IndexOf(extension) + 1;
                foreach (string interval in extensionSeries[..i])
                {
                    intervals = intervals.Add(interval == Symbols.Seven ? Symbols.DoubleFlatSeven : interval);
                }
                extension = string.Empty;
            }
        }
        // Augmented chords, e.g. 'Caug', 'A+'
        else if (Symbols.RecognizedAugmentedSymbols.Contains(main))
        {
            intervals = intervals
                            .Add(Symbols.Three)
                            .Add(Symbols.SharpFive)
                            .Remove(Symbols.Five);

        }
        // Half-diminished chords, e.g. 'Cø'
        else if (Symbols.RecognizedHalfDiminishedSymbols.Contains(main))
        {
            intervals = intervals
                            .Add(Symbols.FlatThree)
                            .Add(Symbols.FlatFive)
                            .Remove(Symbols.Five);

        }

        // Minor chords, e.g. 'Cmin', 'Am', 'F#-'
        else if (Symbols.RecognizedMinorSymbols.Contains(main))
        {
            intervals = intervals.Add(Symbols.FlatThree);
        }

        // Apart from major and diminished chords, above, a 7 implies a flat 7,
        // and a natural 7 must be indicated as a major 7.
        if (extension != string.Empty)
        {
            // e.g. "Gmaj7", "FM9", "DbΔ7"
            foreach (string symb in Symbols.RecognizedMajorSymbols)
            {
                if (extension.Contains(symb))
                {
                    string baseNumber = extension.Replace(symb, string.Empty);
                    if (extensionSeries.Contains(baseNumber))
                    {
                        int i = extensionSeries.IndexOf(baseNumber) + 1;
                        foreach (string interval in extensionSeries[..i])
                        {
                            intervals = intervals.Add(interval);
                        }
                    }
                }
            }
            // Any other 7 is a b7,
            // e.g. "Cmin7", "Gaug7"
            if (extensionSeries.Contains(extension))
            {
                int i = extensionSeries.IndexOf(extension) + 1;
                foreach (string interval in extensionSeries[..i])
                {
                    intervals = intervals.Add(interval == Symbols.Seven ? Symbols.FlatSeven : interval);
                }
            }
        }

        List<string> subtractions = [];
        // Chords not in one of the above categories may have some kind of
        // special modifier describing their structure.
        if (modifications != string.Empty)
        {
            foreach (string sus in susIntervals)
            {
                if (modifications.Contains(Symbols.Sus + sus))
                {
                    modifications = modifications.Replace(Symbols.Sus + sus, string.Empty);
                    intervals = intervals.Add(sus);
                    subtractions.AddRange([Symbols.Three, Symbols.FlatThree]);
                    main = string.Empty;
                }
            }

            // Special chords in our system might have bb7.
            if (modifications.Contains(Symbols.DoubleFlatSeven))
            {
                string name = Symbols.No + Symbols.DoubleFlatSeven;
                if (modifications.Contains(name))
                {
                    subtractions.Add(Symbols.DoubleFlatSeven);
                    modifications = modifications.Replace(name, string.Empty);
                }
                else
                {
                    name = Symbols.Add + Symbols.DoubleFlatSeven;
                    if (modifications.Contains(name))
                    {
                        intervals = intervals.Add(Symbols.DoubleFlatSeven);
                        modifications = modifications.Replace(name, string.Empty);
                    }
                    else
                    {
                        intervals = intervals.Add(Symbols.DoubleFlatSeven);
                        modifications = modifications.Replace(Symbols.DoubleFlatSeven, string.Empty);
                    }
                }
            }

            // Most remaining symbols will be a bare interval, an addition, or
            // subtraction. 
            int[] _intervals = [15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
            foreach (int number in _intervals)
            {
                if (modifications == string.Empty) break;

                // TODO: This will currently not recognize 
                // double, triple, etc. accidentals, e.g. Cmaj7##11.
                // For chords in common use, this is not a problem, but it
                // means that certain domain-generated exotic chords will
                // cause errors leter.
                foreach (string accidental in new[] { Symbols.Sharp, Symbols.Flat, string.Empty })
                {
                    string interval = accidental + number.ToString();

                    if (modifications.Contains(Symbols.Add + interval))
                    {
                        intervals = intervals.Add(interval);
                        modifications = modifications.Replace(Symbols.Add + interval, string.Empty);
                    }
                    else if (modifications.Contains(Symbols.No + interval))
                    {
                        intervals = intervals.Remove(interval);
                        modifications = modifications.Replace(Symbols.No + interval, string.Empty);
                    }
                    else if (modifications.Contains(interval))
                    {
                        intervals = intervals.Add(interval);
                        modifications = modifications.Replace(interval, string.Empty);
                        if ((new[] { Symbols.SharpFive, Symbols.FlatFive }).Contains(interval))
                        {
                            intervals = intervals.Remove(Symbols.Five);
                        }
                    }
                }

            }
            // Augmented symbols are either main (already handled) or
            // modification,
            // e.g. Caug7 vs. C7aug
            foreach (string aug in Symbols.RecognizedAugmentedSymbols)
            {
                if (modifications.Contains(aug))
                {
                    intervals = intervals
                                    .Remove(Symbols.Five)
                                    .Add(Symbols.SharpFive);
                }
            }
        }
        //  Subtractions are treated last in case they depend on
        //  an implication in a preceding symbol.
        foreach (string s in subtractions)
        {
            if (intervals.Contains(s))
            {
                intervals = intervals.Remove(s);
            }
        }

        return intervals;

    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (string symbol in IntervalSymbols)
        {
            yield return symbol;
        }
    }
}
