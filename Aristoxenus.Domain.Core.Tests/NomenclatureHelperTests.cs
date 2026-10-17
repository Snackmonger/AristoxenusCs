using Aristoxenus.Domain.Core.Utilities;

namespace Aristoxenus.Domain.Core.Tests;


[TestClass]
public sealed class NomenclatureHelperTests
{

    private readonly (int, int) G = (4, 0);
    private readonly (int, int) GSharp = (4, 1);
    private readonly (int, int) A = (5, 0);
    private readonly (int, int) F = (3, 0);
    private readonly (int, int) Db = (2, -1); // TODO: ADD TESTS FOR THIS CASE

    [TestMethod]
    [DataRow(0, "1")]
    [DataRow(1, "b2")]
    [DataRow(2, "2")]
    [DataRow(2, "bb3")]
    [DataRow(3, "b3")]
    [DataRow(4, "3")]
    [DataRow(4, "b4")]
    [DataRow(5, "#3")]
    [DataRow(5, "4")]
    [DataRow(6, "#4")]
    [DataRow(7, "##4")]
    [DataRow(5, "bb5")]
    [DataRow(6, "b5")]
    [DataRow(7, "5")]
    [DataRow(8, "#5")]
    [DataRow(8, "b6")]
    [DataRow(9, "6")]
    [DataRow(9, "bb7")]
    [DataRow(10, "b7")]
    [DataRow(11, "7")]
    [DataRow(12, "8")]
    [DataRow(13, "b9")]
    [DataRow(14, "9")]
    [DataRow(15, "#9")]
    [DataRow(15, "b10")]
    [DataRow(16, "10")]
    [DataRow(17, "11")]
    [DataRow(18, "#11")]
    [DataRow(18, "b12")]
    [DataRow(19, "12")]
    public void GetIntervalValueFromName_ValidInput_CorrectOutput(int expectedValue, string intervalSymbol)
    {
        (int degree, int accidentals) = SymbolHelper.ParseScaleDegreeSymbol(intervalSymbol);
        int actualValue = NomenclatureHelper.GetIntervalValueFromDegreeName((degree, accidentals));
        Assert.AreEqual(expectedValue, actualValue);
    }


    [TestMethod]
    [DataRow("#1", "A#")]
    [DataRow("b2", "Bb")]
    [DataRow("2", "B")]
    [DataRow("bb3", "Cb")]
    [DataRow("b3", "C")]
    [DataRow("3", "C#")]
    [DataRow("#3", "C##")]
    [DataRow("b4", "Db")]
    [DataRow("4", "D")]
    [DataRow("#4", "D#")]
    [DataRow("b5", "Eb")]
    [DataRow("5", "E")]
    [DataRow("#5", "E#")]
    [DataRow("bb6", "Fb")]
    [DataRow("b6", "F")]
    [DataRow("6", "F#")]
    [DataRow("#6", "F##")]
    [DataRow("bb7", "Gb")]
    [DataRow("b7", "G")]
    [DataRow("7", "G#")]
    public void GetIntervalNameBetweenNoteNames_ANoteInBass_ReturnsCorrectIntervalAbove(string expectedInterval, string noteName)
    {
        (int, int) noteNameRawValues = SymbolHelper.ParseNoteNameSymbol(noteName);
        (int, int) actualIntervalRawValues = NomenclatureHelper.GetIntervalNameBetweenNoteNames(A, noteNameRawValues);
        string actualInterval = SymbolHelper.CreateScaleDegreeSymbol(actualIntervalRawValues);
        Assert.AreEqual(expectedInterval, actualInterval);
    }

    [TestMethod]
    [DataRow("b2", "Ab")]
    [DataRow("2", "A")]
    [DataRow("#2", "A#")]
    [DataRow("##2", "A##")]
    [DataRow("b3", "Bb")]
    [DataRow("3", "B")]
    [DataRow("b4", "Cb")]
    [DataRow("4", "C")]
    [DataRow("#4", "C#")]
    [DataRow("b5", "Db")]
    [DataRow("5", "D")]
    [DataRow("#5", "D#")]
    [DataRow("##5", "D##")]
    [DataRow("b6", "Eb")]
    [DataRow("bb6", "Ebb")]
    [DataRow("6", "E")]
    [DataRow("#6", "E#")]
    [DataRow("bb7", "Fb")]
    [DataRow("b7", "F")]
    [DataRow("7", "F#")]
    public void GetIntervalNameBetweenNoteNames_GNoteInBass_ReturnsCorrectIntervalAbove(string expectedInterval, string noteName)
    {
        (int, int) noteNameRawValues = SymbolHelper.ParseNoteNameSymbol(noteName);
        (int, int) actualIntervalRawValues = NomenclatureHelper.GetIntervalNameBetweenNoteNames(G, noteNameRawValues);
        string actualInterval = SymbolHelper.CreateScaleDegreeSymbol(actualIntervalRawValues);
        Assert.AreEqual(expectedInterval, actualInterval);
    }

    // TODO: MAKE MORE READABLE WITH SYMBOLS
    [TestMethod]
    [DataRow(6, 0, 5, 2)]
    [DataRow(3, 0, 2, 1)]
    [DataRow(3, 1, 3, 1)]
    [DataRow(2, 0, 3, -1)]
    [DataRow(6, 0, 0, -1)]
    [DataRow(0, 0, 6, 1)]
    [DataRow(1, 0, 2, -2)]
    [DataRow(1, -1, 1, -1)]
    [DataRow(6, -1, 6, -1)]
    [DataRow(4, -1, 4, -1)]

    public void GetSimplifiedName_ValidInput_CorrectOutput(int expectedIndex, int expectedAccidentals, int inputIndex, int inputAccidentals)
    {

        (int, int) name = NomenclatureHelper.GetSimplifiedName(inputIndex, inputAccidentals);
        Assert.AreEqual((expectedIndex, expectedAccidentals), name);

    }

    [TestMethod]
    [DataRow("b2", "Ab")]
    [DataRow("2", "A")]
    [DataRow("#2", "A#")]
    [DataRow("##2", "A##")]
    [DataRow("bb3", "Bbb")]
    [DataRow("b3", "Bb")]
    [DataRow("3", "B")]
    [DataRow("#3", "B#")]
    [DataRow("bb4", "Cbb")]
    [DataRow("b4", "Cb")]
    [DataRow("4", "C")]
    [DataRow("#4", "C#")]
    [DataRow("##4", "C##")]
    [DataRow("bb5", "Dbb")]
    [DataRow("b5", "Db")]
    [DataRow("5", "D")]
    [DataRow("#5", "D#")]
    [DataRow("##5", "D##")]
    [DataRow("bb6", "Ebb")]
    [DataRow("b6", "Eb")]
    [DataRow("6", "E")]
    [DataRow("#6", "E#")]
    [DataRow("bb7", "Fb")]
    [DataRow("b7", "F")]
    [DataRow("7", "F#")]
    public void NoteNameAbove_GNoteInBass_ReturnsCorrectNameAbove(string intervalName, string expectedNoteName)
    {
        (int, int) intervalRawValue = SymbolHelper.ParseScaleDegreeSymbol(intervalName);
        (int, int) actualNoteNameRawValue = NomenclatureHelper.GetNoteNameAbove(G, intervalRawValue);
        string actualNoteName = SymbolHelper.CreateNoteNameSymbol(actualNoteNameRawValue);
        Assert.AreEqual(expectedNoteName, actualNoteName);
    }

    [TestMethod]
    [DataRow("b2", "A")]
    [DataRow("2", "A#")]
    [DataRow("#2", "A##")]
    [DataRow("##2", "A###")]
    [DataRow("bb3", "Bb")]
    [DataRow("b3", "B")]
    [DataRow("3", "B#")]
    [DataRow("b4", "C")]
    [DataRow("4", "C#")]
    [DataRow("#4", "C##")]
    [DataRow("bb5", "Db")]
    [DataRow("b5", "D")]
    [DataRow("5", "D#")]
    [DataRow("#5", "D##")]
    [DataRow("b6", "E")]
    [DataRow("6", "E#")]
    [DataRow("#6", "E##")]
    [DataRow("bb7", "F")]
    [DataRow("b7", "F#")]
    [DataRow("7", "F##")]
    public void NoteNameAbove_GSharpNoteInBass_ReturnsCorrectNameAbove(string intervalName, string expectedNoteName)
    {
        (int, int) intervalRawValue = SymbolHelper.ParseScaleDegreeSymbol(intervalName);
        (int, int) actualNoteNameRawValue = NomenclatureHelper.GetNoteNameAbove(GSharp, intervalRawValue);
        string actualNoteName = SymbolHelper.CreateNoteNameSymbol(actualNoteNameRawValue);
        Assert.AreEqual(expectedNoteName, actualNoteName);
    }

    [TestMethod]
    [DataRow("b2", "Bb")]
    [DataRow("2", "B")]
    [DataRow("#2", "B#")]
    [DataRow("b3", "C")]
    [DataRow("3", "C#")]
    [DataRow("b4", "Db")]
    [DataRow("4", "D")]
    [DataRow("#4", "D#")]
    [DataRow("##4", "D##")]
    [DataRow("5", "E")]
    [DataRow("b5", "Eb")]
    [DataRow("##5", "E##")]
    [DataRow("#5", "E#")]
    [DataRow("bb6", "Fb")]
    [DataRow("b6", "F")]
    [DataRow("6", "F#")]
    [DataRow("#6", "F##")]
    [DataRow("bb7", "Gb")]
    [DataRow("b7", "G")]
    [DataRow("7", "G#")]
    public void NoteNameAbove_ANoteInBass_ReturnsCorrectNoteNameAbove(string intervalName, string expectedNoteName)
    {
        (int, int) intervalRawValue = SymbolHelper.ParseScaleDegreeSymbol(intervalName);
        (int, int) actualNoteNameRawValue = NomenclatureHelper.GetNoteNameAbove(A, intervalRawValue);
        string actualNoteName = SymbolHelper.CreateNoteNameSymbol(actualNoteNameRawValue);
        Assert.AreEqual(expectedNoteName, actualNoteName);

    }

    [TestMethod]
    [DataRow("b2", "Gb")]
    [DataRow("2", "G")]
    [DataRow("#2", "G#")]
    [DataRow("##2", "G##")]
    [DataRow("b3", "Ab")]
    [DataRow("bb3", "Abb")]
    [DataRow("3", "A")]
    [DataRow("#3", "A#")]
    [DataRow("##3", "A##")]
    [DataRow("b4", "Bbb")]
    [DataRow("4", "Bb")]
    [DataRow("#4", "B")]
    [DataRow("##4", "B#")]
    [DataRow("bb5", "Cbb")]
    [DataRow("b5", "Cb")]
    [DataRow("5", "C")]
    [DataRow("#5", "C#")]
    [DataRow("##5", "C##")]
    [DataRow("bb6", "Dbb")]
    [DataRow("b6", "Db")]
    [DataRow("6", "D")]
    [DataRow("#6", "D#")]
    [DataRow("bb7", "Ebb")]
    [DataRow("b7", "Eb")]
    [DataRow("7", "E")]
    public void NoteNameAbove_FNoteInBass_ReturnsCorrectNoteNameAbove(string intervalName, string expectedNoteName)
    {
        (int, int) intervalRawValue = SymbolHelper.ParseScaleDegreeSymbol(intervalName);
        (int, int) actualNoteNameRawValue = NomenclatureHelper.GetNoteNameAbove(F, intervalRawValue);
        string actualNoteName = SymbolHelper.CreateNoteNameSymbol(actualNoteNameRawValue);
        Assert.AreEqual(expectedNoteName, actualNoteName);

    }

    [TestMethod]
    [DataRow(["1", "3", "5", "b7"])]
    [DataRow(["1", "3", "5", "b7", "9", "13"])]
    [DataRow(["1", "3", "b5", "b7"])]
    [DataRow(["1", "3", "#5", "b7", "#11"])]
    [DataRow(["1", "3", "5", "b7", "b9"])]
    public void IsDominant_DominantStructure_ReturnsTrue(string[] intervals)
    {
        IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseScaleDegreeSymbol);
        Assert.IsTrue(NomenclatureHelper.IsDominant(rawValues));
    }

    [TestMethod]
    [DataRow(["1", "b3", "5", "b7"])]
    [DataRow(["1", "3", "5", "7", "9", "13"])]
    [DataRow(["1", "3", "b5", "bb7"])]
    [DataRow(["1", "#3", "#5", "b7", "#11"])]
    [DataRow(["1", "3", "5", "7", "b9"])]
    public void IsDominant_NonDominantStructure_ReturnsFalse(string[] intervals)
    {
        IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseScaleDegreeSymbol);
        Assert.IsFalse(NomenclatureHelper.IsDominant(rawValues));
    }

    [TestMethod]
    [DataRow(["1", "b3", "b5", "bb7"])]
    [DataRow(["1", "b3", "b5", "bb7", "9", "13"])]
    [DataRow(["1", "b3", "b5", "bb7", "#11"])]
    [DataRow(["1", "b3", "b5", "bb7", "b9"])]
    [DataRow(["1", "b3", "b5", "bb7", "11"])]
    public void IsDiminished_DiminishedStructure_ReturnsTrue(string[] intervals)
    {
        IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseScaleDegreeSymbol);
        Assert.IsTrue(NomenclatureHelper.IsDiminished(rawValues));
    }

    [TestMethod]
    [DataRow(["1", "3", "b5", "bb7"])]
    [DataRow(["1", "b3", "5", "bb7", "9", "13"])]
    [DataRow(["1", "b3", "b5", "b7"])]
    [DataRow(["1", "3", "5", "bb7", "#11"])]
    [DataRow(["1", "b3", "5", "bb7", "b9"])]
    public void IsDiminished_NonDiminishedStructure_ReturnsFalse(string[] intervals)
    {
        IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseScaleDegreeSymbol);
        Assert.IsFalse(NomenclatureHelper.IsDiminished(rawValues));
    }


    [TestMethod]
    [DataRow(["1", "3", "5"])]
    [DataRow(["1", "b3", "5"])]
    [DataRow(["1", "3", "5", "b6"])]
    [DataRow(["1", "b3", "5", "b5"])]
    [DataRow(["1", "3", "5", "7"])]
    public void HasPerfectFifth_PerfectFifthStructure_ReturnsTrue(string[] intervals)
    {
        IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseScaleDegreeSymbol);
        Assert.IsTrue(NomenclatureHelper.HasPerfectFifth(rawValues));
    }

    [TestMethod]
    [DataRow(["1", "3", "b5"])]
    [DataRow(["1", "3", "#5"])]
    [DataRow(["1", "2", "6"])]
    [DataRow(["1", "4", "#5"])]
    [DataRow(["1", "3", "4"])]
    [DataRow(["1", "b2", "b5"])]
    [DataRow(["1", "bb3", "bb5"])]
    [DataRow(["1", "#3", "4"])]
    [DataRow(["1", "3", "6"])]
    [DataRow(["1", "4", "7"])]
    public void HasPerfectFifth_NonPerfectFifthStructure_ReturnsFalse(string[] intervals)
    {
        IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseScaleDegreeSymbol);
        Assert.IsFalse(NomenclatureHelper.HasPerfectFifth(rawValues));
    }

    [TestMethod]
    [DataRow(["1", "3", "b5"])]
    [DataRow(["1", "3", "#5"])]
    [DataRow(["1", "b3", "b5"])]
    [DataRow(["1", "b3", "#5"])]
    [DataRow(["1", "bb3", "#5"])]
    [DataRow(["1", "2", "#5"])]
    [DataRow(["1", "2", "b5"])]
    [DataRow(["1", "4", "#5"])]
    [DataRow(["1", "2", "b5", "b7"])]
    [DataRow(["1", "3", "#5", "6"])]
    public void HasAlteredFifth_AlteredFifthStructure_ReturnsTrue(string[] intervals)
    {
        IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseScaleDegreeSymbol);
        Assert.IsTrue(NomenclatureHelper.HasAlteredFifth(rawValues));
    }

    [TestMethod]
    [DataRow(["1", "3", "5"])]
    [DataRow(["1", "b3", "5"])]
    [DataRow(["1", "3", "5", "b7"])]
    [DataRow(["1", "b3", "5", "7"])]
    [DataRow(["1", "bb3", "5", "bb7"])]
    public void HasAlteredFifth_NonAlteredFifthStructure_ReturnsFalse(string[] intervals)
    {
        IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseScaleDegreeSymbol);
        Assert.IsFalse(NomenclatureHelper.HasAlteredFifth(rawValues));
    }

    // TODO: MAKE MORE READABLE WITH SYMBOLS
    [TestMethod]
    [DataRow([4, 1, 4, 6])]
    [DataRow([4, 0, 4, 5])]
    [DataRow([1, 1, 1, 1])]
    [DataRow([5, -1, 5, 6])]
    [DataRow([2, -1, 2, 1])]
    [DataRow([2, 1, 2, 3])]
    [DataRow([3, -1, 3, 3])]
    [DataRow([3, 1, 3, 5])]
    [DataRow([7, -1, 7, 10])]
    [DataRow([7, -2, 7, 9])]
    public void GetIntervalNameFromExpectedValue_ValidValues_CorrectOutputs(int expectedDegree, int expectedAccidentals, int degree, int value)
    {
        (int, int) actualResult = NomenclatureHelper.GetIntervalDegreeNameFromExpectedValue(degree, value);
        Assert.AreEqual((expectedDegree, expectedAccidentals), actualResult);
    }

    [TestMethod]
    [DataRow([new[] { "G", "A", "B", "C", "D", "E", "F#" }, "G", new[] { 0, 2, 4, 5, 7, 9, 11 }])]
    [DataRow([new[] { "G", "A", "Bb", "C", "D", "E", "F" }, "G", new[] { 0, 2, 3, 5, 7, 9, 10 }])]
    [DataRow([new[] { "G", "A", "Bb", "C", "D", "Eb", "F" }, "G", new[] { 0, 2, 3, 5, 7, 8, 10 }])]
    [DataRow([new[] { "A", "B", "C#", "D", "E", "F#", "G#" }, "A", new[] { 0, 2, 4, 5, 7, 9, 11 }])]
    [DataRow([new[] { "Bb", "C", "D", "Eb", "F", "G", "A" }, "Bb", new[] { 0, 2, 4, 5, 7, 9, 11 }])]
    [DataRow([new[] { "E", "F#", "G#", "A", "B", "C#", "D#", }, "E", new[] { 0, 2, 4, 5, 7, 9, 11 }])]
    [DataRow([new[] { "B#", "C##", "D##", "E#", "F##", "G##", "A##" }, "B#", new[] { 0, 2, 4, 5, 7, 9, 11 }])]
    [DataRow([new[] { "Cb", "Db", "Eb", "Fb", "Gb", "Ab", "Bb" }, "Cb", new[] { 0, 2, 4, 5, 7, 9, 11 }])]
    [DataRow([new[] { "D", "E", "F#", "G", "Ab", "Bb", "C#" }, "D", new[] { 0, 2, 4, 5, 6, 8, 11 }])]
    [DataRow([new[] { "A", "B", "C", "D#", "E", "F#", "G#"}, "A", new[] { 0, 2, 3, 6, 7, 9, 11 }])]
    public void CalculateHeptatonicNames_ValidValues_CorrectOutputs(string[] expectedNoteNames, string noteNameSymbol, int[] pattern)
    {
        var result = NomenclatureHelper.CalculateHeptatonicNames(SymbolHelper.ParseNoteNameSymbol(noteNameSymbol), pattern);
        var names = expectedNoteNames.Select(SymbolHelper.ParseNoteNameSymbol).ToList();
        CollectionAssert.AreEqual(names, result);
    }
}