using System.Text.RegularExpressions;

using Aristoxenus.Core.Shared.Data;
using Aristoxenus.Core.Shared.Errors;

namespace Aristoxenus.Core.Shared.Utilities;

public static partial class SymbolHelper
{
    /// <summary>
    /// Parse a chord symbol into its implied intervals.
    /// </summary>
    /// <param name="chordSymbol">A chord symbol, with or without a 
    /// root symbol (i.e. e.g. either "Cmaj7#11" or "maj7#11").</param>
    /// <returns></returns>
    /// <exception cref="InvalidAsSymbol">If the symbol cannot be parsed.
    /// </exception>
    public static HashSet<(int, int)> ParseChordSymbol(string chordSymbol)
    {
        HashSet<(int, int)> intervalSet = [(1, 0), (5, 0)];
        List<(int, int)> susIntervals = [(3, -2), (3, -1), (4, 0), (2, 0)];
        List<(int, int)> extensionSeries = [(7, 0), (9, 0), (11, 0), (13, 0)];
        Dictionary<string, int> extensionIndex = extensionSeries
            .Select((s, i) => (s, i))
            .ToDictionary(x => CreateIntervalNameSymbol(x.s), x => x.i);

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
            foreach ((int, int) interval in extensionSeries[..exclusiveIndex])
            {
                if (diminished)
                    intervalSet.Add(interval == (7, 0) ? (7, -2) : interval);
                else
                    intervalSet.Add(flatSeven && interval == (7, 0) ? (7, -1) : interval);
            }
        }

        // Bare chords imply major,
        // e.g. "C", "Ab", "G"
        if (main == string.Empty)
        {
            intervalSet.Add((3, 0));

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
            intervalSet.Add((3, 0));

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
            intervalSet.Add((3, -1));
            intervalSet.Add((5, -1));
            intervalSet.Remove((5, 0));

            // Diminished 7 implies bb7, e.g. 'Cdim7', 'Ao9'
            if (extensionIndex.TryGetValue(extension, out int dimIdx))
            {
                AddExtensionSeriesUpTo(dimIdx + Numbers.IndexOffset, flatSeven: false, diminished: true);
                extension = string.Empty;
            }
        }
        // Augmented chords, e.g. 'Caug', 'A+'
        // (NB: our system always generates 'maj#5' instead of using an
        // augmented symbol)
        else if (Symbols.RecognizedAugmentedSymbols.Contains(main))
        {
            intervalSet.Add((3, 0));
            intervalSet.Add((5, 1));
            intervalSet.Remove((5, 0));

        }
        // Half-diminished chords, e.g. 'Cø'
        // (NB: our system always generates 'min7b5' instead of using a
        // half diminished symbol)
        else if (Symbols.RecognizedHalfDiminishedSymbols.Contains(main))
        {
            intervalSet.Add((3, -1));
            intervalSet.Add((5, -1));
            intervalSet.Remove((5, 0));

        }

        // Minor chords, e.g. 'Cmin', 'Am', 'F#-'
        else if (Symbols.RecognizedMinorSymbols.Contains(main))
        {
            intervalSet.Add((3, -1));
        }

        // Apart from major and diminished chords (above), a 7 implies a flat 7
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
        List<(int, int)> subtractions = [];
        if (modifications != string.Empty)
        {
            foreach ((int, int) susInterval in susIntervals)
            {
                string susKey = Symbols.Sus + CreateIntervalNameSymbol(susInterval);
                if (modifications.Contains(susKey))
                {
                    modifications = modifications.Replace(susKey, string.Empty);
                    intervalSet.Add(susInterval);
                    // Any explicit 'sus' overrides an explicit or implicit 3 or b3.
                    subtractions.AddRange([(3, 0), (3, -1)]);
                    main = string.Empty;
                }
            }

            // Special chords in our system might have 'bb7'.
            if (modifications.Contains(Symbols.DoubleFlatSeven))
            {
                string noName = Symbols.No + Symbols.DoubleFlatSeven;
                string addName = Symbols.Add + Symbols.DoubleFlatSeven;
                if (modifications.Contains(noName))
                {
                    subtractions.Add((7, -2));
                    modifications = modifications.Replace(noName, string.Empty);
                }
                else if (modifications.Contains(addName))
                {
                    intervalSet.Add((7, -2));
                    modifications = modifications.Replace(addName, string.Empty);
                }
                else
                {
                    intervalSet.Add((7, -2));
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
                    // "addX" symbol
                    if (modifications.Contains(Symbols.Add + interval))
                    {
                        intervalSet.Add(ParseIntervalNameSymbol(interval));
                        modifications = modifications.Replace(Symbols.Add + interval, string.Empty);
                    }
                    // "noX" symbol
                    else if (modifications.Contains(Symbols.No + interval))
                    {
                        intervalSet.Remove(ParseIntervalNameSymbol(interval));
                        modifications = modifications.Replace(Symbols.No + interval, string.Empty);
                    }
                    // "X" bare interval symbol
                    else if (modifications.Contains(interval))
                    {
                        intervalSet.Add(ParseIntervalNameSymbol(interval));
                        modifications = modifications.Replace(interval, string.Empty);
                        // Any explicit altered fifth overrides an explicit or implicit
                        // perfect fifth
                        if ((new[] { Symbols.SharpFive, Symbols.FlatFive }).Contains(interval))
                            intervalSet.Remove((5, 0));
                    }
                }
            }
            // Augmented symbols are expressed either as the main symbol
            // (e.g. "Caug7", already handled) or as a modification
            // (handled now, "C7aug")
            foreach (string aug in Symbols.RecognizedAugmentedSymbols)
                if (modifications.Contains(aug))
                {
                    intervalSet.Remove((5, 0));
                    intervalSet.Add((5, 1));
                }
        }
        // Subtractions are treated last. Any explicit or implicit symbol
        // can be negated by a corresponding "noX"; the order of the
        // symbols does not affect this (e.g. "Cmin6no6add6" produces the
        // same intervals as "Cmin6no6" or "Cminadd6no6", etc.)
        foreach ((int, int) s in subtractions)
            intervalSet.Remove(s);

        return intervalSet;
    }



    public static HashSet<string> GetIntervalsFromChordSymbolBACKUP(string chordSymbol)
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
        // (NB: our system always generates 'maj#5' instead of using an
        // augmented symbol)
        else if (Symbols.RecognizedAugmentedSymbols.Contains(main))
        {
            intervalSet.Add(Symbols.Three);
            intervalSet.Add(Symbols.SharpFive);
            intervalSet.Remove(Symbols.Five);

        }
        // Half-diminished chords, e.g. 'Cø'
        // (NB: our system always generates 'min7b5' instead of using a
        // half diminished symbol)
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

        // Apart from major and diminished chords (above), a 7 implies a flat 7
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
                    // Any explicit 'sus' overrides an explicit or implicit 3 or b3.
                    subtractions.AddRange([Symbols.Three, Symbols.FlatThree]);
                    main = string.Empty;
                }
            }

            // Special chords in our system might have 'bb7'.
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
                    // "addX" symbol
                    if (modifications.Contains(Symbols.Add + interval))
                    {
                        intervalSet.Add(interval);
                        modifications = modifications.Replace(Symbols.Add + interval, string.Empty);
                    }
                    // "noX" symbol
                    else if (modifications.Contains(Symbols.No + interval))
                    {
                        intervalSet.Remove(interval);
                        modifications = modifications.Replace(Symbols.No + interval, string.Empty);
                    }
                    // "X" bare interval symbol
                    else if (modifications.Contains(interval))
                    {
                        intervalSet.Add(interval);
                        modifications = modifications.Replace(interval, string.Empty);
                        // Any explicit altered fifth overrides an explicit or implicit
                        // perfect fifth
                        if ((new[] { Symbols.SharpFive, Symbols.FlatFive }).Contains(interval))
                            intervalSet.Remove(Symbols.Five);
                    }
                }
            }
            // Augmented symbols are expressed either as the main symbol
            // (e.g. "Caug7", already handled) or as a modification
            // (handled now, "C7aug")
            foreach (string aug in Symbols.RecognizedAugmentedSymbols)
                if (modifications.Contains(aug))
                {
                    intervalSet.Remove(Symbols.Five);
                    intervalSet.Add(Symbols.SharpFive);
                }
        }
        // Subtractions are treated last. Any explicit or implicit symbol
        // can be negated by a corresponding "noX"; the order of the
        // symbols does not affect this (e.g. "Cmin6no6add6" produces the
        // same intervals as "Cmin6no6" or "Cminadd6no6", etc.)
        foreach (string s in subtractions)
            intervalSet.Remove(s);

        return intervalSet;
    }


    /// <summary>
    /// Translate a collection of intervals into a chord symbol string.
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
    public static string CreateChordSymbol(IEnumerable<(int degree, int accidental)> intervalSymbols, (string? MajorSymbol, string? MinorSymbol, string? DiminishedSymbol)? style = null)
    {
        // These values represent the base symbols that will be used 
        // for basic chord categories: Cmaj vs CM vs CΔ, etc. We don't 
        // check these, so it's possible to make chord symbols that our
        // own parse method won't actually recognize (many common variations
        // will be recognized: maj|M|Δ|min|m|-|dim|o|aug|+|ø).
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

        HashSet<(int, int)> remaining = [.. intervalSymbols];

        // Set up data for the main features of the interval structure,
        // which are mapped to strings to create a chord symbol.
        bool IsDominant = remaining.IsSupersetOf([(3, 0), (7, -1)]);
        bool IsDiminished = remaining.IsSupersetOf([(3, -1), (5, -1), (7, -2)]);
        bool HasPerfectFifth = false;
        (int, int)? Third = null;
        (int, int)? AlteredFifth = null;
        List<(int, int)> Suspensions = [];
        foreach ((int Degree, int Accidental) interval in intervalSymbols)
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

        ///////////////////////////////////////////////////////////////////////
        // Diminished Chord: Special structure on all notes.
        ///////////////////////////////////////////////////////////////////////
        if (IsDiminished)
        {
            normal3 = DiminishedSymbol;
            primary = Symbols.Seven;
            remaining.Remove((7, -2));
            remaining.Remove((5, -1));
            remaining.Remove((3, -1));
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
            if (remaining.Contains((7, -2)))
            {
                alt7 = Symbols.DoubleFlatSeven;
                remaining.Remove((7, -2));
            }
            // A non-diminished chord with an altered 5th must always have
            // an explicit alt5 symbol.
            if (AlteredFifth is (int, int) fifth)
            {
                alt5 = CreateIntervalNameSymbol(fifth);
                remaining.Remove(fifth);
            }
            // A chord with no 5th must have an explicit no5 symbol.
            else if (!HasPerfectFifth)
                no5 = Symbols.No + Symbols.Five;
            // The fifth is implied in any other chord and has no symbol.
            else
                remaining.Remove((5, 0));
        }

        ///////////////////////////////////////////////////////////////////////
        // Dominant Chord: The 7 symbol is used to represent a b7 interval.
        ///////////////////////////////////////////////////////////////////////
        if (IsDominant)
        {
            primary = Symbols.Seven;
            remaining.Remove((3, 0));
            remaining.Remove((7, -1));
        }

        ///////////////////////////////////////////////////////////////////////
        // Chords with a third: We always write 'maj' or 'min' (or whatever
        // default the user chose) to indicate the third of a chord, unless
        // some other symbol already implies it (i.e. diminished and dominant).
        ///////////////////////////////////////////////////////////////////////
        if (Third is (int, int) n)
        {
            // Dominant and diminished chords do not have a symbol
            // for thier third.
            if (!(IsDiminished || IsDominant))
            {
                // Major chord:
                if (n == (3, 0))
                {
                    normal3 = MajorSymbol;
                    // Major 7: The 7 symbol is used to represent a natural
                    // 7 interval.
                    if (remaining.Contains((7, 0)))
                    {
                        primary = Symbols.Seven;
                        remaining.Remove((7, 0));
                    }
                }

                // Minor chord:
                else if (n == (3, -1))
                {
                    normal3 = MinorSymbol;

                    // Minor major 7: the natural 7 is symbolized with a
                    // special symbol.
                    if (remaining.Contains((7, 0)))
                    {
                        primary = MajorSymbol + Symbols.Seven;
                        remaining.Remove((7, 0));
                    }

                    // Minor 7: The 7 symbol is used to represent a b7 interval.
                    if (remaining.Contains((7, -1)))
                    {
                        primary = Symbols.Seven;
                        remaining.Remove((7, -1));
                    }
                }
                remaining.Remove(n);
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
            (int, int) main = Suspensions[Numbers.Zero];
            string mainSus = CreateIntervalNameSymbol(main);
            sus = Symbols.Sus + mainSus;
            remaining.Remove(main);
            foreach ((int, int) x in Suspensions[1..])
            {
                string xSymb = CreateIntervalNameSymbol(x);
                add += Symbols.Add + xSymb;
                remaining.Remove(x);
            }
            if (remaining.Contains((7, 0)))
            {
                primary = MajorSymbol + Symbols.Seven;
                remaining.Remove((7, 0));
            }
            else if (remaining.Contains((7, -1)))
            {
                primary = Symbols.Seven;
                remaining.Remove((7, -1));
            }
        }
        // Any chord without a 3 and without a sus must have a no3 symbol
        else
            no3 = Symbols.No + Symbols.Three;

        ///////////////////////////////////////////////////////////////////////////
        // Primary Extension: Our system treats primary extension symbols as
        // implying ALL previous extensions in the series, i.e. a 13 chord
        // includes 7, 9, 11, 13. If this series is interrupted by a missing or
        // altered note, then the primary extension is the last continuous
        // extension, and any following natural extensions are treated as
        // additions (e.g. C9#11add13).
        ///////////////////////////////////////////////////////////////////////////
        (int, int)[] NaturalExtensions = [(9, 0), (11, 0), (13, 0)];
        (int, int) largest = (0, 0);
        if (primary != string.Empty)
        {
            List<(int, int)> checkedExtensions = [];
            foreach (((int degree, int) extension, int index) in NaturalExtensions.Select((e, i) => (e, i)))
            {
                (int, int)? prev = index > Numbers.Zero ? NaturalExtensions[index - Numbers.IndexOffset] : null;
                if (remaining.Contains(extension))
                {
                    if (prev is null || checkedExtensions.Contains(((int, int))prev))
                    {
                        largest = extension;
                        remaining.Remove(extension);
                        checkedExtensions.Add(extension);
                    }
                    else
                    {
                        add += Symbols.Add + CreateDegreeSymbol(extension.degree);
                        remaining.Remove(extension);
                        checkedExtensions.Add(extension);
                    }
                }
            }
            if (largest != (0, 0))
            {
                primary = primary.Replace(Symbols.Seven, CreateIntervalNameSymbol(largest));
            }
        }
        ///////////////////////////////////////////////////////////////////////
        // Secondary Extension: For chords where a 6 replaces a 7. In this 
        // case, only the natural 6 is ever used; any other type of 6 will be
        // written as an addition. In cases where a primary suffix also
        // exists, the secondary suffix becomes an addition.
        ///////////////////////////////////////////////////////////////////////
        if (remaining.Contains((6, 0)))
        {
            remaining.Remove((6, 0));

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
        foreach ((int Degree, int Accidental) extension in remaining.Where(i => i != (1, 0)))
        {
            if (extension.Accidental == Numbers.Zero)
            {
                add += Symbols.Add + CreateIntervalNameSymbol(extension);
                remaining.Remove(extension);
            }
        }
        // Altered Extensions: Any extension with an accidental does not need
        // an "add" symbol (and if anything is harder to read if it has one,
        // e.g. C7#11 vs C7add#11).
        extensions = string.Join(
            string.Empty, 
            NomenclatureHelper
                .SortIntervalNames(remaining.Where(s => s != (1, 0)).Select(s => CreateIntervalNameSymbol(s)))
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


}
