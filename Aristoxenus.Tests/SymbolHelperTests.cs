
using Aristoxenus.Core.Utilities;

namespace Aristoxenus.Tests
{
    [TestClass]
    public sealed class SymbolHelperTests
    {
        [TestMethod]
        [DataRow("Cmaj", new[] { "1", "3", "5" })]
        [DataRow("Cmaj7", new[] { "1", "3", "5", "7" })]
        [DataRow("CM7", new[] { "1", "3", "5", "7" })]
        [DataRow("Cm7", new[] { "1", "b3", "5", "b7" })]
        [DataRow("Cmin7", new[] { "1", "b3", "5", "b7" })]
        [DataRow("CmΔ7", new[] { "1", "b3", "5", "7" })]
        [DataRow("CmM7", new[] { "1", "b3", "5", "7" })]
        [DataRow("Cminmaj7", new[] { "1", "b3", "5", "7" })]
        [DataRow("CminM7", new[] { "1", "b3", "5", "7" })]
        [DataRow("C7aug", new[] { "1", "3", "#5", "b7" })]
        [DataRow("Caug7", new[] { "1", "3", "#5", "b7" })]
        [DataRow("C+7", new[] { "1", "3", "#5", "b7" })]
        [DataRow("C7+", new[] { "1", "3", "#5", "b7" })]
        [DataRow("Cmin11b5", new[] { "1", "b3", "b5", "b7", "9", "11" })]
        [DataRow("Co7", new[] { "1", "b3", "b5", "bb7" })]
        [DataRow("Cmin7b5add11", new[] { "1", "b3", "b5", "b7", "11" })]
        [DataRow("Cmaj7#5", new[] { "1", "3", "#5", "7" })]
        [DataRow("Cmaj7sus2", new[] { "1", "2", "5", "7" })]
        [DataRow("Csus4", new[] { "1", "4", "5" })]
        [DataRow("Csus2", new[] { "1", "2", "5" })]
        [DataRow("C7sus4", new[] { "1", "4", "5", "b7" })]
        [DataRow("Csusbb3bb7", new[] { "1", "bb3", "5", "bb7" })]
        [DataRow("Cmin11", new[] { "1", "b3", "5", "b7", "9", "11" })]
        [DataRow("Cdim11nobb7", new[] { "1", "b3", "b5", "9", "11" })]
        [DataRow("Cminno5bb7", new[] { "1", "b3", "bb7" })]
        [DataRow("C9no3", new[] { "1", "5", "b7", "9" })]
        [DataRow("C11no5", new[] { "1", "3","b7", "9" ,"11"})]
        [DataRow("C13no11", new[] { "1", "3", "5", "b7", "9", "13"})]
        [DataRow("C9add11", new[] { "1", "3", "5", "b7", "9" , "11"})]
        [DataRow("C9no9", new[] { "1", "3", "5", "b7" })]
        public void ParseChordSymbol_ValidString_ReturnsCorrectImplicitStructure(string chordSymbol, string[] expectedIntervals)
        {
            HashSet<(int, int)> intervals = SymbolHelper.ParseChordSymbol(chordSymbol);
            List<string> symbols = NomenclatureHelper.SortIntervalNames(intervals.Select(it => SymbolHelper.CreateScaleDegreeSymbol(it)));
            CollectionAssert.AreEqual(expectedIntervals, symbols);
        }

        [TestMethod]
        [DataRow(new[] { "1", "3", "5" }, "maj")]
        [DataRow(new[] { "1", "3", "5", "7" }, "maj7")]
        [DataRow(new[] { "1", "3", "5", "7", "9" }, "maj9")]
        [DataRow(new[] { "1", "3", "5", "9" }, "majadd9")]
        [DataRow(new[] { "1", "3", "5", "7", "9", "11" }, "maj11")]
        [DataRow(new[] { "1", "3", "5", "7", "9", "11", "13" }, "maj13")]
        [DataRow(new[] { "1", "3", "5", "7", "11" }, "maj7add11")]
        [DataRow(new[] { "1", "3", "5", "7", "9", "13" }, "maj9add13")]
        [DataRow(new[] { "1", "3", "5", "b7" }, "7")]
        [DataRow(new[] { "1", "3", "5", "b7", "9" }, "9")]
        [DataRow(new[] { "1", "3", "5", "b7", "9", "13" }, "9add13")]
        [DataRow(new[] { "1", "b3", "5" }, "min")]
        [DataRow(new[] { "1", "b3", "5", "b7", "9" }, "min9")]
        [DataRow(new[] { "1", "b3", "5", "9" }, "minadd9")]
        [DataRow(new[] { "1", "b3", "5", "7" }, "minmaj7")]
        [DataRow(new[] { "1", "b3", "5", "7", "9" }, "minmaj9")]
        [DataRow(new[] { "1", "b3", "5", "7", "11" }, "minmaj7add11")]
        [DataRow(new[] { "1", "b3", "5", "7", "9", "11", "13" }, "minmaj13")]
        [DataRow(new[] { "1", "b3", "b5", "b7" }, "min7b5")]
        [DataRow(new[] { "1", "b3", "b5", "b7", "9" }, "min9b5")]
        [DataRow(new[] { "1", "b3", "b5", "b7", "9", "11", "13" }, "min13b5")]
        [DataRow(new[] { "1", "b3", "b5", "bb7" }, "dim7")]
        [DataRow(new[] { "1", "b3", "b5", "bb7", "9" }, "dim9")]
        [DataRow(new[] { "1", "b3", "b5", "bb7", "9", "11" }, "dim11")]
        [DataRow(new[] { "1", "3", "7", "9" }, "maj9no5")]
        [DataRow(new[] { "1", "2", "5" }, "sus2")]
        [DataRow(new[] { "1", "bb3", "5" }, "susbb3")]
        [DataRow(new[] { "1", "#3", "5" }, "sus#3")]
        [DataRow(new[] { "1", "4", "5" }, "sus4")]
        [DataRow(new[] { "1", "2", "5", "7" }, "maj7sus2")]
        [DataRow(new[] { "1", "2", "5", "b7" }, "7sus2")]
        [DataRow(new[] { "1", "2", "5", "bb7" }, "sus2bb7")]
        [DataRow(new[] { "1", "3", "5", "bb7" }, "majbb7")]
        [DataRow(new[] { "1", "bb3", "#5", "7" }, "maj7susbb3#5")]
        [DataRow(new[] { "1", "bb3", "b5", "bb7", "9" }, "susbb3b5bb7add9")]
        [DataRow(new[] { "1", "5", "bb7", "9" }, "no3bb7add9")]
        [DataRow(new[] { "1", "3", "##5", "7" }, "maj7##5")]
        [DataRow(new[] { "1", "3", "bb5", "7" }, "maj7bb5")]
        [DataRow(new[] { "1", "3", "#5", "bb7" }, "maj#5bb7")]
        [DataRow(new[] { "1", "4", "5", "b7" }, "7sus4")]
        public void CreateChordSymbol_DefaultStyling_ReturnsCorrectSymbol(string[] intervals, string expectedChordSymbol)
        {
            string chordSymbol = SymbolHelper.CreateChordSymbol(intervals.Select(SymbolHelper.ParseScaleDegreeSymbol));
            Assert.AreEqual(expectedChordSymbol, chordSymbol);
        }
    }
}