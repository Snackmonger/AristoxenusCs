
using Aristoxenus.Core.Models;

namespace Aristoxenus.Core.Tests;

[TestClass]
public sealed class ChordNameTests
{
    private static NoteName NoteName_C = NoteName.FromSymbol("C");


    [TestMethod]
    [DataRow(new[] { "1", "3", "5" }, "Cmaj")]
    [DataRow(new[] { "1", "3", "5" }, "C")]
    [DataRow(new[] { "1", "3", "5" }, "CΔ")]
    [DataRow(new[] { "1", "3", "5" }, "CM")]
    [DataRow(new[] { "1", "b3", "5" }, "Cmin")]
    [DataRow(new[] { "1", "b3", "5" }, "Cm")]
    [DataRow(new[] { "1", "b3", "5" }, "C-")]
    [DataRow(new[] { "1", "3", "5", "7" }, "Cmaj7")]
    [DataRow(new[] { "1", "3", "5", "7" }, "CM7")]
    [DataRow(new[] { "1", "3", "5", "7" }, "CΔ7")]
    [DataRow(new[] { "1", "b3", "5", "b7" }, "Cm7")]
    [DataRow(new[] { "1", "b3", "5", "b7" }, "C-7")]
    [DataRow(new[] { "1", "b3", "5", "b7" }, "Cmin7")]
    [DataRow(new[] { "1", "b3", "5", "7" }, "CmΔ7")]
    [DataRow(new[] { "1", "b3", "5", "7" }, "CmM7")]
    [DataRow(new[] { "1", "b3", "5", "7" }, "C-M7")]
    [DataRow(new[] { "1", "b3", "5", "b7", "9", "11" }, "Cmin11")]
    [DataRow(new[] { "1", "b3", "5", "b7", "11" }, "C-7add11")]
    [DataRow(new[] { "1", "b3", "5", "7" }, "Cminmaj7")]
    [DataRow(new[] { "1", "b3", "5", "7" }, "C-maj7")]
    [DataRow(new[] { "1", "b3", "5", "7" }, "CminM7")]
    [DataRow(new[] { "1", "3", "#5", "b7" }, "C7aug")]
    [DataRow(new[] { "1", "3", "#5", "b7" }, "Caug7")]
    [DataRow(new[] { "1", "3", "#5", "b7" }, "C+7")]
    [DataRow(new[] { "1", "3", "#5", "b7" }, "C7+")]
    [DataRow(new[] { "1", "3", "#5", "7" }, "Cmaj7#5")]
    [DataRow(new[] { "1", "3", "#5", "7" }, "Cmaj7+")]
    [DataRow(new[] { "1", "3", "#5", "7" }, "Cmaj7aug")]
    [DataRow(new[] { "1", "b3", "b5", "b7" }, "Cmin7b5")]
    [DataRow(new[] { "1", "b3", "b5", "b7" }, "Cm7b5")]
    [DataRow(new[] { "1", "b3", "b5", "b7" }, "C-7b5")]
    [DataRow(new[] { "1", "b3", "b5", "b7", "9", "11" }, "Cmin11b5")]
    [DataRow(new[] { "1", "b3", "b5", "b7", "11" }, "Cmin7b5add11")]
    [DataRow(new[] { "1", "b3", "b5", "b7", "9", "11" }, "Cm11b5")]
    [DataRow(new[] { "1", "b3", "b5", "b7", "9", "11" }, "C-11b5")]
    [DataRow(new[] { "1", "b3", "b5" }, "Cø")]
    [DataRow(new[] { "1", "b3", "b5" }, "Cdim")]
    [DataRow(new[] { "1", "b3", "b5", "b7" }, "Cø7")]
    [DataRow(new[] { "1", "b3", "b5", "bb7" }, "Co7")]
    [DataRow(new[] { "1", "b3", "b5", "bb7", "9" }, "Co9")]
    [DataRow(new[] { "1", "b3", "b5", "bb7", "9", "11" }, "Co11")]
    [DataRow(new[] { "1", "b3", "b5", "bb7" }, "Cdim7")]
    [DataRow(new[] { "1", "b3", "b5", "bb7", "9" }, "Cdim9")]
    [DataRow(new[] { "1", "b3", "b5", "bb7", "9", "11" }, "Cdim11")]
    [DataRow(new[] { "1", "2", "5" }, "Csus2")]
    [DataRow(new[] { "1", "4", "5" }, "Csus4")]
    [DataRow(new[] { "1", "2", "5", "7" }, "Cmaj7sus2")]
    [DataRow(new[] { "1", "4", "5", "7" }, "Cmaj7sus4")]
    [DataRow(new[] { "1", "2", "5", "7" }, "CM7sus2")]
    [DataRow(new[] { "1", "2", "5", "7" }, "CΔ7sus2")]
    [DataRow(new[] { "1", "4", "5", "b7" }, "C7sus4")]
    [DataRow(new[] { "1", "bb3", "5", "bb7" }, "Csusbb3bb7")]
    [DataRow(new[] { "1", "#3", "5", "bb7" }, "Csus#3bb7")]
    [DataRow(new[] { "1", "#3", "5", "b7" }, "C7sus#3")]
    [DataRow(new[] { "1", "bb3", "5", "b7" }, "C7susbb3")]
    [DataRow(new[] { "1", "b3", "b5", "9", "11" }, "Cdim11nobb7")]
    [DataRow(new[] { "1", "b3", "b5", "9", "11" }, "Co11nobb7")]
    [DataRow(new[] { "1", "b3", "bb7" }, "Cminno5bb7")]
    [DataRow(new[] { "1", "b3", "bb7" }, "Cmno5bb7")]
    [DataRow(new[] { "1", "b3", "bb7" }, "C-no5bb7")]
    [DataRow(new[] { "1", "5", "b7", "9" }, "C9no3")]
    [DataRow(new[] { "1", "3", "b7", "9", "11" }, "C11no5")]
    [DataRow(new[] { "1", "3", "5", "b7", "9", "13" }, "C13no11")]
    [DataRow(new[] { "1", "3", "5", "b7" }, "C9no9")]
    [DataRow(new[] { "1", "3", "5", "b7", "9" }, "C9no4")]
    [DataRow(new[] { "1", "3", "5", "b7", "9", "11" }, "C9add11")]
    [DataRow(new[] { "1", "b3", "5", "b7", "11" }, "Cmin7add11")]
    [DataRow(new[] { "1", "3", "#5", "11" }, "Caugadd11")]
    [DataRow(new[] { "1", "3", "#5", "11" }, "C+add11")]
    [DataRow(new[] { "1", "3", "#5", "b7", "11" }, "C7+add11")]
    [DataRow(new[] { "1", "3", "#5", "bb7", "b7" }, "C7+addbb7")]
    public void FromSymbol_ValidChordSymbol_ReturnsCorrectImplicitStructure(string[] expectedIntervals, string chordSymbol)
    {
        ChordName chordName = ChordName.FromSymbol(chordSymbol);

        List<string> intervals = [.. chordName.Intervals.Select(it => it.Name.ToSymbol())];

        CollectionAssert.AreEquivalent(expectedIntervals, intervals);
    }

    [TestMethod]
    [DataRow("Cmaj", new[] { "1", "3", "5" })]
    [DataRow("Cmaj7", new[] { "1", "3", "5", "7" })]
    [DataRow("Cmaj9", new[] { "1", "3", "5", "7", "9" })]
    [DataRow("Cmajadd9", new[] { "1", "3", "5", "9" })]
    [DataRow("Cmaj11", new[] { "1", "3", "5", "7", "9", "11" })]
    [DataRow("Cmaj13", new[] { "1", "3", "5", "7", "9", "11", "13" })]
    [DataRow("Cmaj7add11", new[] { "1", "3", "5", "7", "11" })]
    [DataRow("Cmaj9add13", new[] { "1", "3", "5", "7", "9", "13" })]
    [DataRow("C7", new[] { "1", "3", "5", "b7" })]
    [DataRow("C9", new[] { "1", "3", "5", "b7", "9" })]
    [DataRow("C9add13", new[] { "1", "3", "5", "b7", "9", "13" })]
    [DataRow("Cmin", new[] { "1", "b3", "5" })]
    [DataRow("Cmin9", new[] { "1", "b3", "5", "b7", "9" })]
    [DataRow("Cminadd9", new[] { "1", "b3", "5", "9" })]
    [DataRow("Cminmaj7", new[] { "1", "b3", "5", "7" })]
    [DataRow("Cminmaj9", new[] { "1", "b3", "5", "7", "9" })]
    [DataRow("Cminmaj7add11", new[] { "1", "b3", "5", "7", "11" })]
    [DataRow("Cminmaj13", new[] { "1", "b3", "5", "7", "9", "11", "13" })]
    [DataRow("Cmin7b5", new[] { "1", "b3", "b5", "b7" })]
    [DataRow("Cminb5", new[] { "1", "b3", "b5" })]
    [DataRow("Cmin9b5", new[] { "1", "b3", "b5", "b7", "9" })]
    [DataRow("Cmin13b5", new[] { "1", "b3", "b5", "b7", "9", "11", "13" })]
    [DataRow("Cdim7", new[] { "1", "b3", "b5", "bb7" })]
    [DataRow("Cdim9", new[] { "1", "b3", "b5", "bb7", "9" })]
    [DataRow("Cdim11", new[] { "1", "b3", "b5", "bb7", "9", "11" })]
    [DataRow("Cmaj9no5", new[] { "1", "3", "7", "9" })]
    [DataRow("Csus2", new[] { "1", "2", "5" })]
    [DataRow("Csus2add4", new[] { "1", "2", "4", "5" })]
    [DataRow("Csusbb3", new[] { "1", "bb3", "5" })]
    [DataRow("Csus#3", new[] { "1", "#3", "5" })]
    [DataRow("Csus4", new[] { "1", "4", "5" })]
    [DataRow("Cmaj7sus2", new[] { "1", "2", "5", "7" })]
    [DataRow("C7sus2", new[] { "1", "2", "5", "b7" })]
    [DataRow("Csus2bb7", new[] { "1", "2", "5", "bb7" })]
    [DataRow("Cmajbb7", new[] { "1", "3", "5", "bb7" })]
    [DataRow("Cmaj7susbb3#5", new[] { "1", "bb3", "#5", "7" })]
    [DataRow("Csusbb3b5bb7add9", new[] { "1", "bb3", "b5", "bb7", "9" })]
    [DataRow("Cno3bb7add9", new[] { "1", "5", "bb7", "9" })]
    [DataRow("Cmaj7##5", new[] { "1", "3", "##5", "7" })]
    [DataRow("Cmaj7bb5", new[] { "1", "3", "bb5", "7" })]
    [DataRow("Cmaj#5bb7", new[] { "1", "3", "#5", "bb7" })]
    [DataRow("C7sus4", new[] { "1", "4", "5", "b7" })]
    public void ToSymbol_CNoteNameRoot_ReturnsCorrectSymbol(string expectedChordSymbol, string[] intervals)
    {
        IntervalStructure intervalStructure = new(intervals.Select(Interval.FromScaleDegreeSymbol));
        ChordName chordName = new(NoteName_C, intervalStructure);
        string chordSymbol = chordName.ToSymbol();
        Assert.AreEqual(expectedChordSymbol, chordSymbol);
    }


}