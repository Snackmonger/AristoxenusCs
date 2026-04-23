namespace Aristoxenus.Core.Domain.Theory;

using Aristoxenus.Core.Constants;
using Aristoxenus.Core.Domain.Exceptions;
using Aristoxenus.Core.SharedKernel.Bases;
using Aristoxenus.Core.SharedKernel.Interfaces;
using Aristoxenus.Core.Utilities;
using System.Collections;
using System.Collections.Immutable;
using System.Text.RegularExpressions;


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

    public static IntervalStructure Diatonic { get; } = CreateAsHeptatonic(ScalePatternsHeptatonic.Diatonic);
    public static IntervalStructure Altered { get; } = CreateAsHeptatonic(ScalePatternsHeptatonic.Altered);

    private IntervalStructure(IEnumerable<Interval> intervals)
    {
        Intervals = [.. intervals];
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
    /// <param name="intervalNames"></param>
    /// <returns></returns>
    public static IntervalStructure CreateNew(IEnumerable<string> intervalNames)
        => CreateNew(intervalNames.Select(n => Interval.FromSymbol(n)));

    public static IntervalStructure CreateNew()
    {
        return CreateNew([Symbols.One]);
    }

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

    // IReadOnlySet <Interval> ////////////////////////////////////////////////

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

    // Copy-on-write mutations ////////////////////////////////////////////////


    public IntervalStructure Remove(Interval interval)
    {
        List<Interval> _intervals = [];
        foreach (Interval i in Intervals)
        {
            if (!i.Equals(interval) || interval.Value == Numbers.Zero)
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
            if (i.Name.ToSymbol() != interval || i.Value == Numbers.Zero)
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
            if (!intervals.Contains(i.Name.ToSymbol()) || i.Value == Numbers.Zero)
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

    // Chord Symbol Api ///////////////////////////////////////////////////////

    /// <summary>
    /// Translate this interval structure into a chord symbol string.
    /// </summary>
    /// <param name="style">
    /// Replacement symbols for the basic chord symbol elements, 
    /// allowing you to turn "maj" into "Δ", "M", or anything else.
    /// <note>
    /// Only major, minor, and diminished symbols can be configured (augmented
    /// and diminished triads and their derivatives are always written as 
    /// "maj#5" and "minb5", or "maj7#5", "min7b5", etc.). This method accepts
    /// arbitrary strings for this configuration, meaning it is possible to 
    /// generate symbols that this class will NOT be able to parse.
    /// </note>
    /// </param>
    public string ToChordSymbol((string? MajorSymbol, string? MinorSymbol, string? DiminishedSymbol)? style = null)
    {
        // These values represent the default symbols that will be used 
        // for basic chord categories: Cmaj vs CM vs CΔ, etc. We don't 
        // check these, so it's possible to make chord symbols that our
        // own `FromChordSymbol` method won't actually recognize. 
        string MajorSymbol = style?.MajorSymbol ?? Symbols.Major;
        string MinorSymbol = style?.MinorSymbol ?? Symbols.Minor;
        string DiminishedSymbol = style?.DiminishedSymbol ?? Symbols.Diminished;


        // The total set of suffixes that might be added to a note name
        // to form a chord symbol. Only a subset of suffixes can appear
        // in any given symbol. The order is designed to make chord symbols
        // (including those for exotic chords) easier to read:
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

        HashSet<string> remaining = [.. IntervalSymbols];

        ///////////////////////////////////////////////////////////////////////
        // Diminished Chord: Special structure on all notes.
        ///////////////////////////////////////////////////////////////////////
        if (IsDiminished)
        {
            normal3 = DiminishedSymbol;
            primary = Symbols.Seven;
            remaining.Remove(Symbols.DoubleFlatSeven);
            remaining.Remove(Symbols.FlatFive);
            remaining.Remove(Symbols.FlatThree);
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
            if (remaining.Contains(Symbols.DoubleFlatSeven))
            {
                alt7 = Symbols.DoubleFlatSeven;
                remaining.Remove(Symbols.DoubleFlatSeven);
            }
            // A non-diminished chord with an altered 5th must always have
            // an explicit alt5 symbol.
            if (AlteredFifth is Interval fifth)
            {
                alt5 = fifth.Name.ToSymbol();
                remaining.Remove(alt5);
            }
            // A chord with no 5th must have an explicit no5 symbol.
            else if (!HasPerfectFifth)
            {
                no5 = Symbols.No + Symbols.Five;
            }
            // The fifth is implied in any other chord and has no symbol.
            else
            {
                remaining.Remove(Symbols.Five);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Dominant Chord: The 7 symbol is used to represent a b7 interval.
        ///////////////////////////////////////////////////////////////////////
        if (IsDominant)
        {
            primary = Symbols.Seven;
            remaining.Remove(Symbols.Three);
            remaining.Remove(Symbols.FlatSeven);
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
                string thirdSymbol = n.Name.ToSymbol();
                // Major chord:
                if (thirdSymbol == Symbols.Three)
                {
                    normal3 = MajorSymbol;

                    // Major 7: The 7 symbol is used to represent a natural
                    // 7 interval.
                    if (remaining.Contains(Symbols.Seven))
                    {
                        primary = Symbols.Seven;
                        remaining.Remove(Symbols.Seven);
                    }
                }

                // Minor chord:
                else if (thirdSymbol == Symbols.FlatThree)
                {
                    normal3 = MinorSymbol;

                    // Minor major 7: the natural 7 is symbolized with a
                    // special symbol.
                    if (remaining.Contains(Symbols.Seven))
                    {
                        primary = MajorSymbol + Symbols.Seven;
                        remaining.Remove(Symbols.Seven);
                    }

                    // Minor 7: The 7 symbol is used to represent a b7 interval.
                    if (remaining.Contains(Symbols.FlatSeven))
                    {
                        primary = Symbols.Seven;
                        remaining.Remove(Symbols.FlatSeven);
                    }
                }
                remaining.Remove(thirdSymbol);
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
        else if (Suspensions.Count > Numbers.Zero)
        {
            // If there's more than one candidate for a sus note, just
            // take the first one and treat the extras as 'add' notes,
            // (e.g. 1, 2, 4, 5 -> sus2add4).
            Interval main = Suspensions[Numbers.Zero];
            string mainSus = main.Name.ToSymbol();
            sus = Symbols.Sus + mainSus;
            remaining.Remove(mainSus);
            foreach (Interval x in Suspensions[1..])
            {
                string xSymb = x.Name.ToSymbol();
                add += Symbols.Add + xSymb;
                remaining.Remove(xSymb);
            }
            if (remaining.Contains(Symbols.Seven))
            {
                primary = MajorSymbol + Symbols.Seven;
                remaining.Remove(Symbols.Seven);
            }
            else if (remaining.Contains(Symbols.FlatSeven))
            {
                primary = Symbols.Seven;
                remaining.Remove(Symbols.FlatSeven);
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
            foreach ((string extension, int index) in NaturalExtensions.Select((e, i) => (e, i)))
            {
                string? prev = index > Numbers.Zero ? NaturalExtensions[index - Numbers.IndexOffset] : null;
                if (remaining.Contains(extension))
                {
                    if (prev is null || checkedExtensions.Contains(prev))
                    {
                        largest = extension;
                        remaining.Remove(extension);
                        checkedExtensions.Add(extension);
                    }
                    else
                    {
                        add += Symbols.Add + extension;
                        remaining.Remove(extension);
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
        if (remaining.Contains(Symbols.Six))
        {
            remaining.Remove(Symbols.Six);

            if (primary != string.Empty)
                add += Symbols.Add + Symbols.Six;
            else
                secondary = Symbols.Six;
        }
        ///////////////////////////////////////////////////////////////////////
        // Tertiary Extensions 
        ///////////////////////////////////////////////////////////////////////
        // Added Extensions: We characterize some extensions as "add" type
        // symbols to make them easier to read (e.g. C7add13 vs C713)
        foreach (string ext in remaining.Where(i => i != Symbols.One))
        {
            Interval extension = Interval.FromSymbol(ext);
            if (extension.Accidental == Numbers.Zero)
            {
                add += Symbols.Add + extension.Name.ToSymbol();
                remaining.Remove(ext);
            }
        }
        // Altered Extensions: Any extension with an accidental does not need
        // an "add" symbol (and if anything is harder to read if it has one,
        // e.g. C7#11 vs C7add#11).
        extensions = string.Join(string.Empty, NomenclatureHelper.SortIntervalNames(remaining.Where(s => s != Symbols.One))
        );

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

    /// <summary>
    /// Parse a chord symbol into its implied intervals.
    /// </summary>
    /// <param name="chordSymbol">A chord symbol, with or without a 
    /// root symbol (i.e. e.g. either "Cmaj7#11" or "maj7#11").</param>
    /// <returns></returns>
    /// <exception cref="InvalidAsSymbol">If the symbol cannot be parsed.
    /// </exception>
    public static IntervalStructure FromChordSymbol(string chordSymbol)
    {
        HashSet<string> intervalSet = [Symbols.One, Symbols.Five];
        List<string> susIntervals = [Symbols.DoubleFlatThree, Symbols.SharpThree, Symbols.Four, Symbols.Two];
        List<string> extensionSeries = [Symbols.Seven, Symbols.Nine, Symbols.Eleven, Symbols.Thirteen];
        Dictionary<string, int> extensionIndex = extensionSeries
            .Select((s, i) => (s, i))
            .ToDictionary(x => x.s, x => x.i);

        // Order accidental symbols for maximal munch, with empty
        // string (bare interval) as the last element.
        // This handles absurd chord names, e.g. Cmaj7add###11.
        IReadOnlyList<string> accidentalCombinations = (
            from accidentalSymbol in new[] { Symbols.Sharp, Symbols.Flat }
            from repetitions in Enumerable.Range(1, Numbers.MaxAccidentals).Reverse()
            select string.Concat(Enumerable.Repeat(accidentalSymbol, repetitions)))
            .Append(string.Empty)
            .ToList();

        // Regex sorts elements into four categories.
        //
        // 1) The root symbol defines the note name or Roman interval relative to
        // which the chord's structure is understood (not required, and not used
        // in this method).
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

        void AddExtensionSeriesUpTo(int exclusiveIndex, bool flatSeven, bool diminished = false)
        {
            foreach (string interval in extensionSeries[..exclusiveIndex])
            {
                if (diminished)
                    intervalSet.Add(interval == Symbols.Seven ? Symbols.DoubleFlatSeven : interval);
                else
                    intervalSet.Add(flatSeven && interval == Symbols.Seven ? Symbols.FlatSeven : interval);
            }
        }

        // Bare chords imply major,
        // e.g. "C", "Ab", "G"
        if (main == string.Empty)
        {
            intervalSet.Add(Symbols.Three);

            // Bare chord plus extension implies b7,
            // e.g. "C7", "A11", "Gb13"
            if (extensionIndex.TryGetValue(extension, out int bareIdx))
            {
                AddExtensionSeriesUpTo(bareIdx + Numbers.IndexOffset, flatSeven: true);
                extension = string.Empty;
            }

        }
        // Explicit major chord,
        // e.g. "Cmaj", "BbM", 'F#Δ'
        else if (Symbols.RecognizedMajorSymbols.Contains(main))
        {
            intervalSet.Add(Symbols.Three);

            // Explicit major plus extension implies natural 7,
            // e.g. 'Cmaj7', 'AM7', 'F#Δ7'
            if (extensionIndex.TryGetValue(extension, out int majIdx))
            {
                AddExtensionSeriesUpTo(majIdx + Numbers.IndexOffset, flatSeven: false);
                extension = string.Empty;
            }
        }
        // Diminished chords, e.g. 'Cdim', 'Ao'
        else if (Symbols.RecognizedDiminishedSymbols.Contains(main))
        {
            intervalSet.Add(Symbols.FlatThree);
            intervalSet.Add(Symbols.FlatFive);
            intervalSet.Remove(Symbols.Five);

            // Diminished 7 implies bb7, e.g. 'Cdim7', 'Ao9'
            if (extensionIndex.TryGetValue(extension, out int dimIdx))
            {
                AddExtensionSeriesUpTo(dimIdx + Numbers.IndexOffset, flatSeven: false, diminished: true);
                extension = string.Empty;
            }
        }
        // Augmented chords, e.g. 'Caug', 'A+'
        else if (Symbols.RecognizedAugmentedSymbols.Contains(main))
        {
            intervalSet.Add(Symbols.Three);
            intervalSet.Add(Symbols.SharpFive);
            intervalSet.Remove(Symbols.Five);

        }
        // Half-diminished chords, e.g. 'Cø'
        else if (Symbols.RecognizedHalfDiminishedSymbols.Contains(main))
        {
            intervalSet.Add(Symbols.FlatThree);
            intervalSet.Add(Symbols.FlatFive);
            intervalSet.Remove(Symbols.Five);

        }

        // Minor chords, e.g. 'Cmin', 'Am', 'F#-'
        else if (Symbols.RecognizedMinorSymbols.Contains(main))
        {
            intervalSet.Add(Symbols.FlatThree);
        }

        // Apart from major and diminished chords (above) a 7 implies a flat 7,
        // and a natural 7 must be indicated as a major 7.
        if (extension != string.Empty)
        {
            // e.g. "Gmaj7", "FM9", "DbΔ7"
            foreach (string symb in Symbols.RecognizedMajorSymbols)
            {
                if (extension.Contains(symb))
                {
                    string baseNumber = extension.Replace(symb, string.Empty);
                    if (extensionIndex.TryGetValue(baseNumber, out int idx))
                        AddExtensionSeriesUpTo(idx + Numbers.IndexOffset, flatSeven: false);
                }
            }
            // Any other 7 is a b7,
            // e.g. "Cmin7", "Gaug7"
            if (extensionIndex.TryGetValue(extension, out int flatIdx))
                AddExtensionSeriesUpTo(flatIdx + Numbers.IndexOffset, flatSeven: true);
        }

        // Chords not in one of the above categories may have some kind of
        // special modifier describing their structure.
        List<string> subtractions = [];
        if (modifications != string.Empty)
        {
            foreach (string susInterval in susIntervals)
            {
                string susKey = Symbols.Sus + susInterval;
                if (modifications.Contains(susKey))
                {
                    modifications = modifications.Replace(susKey, string.Empty);
                    intervalSet.Add(susInterval);
                    subtractions.AddRange([Symbols.Three, Symbols.FlatThree]);
                    main = string.Empty;
                }
            }

            // Special chords in our system might have bb7.
            if (modifications.Contains(Symbols.DoubleFlatSeven))
            {
                string noName = Symbols.No + Symbols.DoubleFlatSeven;
                string addName = Symbols.Add + Symbols.DoubleFlatSeven;
                if (modifications.Contains(noName))
                {
                    subtractions.Add(Symbols.DoubleFlatSeven);
                    modifications = modifications.Replace(noName, string.Empty);
                }
                else if (modifications.Contains(addName))
                {
                    intervalSet.Add(Symbols.DoubleFlatSeven);
                    modifications = modifications.Replace(addName, string.Empty);
                }
                else
                {
                    intervalSet.Add(Symbols.DoubleFlatSeven);
                    modifications = modifications.Replace(Symbols.DoubleFlatSeven, string.Empty);
                }
            }

            // Most remaining symbols will be a bare interval, an addition, or
            // subtraction. 
            var numberRange = Enumerable.Range(2, Numbers.NotesInOctave * 2).Reverse();
            foreach (int number in numberRange)
            {
                if (modifications == string.Empty)
                    break;

                foreach (string accidental in accidentalCombinations)
                {
                    string interval = accidental + number.ToString();
                    if (modifications.Contains(Symbols.Add + interval))
                    {
                        intervalSet.Add(interval);
                        modifications = modifications.Replace(Symbols.Add + interval, string.Empty);
                    }
                    else if (modifications.Contains(Symbols.No + interval))
                    {
                        intervalSet.Remove(interval);
                        modifications = modifications.Replace(Symbols.No + interval, string.Empty);
                    }
                    else if (modifications.Contains(interval))
                    {
                        intervalSet.Add(interval);
                        modifications = modifications.Replace(interval, string.Empty);
                        if ((new[] { Symbols.SharpFive, Symbols.FlatFive }).Contains(interval))
                            intervalSet.Remove(Symbols.Five);
                    }
                }
            }
            // Augmented symbols are either main (already handled) or
            // modification (handled now)
            // e.g. Caug7 vs. C7aug
            foreach (string aug in Symbols.RecognizedAugmentedSymbols)
                if (modifications.Contains(aug))
                {
                    intervalSet.Remove(Symbols.Five);
                    intervalSet.Add(Symbols.SharpFive);
                }
        }
        //  Subtractions are treated last in case they depend on
        //  an implication in a preceding symbol.
        foreach (string s in subtractions)
            intervalSet.Remove(s);

        return CreateNew(intervalSet);
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



/// <summary>
/// Optimized replacement for IntervalStructure for testing purposes.
/// Key changes:
///   - ToChordSymbol uses a local HashSet(string) instead of copy-on-write
///     IntervalStructure mutations to track remaining intervals.
///   - FromChordSymbol uses a local HashSet(string) to accumulate the result
///     and constructs a single IntervalStructureV2 at the end.
///   - accidentalCombinations is materialized once outside the inner loop.
///   - Extension series lookups use a Dictionary for O(1) index retrieval.
///   - Redundant Contains-before-Remove guards are eliminated.
///   - Count uses ImmutableHashSet.Count directly.
/// </summary>
public class IntervalStructureV2 : ValueObject, IReadOnlySet<Interval>, ISymbolSerializable<IntervalStructureV2>
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

    public static IntervalStructureV2 Diatonic { get; } = CreateAsHeptatonic(ScalePatternsHeptatonic.Diatonic);
    public static IntervalStructureV2 Altered { get; } = CreateAsHeptatonic(ScalePatternsHeptatonic.Altered);

    private IntervalStructureV2(IEnumerable<Interval> intervals)
    {
        Intervals = [.. intervals];
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

    // Factory Methods ////////////////////////////////////////////////////////

    /// <summary>
    /// Create a new instance from an enumerable of Intervals.
    /// </summary>
    /// <exception cref="ArgumentException">If the structure contains
    /// a duplicate or overlapping interval.</exception>
    public static IntervalStructureV2 CreateNew(IEnumerable<Interval> intervals)
    {
        List<Interval> sanitized = [];
        foreach (Interval interval in intervals)
        {
            if (sanitized.Contains(interval) || sanitized.Select(i => i.Value).Contains(interval.Value))
                throw new IntervalStructureHasDuplicates(intervals.Select(i => i.Name.ToSymbol()));
            sanitized.Add(interval);
        }
        return new IntervalStructureV2(sanitized);
    }

    /// <summary>
    /// Create a new interval structure from a collection of interval symbols.
    /// </summary>
    public static IntervalStructureV2 CreateNew(IEnumerable<string> intervalNames)
        => CreateNew(intervalNames.Select(n => Interval.FromSymbol(n)));

    public static IntervalStructureV2 CreateNew()
        => CreateNew([Symbols.One]);

    /// <summary>
    /// Create an interval structure on the assumption that the given
    /// integers resolve to a heptatonic scale.
    /// </summary>
    /// <exception cref="ArgumentException">If the structure is not a heptatonic octave.</exception>
    public static IntervalStructureV2 CreateAsHeptatonic(IEnumerable<int> intervals)
    {
        HashSet<int> unique = [Numbers.Zero, .. intervals.Select(x => x % Numbers.SemitonesInOctave)];
        if (unique.Count != Numbers.NotesInOctave)
        {
            throw new DomainException("Intervals must represent a heptatonic structure (have 7 unique intervals, including the unison).");
        }
        intervals = intervals.Order();
        List<Interval> sanitizedIntervals = [
            .. intervals.Select(
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
        => Intervals.Count;

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

    // Set-like Behaviors /////////////////////////////////////////////////////

    public bool IsModalSupersetOf(IEnumerable<Interval> intervals)
    {
        foreach (var mode in Inversions)
        {
            if (mode.IsSupersetOf(intervals))
                return true;
        }
        return false;
    }

    // Copy-on-Write Mutations ////////////////////////////////////////////////

    public IntervalStructureV2 Remove(Interval interval)
    {
        List<Interval> result = [];
        foreach (Interval i in Intervals)
        {
            if (!i.Equals(interval) || interval.Value == Numbers.Zero)
                result.Add(i);
        }
        return CreateNew(result);
    }

    public IntervalStructureV2 Remove(string interval)
    {
        List<Interval> result = [];
        foreach (Interval i in Intervals)
        {
            if (i.Name.ToSymbol() != interval || i.Value == Numbers.Zero)
                result.Add(i);
        }
        return CreateNew(result);
    }

    public IntervalStructureV2 Remove(params string[] intervals)
    {
        List<Interval> result = [];
        foreach (Interval i in Intervals)
        {
            if (!intervals.Contains(i.Name.ToSymbol()) || i.Value == Numbers.Zero)
                result.Add(i);
        }
        return CreateNew(result);
    }

    public IntervalStructureV2 Add(Interval interval)
        => CreateNew([interval, .. Intervals]);

    public IntervalStructureV2 Add(string interval)
        => CreateNew([Interval.FromSymbol(interval), .. Intervals]);

    // Chord Symbol API ///////////////////////////////////////////////////////

    /// <summary>
    /// Translate this interval structure into a chord symbol string.
    ///
    /// Optimization over the original: instead of tracking consumed intervals
    /// via copy-on-write IntervalStructure mutations (each of which rebuilds an
    /// ImmutableHashSet, re-sorts names, and re-evaluates all boolean
    /// properties), this implementation maintains a local HashSet(string) seeded
    /// from IntervalSymbols. All Contains and Remove operations on the working
    /// set are O(1). Control-flow properties (IsDominant, Third, etc.) are read
    /// from `this` as before, since they are not affected by the working set.
    /// </summary>
    public string ToChordSymbol((string? MajorSymbol, string? MinorSymbol, string? DiminishedSymbol)? style = null)
    {
        string MajorSymbol = style?.MajorSymbol ?? Symbols.Major;
        string MinorSymbol = style?.MinorSymbol ?? Symbols.Minor;
        string DiminishedSymbol = style?.DiminishedSymbol ?? Symbols.Diminished;

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

        // A local HashSet<string> replaces the copy-on-write IntervalStructure
        // that the original used to track remaining intervals. All Contains and
        // Remove operations here are O(1) hash operations rather than O(n log n)
        // full object reconstructions.
        HashSet<string> remaining = new(IntervalSymbols);

        ///////////////////////////////////////////////////////////////////////
        // Diminished Chord
        ///////////////////////////////////////////////////////////////////////
        if (IsDiminished)
        {
            normal3 = DiminishedSymbol;
            primary = Symbols.Seven;
            remaining.Remove(Symbols.DoubleFlatSeven);
            remaining.Remove(Symbols.FlatFive);
            remaining.Remove(Symbols.FlatThree);
        }
        else
        {
            if (remaining.Contains(Symbols.DoubleFlatSeven))
            {
                alt7 = Symbols.DoubleFlatSeven;
                remaining.Remove(Symbols.DoubleFlatSeven);
            }

            if (AlteredFifth is Interval fifth)
            {
                // Cache the symbol string to avoid calling ToSymbol() twice.
                alt5 = fifth.Name.ToSymbol();
                remaining.Remove(alt5);
            }
            else if (!HasPerfectFifth)
            {
                no5 = Symbols.No + Symbols.Five;
            }
            else
            {
                remaining.Remove(Symbols.Five);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Dominant Chord
        ///////////////////////////////////////////////////////////////////////
        if (IsDominant)
        {
            primary = Symbols.Seven;
            remaining.Remove(Symbols.Three);
            remaining.Remove(Symbols.FlatSeven);
        }

        ///////////////////////////////////////////////////////////////////////
        // Chords with a third
        ///////////////////////////////////////////////////////////////////////
        if (Third is Interval n)
        {
            if (!(IsDiminished || IsDominant))
            {
                // Cache symbol to avoid repeated ToSymbol() calls.
                string thirdSymbol = n.Name.ToSymbol();

                if (thirdSymbol == Symbols.Three)
                {
                    normal3 = MajorSymbol;
                    if (remaining.Contains(Symbols.Seven))
                    {
                        primary = Symbols.Seven;
                        remaining.Remove(Symbols.Seven);
                    }
                }
                else if (thirdSymbol == Symbols.FlatThree)
                {
                    normal3 = MinorSymbol;

                    // Minor major 7
                    if (remaining.Contains(Symbols.Seven))
                    {
                        primary = MajorSymbol + Symbols.Seven;
                        remaining.Remove(Symbols.Seven);
                    }

                    // Minor 7 — preserved as two separate ifs to match original behavior
                    if (remaining.Contains(Symbols.FlatSeven))
                    {
                        primary = Symbols.Seven;
                        remaining.Remove(Symbols.FlatSeven);
                    }
                }

                remaining.Remove(thirdSymbol);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Sus Chords
        ///////////////////////////////////////////////////////////////////////
        else if (Suspensions.Count > Numbers.Zero)
        {
            Interval mainSus = Suspensions[Numbers.Zero];
            string mainSusSymbol = mainSus.Name.ToSymbol();
            sus = Symbols.Sus + mainSusSymbol;
            remaining.Remove(mainSusSymbol);

            foreach (Interval x in Suspensions[1..])
            {
                string xSymbol = x.Name.ToSymbol();
                add += Symbols.Add + xSymbol;
                remaining.Remove(xSymbol);
            }

            if (remaining.Contains(Symbols.Seven))
            {
                primary = MajorSymbol + Symbols.Seven;
                remaining.Remove(Symbols.Seven);
            }
            else if (remaining.Contains(Symbols.FlatSeven))
            {
                primary = Symbols.Seven;
                remaining.Remove(Symbols.FlatSeven);
            }
        }
        else
        {
            no3 = Symbols.No + Symbols.Three;
        }

        ///////////////////////////////////////////////////////////////////////
        // Primary Extensions
        ///////////////////////////////////////////////////////////////////////
        string[] NaturalExtensions = [Symbols.Nine, Symbols.Eleven, Symbols.Thirteen];
        string largest = string.Empty;

        if (primary != string.Empty)
        {
            List<string> checkedExtensions = [];
            foreach ((string extension, int index) in NaturalExtensions.Select((e, i) => (e, i)))
            {
                string? prev = index > Numbers.Zero ? NaturalExtensions[index - Numbers.IndexOffset] : null;
                if (remaining.Contains(extension))
                {
                    if (prev is null || checkedExtensions.Contains(prev))
                    {
                        largest = extension;
                        remaining.Remove(extension);
                        checkedExtensions.Add(extension);
                    }
                    else
                    {
                        add += Symbols.Add + extension;
                        remaining.Remove(extension);
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
        // Secondary Extension (6)
        ///////////////////////////////////////////////////////////////////////
        if (remaining.Contains(Symbols.Six))
        {
            remaining.Remove(Symbols.Six);
            if (primary != string.Empty)
                add += Symbols.Add + Symbols.Six;
            else
                secondary = Symbols.Six;
        }

        ///////////////////////////////////////////////////////////////////////
        // Tertiary Extensions: natural ones get an "add" prefix; altered ones
        // are written bare. Iterate over a snapshot of the remaining set since
        // we are removing while we enumerate.
        ///////////////////////////////////////////////////////////////////////
        foreach (string s in remaining.Where(s => s != Symbols.One).ToList())
        {
            Interval ext = Interval.FromSymbol(s);
            if (ext.Accidental == Numbers.Zero)
            {
                add += Symbols.Add + s;
                remaining.Remove(s);
            }
        }

        // Whatever is left (excluding unison) is an altered extension. Sort
        // for consistent output since HashSet does not preserve insertion order.
        extensions = string.Join(
            string.Empty,
            NomenclatureHelper.SortIntervalNames(remaining.Where(s => s != Symbols.One))
        );

        ///////////////////////////////////////////////////////////////////////
        // Final Symbol
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

    /// <summary>
    /// Parse a chord symbol into its implied intervals.
    ///
    /// Optimizations over the original:
    ///   - A local HashSet(string) accumulates the interval set throughout, and
    ///     a single IntervalStructureV2 is constructed at the very end, rather
    ///     than performing ~20+ copy-on-write object creations mid-parse.
    ///   - accidentalCombinations is materialized once before the outer loop
    ///     instead of being re-evaluated as a deferred LINQ query on every
    ///     iteration of that loop.
    ///   - Extension series index lookups use a Dictionary for O(1) retrieval
    ///     instead of List.IndexOf.
    ///   - The redundant Contains guard before Remove at the end of the method
    ///     is removed; HashSet.Remove is a safe no-op when the key is absent.
    /// </summary>
    public static IntervalStructureV2 FromChordSymbol(string chordSymbol)
    {
        // Accumulate the interval set as strings throughout, and construct a
        // single IntervalStructureV2 at the end.
        HashSet<string> intervalSet = [Symbols.One, Symbols.Five];

        List<string> extensionSeries = [Symbols.Seven, Symbols.Nine, Symbols.Eleven, Symbols.Thirteen];

        // O(1) index lookup replaces O(n) List.IndexOf across all branches.
        Dictionary<string, int> extensionIndex = extensionSeries
            .Select((s, i) => (s, i))
            .ToDictionary(x => x.s, x => x.i);

        List<string> susIntervals = [Symbols.DoubleFlatThree, Symbols.SharpThree, Symbols.Four, Symbols.Two];

        // Materialize the accidental combinations once. In the original, this
        // was a deferred LINQ query reconstructed on every iteration of the
        // outer number loop (up to ~16 times per method call).
        IReadOnlyList<string> accidentalCombinations = (
            from accidentalSymbol in new[] { Symbols.Sharp, Symbols.Flat }
            from repetitions in Enumerable.Range(1, Numbers.MaxAccidentals).Reverse()
            select string.Concat(Enumerable.Repeat(accidentalSymbol, repetitions)))
            .Append(string.Empty)
            .ToList();

        Match match = RegularExpressions.ChordSymbol.Match(chordSymbol);
        if (!match.Success)
        {
            match = RegularExpressions.RootlessChordSymbol.Match(chordSymbol);
            if (!match.Success)
                throw new InvalidAsSymbol(chordSymbol, nameof(chordSymbol));
        }

        string main = match.Groups.GetValueOrDefault(Keywords.Main)?.Value ?? string.Empty;
        string extension = match.Groups.GetValueOrDefault(Keywords.Extension)?.Value ?? string.Empty;
        string modifications = match.Groups.GetValueOrDefault(Keywords.Modification)?.Value ?? string.Empty;

        // Local helper: add extensions from the series up to (exclusive) index
        // i. The `flatSeven` flag controls whether the 7 slot uses b7 or
        // natural 7. Eliminates the repeated slicing boilerplate across the
        // major/minor/diminished branches.
        void AddExtensionSeriesUpTo(int exclusiveIndex, bool flatSeven, bool diminished = false)
        {
            foreach (string interval in extensionSeries[..exclusiveIndex])
            {
                if (diminished)
                    intervalSet.Add(interval == Symbols.Seven ? Symbols.DoubleFlatSeven : interval);
                else
                    intervalSet.Add(flatSeven && interval == Symbols.Seven ? Symbols.FlatSeven : interval);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Main symbol branch
        ///////////////////////////////////////////////////////////////////////

        // Bare chord implies major.
        if (main == string.Empty)
        {
            intervalSet.Add(Symbols.Three);

            // Bare chord plus numeric extension implies b7.
            if (extensionIndex.TryGetValue(extension, out int bareIdx))
            {
                AddExtensionSeriesUpTo(bareIdx + Numbers.IndexOffset, flatSeven: true);
                extension = string.Empty;
            }
        }
        else if (Symbols.RecognizedMajorSymbols.Contains(main))
        {
            intervalSet.Add(Symbols.Three);

            // Explicit major plus extension implies natural 7.
            if (extensionIndex.TryGetValue(extension, out int majIdx))
            {
                AddExtensionSeriesUpTo(majIdx + Numbers.IndexOffset, flatSeven: false);
                extension = string.Empty;
            }
        }
        else if (Symbols.RecognizedDiminishedSymbols.Contains(main))
        {
            intervalSet.Add(Symbols.FlatThree);
            intervalSet.Add(Symbols.FlatFive);
            intervalSet.Remove(Symbols.Five);

            if (extensionIndex.TryGetValue(extension, out int dimIdx))
            {
                AddExtensionSeriesUpTo(dimIdx + Numbers.IndexOffset, flatSeven: false, diminished: true);
                extension = string.Empty;
            }
        }
        else if (Symbols.RecognizedAugmentedSymbols.Contains(main))
        {
            intervalSet.Add(Symbols.Three);
            intervalSet.Add(Symbols.SharpFive);
            intervalSet.Remove(Symbols.Five);
        }
        else if (Symbols.RecognizedHalfDiminishedSymbols.Contains(main))
        {
            intervalSet.Add(Symbols.FlatThree);
            intervalSet.Add(Symbols.FlatFive);
            intervalSet.Remove(Symbols.Five);
        }
        else if (Symbols.RecognizedMinorSymbols.Contains(main))
        {
            intervalSet.Add(Symbols.FlatThree);
        }

        ///////////////////////////////////////////////////////////////////////
        // Remaining extension symbol (not consumed above)
        ///////////////////////////////////////////////////////////////////////
        if (extension != string.Empty)
        {
            foreach (string symb in Symbols.RecognizedMajorSymbols)
            {
                if (extension.Contains(symb))
                {
                    string baseNumber = extension.Replace(symb, string.Empty);
                    if (extensionIndex.TryGetValue(baseNumber, out int idx))
                        AddExtensionSeriesUpTo(idx + Numbers.IndexOffset, flatSeven: false);
                }
            }

            if (extensionIndex.TryGetValue(extension, out int flatIdx))
                AddExtensionSeriesUpTo(flatIdx + Numbers.IndexOffset, flatSeven: true);
        }

        ///////////////////////////////////////////////////////////////////////
        // Modification string
        ///////////////////////////////////////////////////////////////////////
        List<string> subtractions = [];

        if (modifications != string.Empty)
        {
            // Sus modifiers
            foreach (string susInterval in susIntervals)
            {
                string susKey = Symbols.Sus + susInterval;
                if (modifications.Contains(susKey))
                {
                    modifications = modifications.Replace(susKey, string.Empty);
                    intervalSet.Add(susInterval);
                    subtractions.AddRange([Symbols.Three, Symbols.FlatThree]);
                    main = string.Empty;
                }
            }

            // Double-flat seven handling
            if (modifications.Contains(Symbols.DoubleFlatSeven))
            {
                string noName = Symbols.No + Symbols.DoubleFlatSeven;
                string addName = Symbols.Add + Symbols.DoubleFlatSeven;

                if (modifications.Contains(noName))
                {
                    subtractions.Add(Symbols.DoubleFlatSeven);
                    modifications = modifications.Replace(noName, string.Empty);
                }
                else if (modifications.Contains(addName))
                {
                    intervalSet.Add(Symbols.DoubleFlatSeven);
                    modifications = modifications.Replace(addName, string.Empty);
                }
                else
                {
                    intervalSet.Add(Symbols.DoubleFlatSeven);
                    modifications = modifications.Replace(Symbols.DoubleFlatSeven, string.Empty);
                }
            }

            // General interval parsing — accidentalCombinations is already
            // materialized above; the inner foreach does not trigger any
            // LINQ evaluation here.
            var numberRange = Enumerable.Range(2, Numbers.NotesInOctave * 2).Reverse();
            foreach (int number in numberRange)
            {
                if (modifications == string.Empty)
                    break;

                foreach (string accidental in accidentalCombinations)
                {
                    string interval = accidental + number.ToString();

                    if (modifications.Contains(Symbols.Add + interval))
                    {
                        intervalSet.Add(interval);
                        modifications = modifications.Replace(Symbols.Add + interval, string.Empty);
                    }
                    else if (modifications.Contains(Symbols.No + interval))
                    {
                        intervalSet.Remove(interval);
                        modifications = modifications.Replace(Symbols.No + interval, string.Empty);
                    }
                    else if (modifications.Contains(interval))
                    {
                        intervalSet.Add(interval);
                        modifications = modifications.Replace(interval, string.Empty);
                        if ((new[] { Symbols.SharpFive, Symbols.FlatFive }).Contains(interval))
                            intervalSet.Remove(Symbols.Five);
                    }
                }
            }

            // Augmented as modification (e.g. C7aug)
            foreach (string aug in Symbols.RecognizedAugmentedSymbols)
            {
                if (modifications.Contains(aug))
                {
                    intervalSet.Remove(Symbols.Five);
                    intervalSet.Add(Symbols.SharpFive);
                }
            }
        }

        // Apply subtractions last. HashSet.Remove is a no-op when the key is
        // absent, so the redundant Contains guard from the original is dropped.
        foreach (string s in subtractions)
            intervalSet.Remove(s);

        // Single construction point: one IntervalStructureV2 created per call,
        // replacing the original's pattern of ~20+ intermediate objects.
        return CreateNew(intervalSet);
    }

    // ISymbolSerializable ////////////////////////////////////////////////////

    public string ToSymbol() => ToChordSymbol();

    public static IntervalStructureV2 FromSymbol(string symbol) => FromChordSymbol(symbol);

    // ValueObject ////////////////////////////////////////////////////////////

    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (string symbol in IntervalSymbols)
            yield return symbol;
    }

    public Interval this[int index] => throw new NotImplementedException();
}
