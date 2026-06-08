using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Utilities;
using Aristoxenus.Core.Domains.Theory.Models;

namespace Aristoxenus.Tests
{

    [TestClass]
    public sealed class NomenclatureHelperTests
    {

        private readonly (int, int) G = (4, 0);
        private readonly (int, int) GSharp = (4, 1);
        private readonly (int, int) A = (5, 0);
        private readonly (int, int) F = (3, 0);

        [TestMethod]
        [DataRow("b2", "Ab")]
        [DataRow("2", "A")]
        [DataRow("#2", "A#")]
        [DataRow("b3", "Bb")]
        [DataRow("3", "B")]
        [DataRow("b4", "Cb")]
        [DataRow("4", "C")]
        [DataRow("#4", "C#")]
        [DataRow("b5", "Db")]
        [DataRow("5", "D")]
        [DataRow("#5", "D#")]
        [DataRow("b6", "Eb")]
        [DataRow("6", "E")]
        [DataRow("#6", "E#")]
        [DataRow("bb7", "Fb")]
        [DataRow("b7", "F")]
        [DataRow("7", "F#")]
        public void GetIntervalNameBetweenNoteNames_GNoteInBass_ReturnsCorrectIntervalAbove(string expectedInterval, string noteName)
        {
            (int, int) noteNameRawValues = SymbolHelper.ParseNoteNameSymbol(noteName);
            (int, int) actualIntervalRawValues = NomenclatureHelper.GetIntervalNameBetweenNoteNames(G, noteNameRawValues);
            string actualInterval = SymbolHelper.CreateIntervalNameSymbol(actualIntervalRawValues);
            Assert.AreEqual(expectedInterval, actualInterval);
        }

        [TestMethod]
        [DataRow(5, 2, 6, 0)]
        [DataRow(1, -1, 1, -1)]
        [DataRow(2, 1, 3, 0)]
        [DataRow(6, -1, 6, -1)]
        [DataRow(3, 1, 3, 1)]
        [DataRow(3, -1, 2, 0)]
        [DataRow(2, -1, 2, -1)]
        [DataRow(6, 1, 0, 0)]
        public void GetSimplifiedName_ValidInput_CorrectOutput(int inputIndex, int inputAccidentals, int expectedIndex, int expectedAccidentals)
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
            (int, int) intervalRawValue = SymbolHelper.ParseIntervalNameSymbol(intervalName);
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
        [DataRow("##4", "C###")]
        [DataRow("bb5", "Db")]
        [DataRow("b5", "D")]
        [DataRow("5", "D#")]
        [DataRow("#5", "D##")]
        [DataRow("##5", "D###")]
        [DataRow("bb6", "Eb")]
        [DataRow("b6", "E")]
        [DataRow("6", "E#")]
        [DataRow("#6", "E##")]
        [DataRow("bb7", "F")]
        [DataRow("b7", "F#")]
        [DataRow("7", "F##")]
        public void NoteNameAbove_GSharpNoteInBass_ReturnsCorrectNameAbove(string intervalName, string expectedNoteName)
        {
            (int, int) intervalRawValue = SymbolHelper.ParseIntervalNameSymbol(intervalName);
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
        [DataRow("bb5", "Ebb")]
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
            (int, int) intervalRawValue = SymbolHelper.ParseIntervalNameSymbol(intervalName);
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
        [DataRow("b4", "Bbb")]
        [DataRow("4", "Bb")]
        [DataRow("#4", "B")]
        [DataRow("##4", "B#")]
        [DataRow("bb5", "Cbb")]
        [DataRow("b5", "Cb")]
        [DataRow("5", "C")]
        [DataRow("#5", "C#")]
        [DataRow("##5", "C##")]
        [DataRow("b6", "Db")]
        [DataRow("6", "D")]
        [DataRow("#6", "D#")]
        [DataRow("bb7", "Ebb")]
        [DataRow("b7", "Eb")]
        [DataRow("7", "E")]
        public void NoteNameAbove_FNoteInBass_ReturnsCorrectNoteNameAbove(string intervalName, string expectedNoteName)
        {
            (int, int) intervalRawValue = SymbolHelper.ParseIntervalNameSymbol(intervalName);
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
            IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseIntervalNameSymbol);
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
            IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseIntervalNameSymbol);
            Assert.IsFalse(NomenclatureHelper.IsDominant(rawValues));
        }

        [TestMethod]
        [DataRow(["1", "b3", "b5", "bb7"])]
        [DataRow(["1", "b3", "b5", "bb7", "9", "13"])]
        [DataRow(["1", "b3", "b5", "bb7", "#11"])]
        [DataRow(["1", "b3", "b5", "bb7", "b9"])]
        public void IsDiminished_DiminishedStructure_ReturnsTrue(string[] intervals)
        {
            IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseIntervalNameSymbol);
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
            IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseIntervalNameSymbol);
            Assert.IsFalse(NomenclatureHelper.IsDiminished(rawValues));
        }


        [TestMethod]
        [DataRow(["1", "3", "5"])]
        [DataRow(["1", "b3", "5"])]
        public void HasPerfectFifth_PerfectFifthStructure_ReturnsTrue(string[] intervals)
        {
            IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseIntervalNameSymbol);
            Assert.IsTrue(NomenclatureHelper.HasPerfectFifth(rawValues));
        }

        [TestMethod]
        [DataRow(["1", "3", "b5"])]
        [DataRow(["1", "3", "#5"])]
        public void HasPerfectFifth_NonPerfectFifthStructure_ReturnsFalse(string[] intervals)
        {
            IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseIntervalNameSymbol);
            Assert.IsFalse(NomenclatureHelper.HasPerfectFifth(rawValues));
        }

        [TestMethod]
        [DataRow(["1", "3", "b5"])]
        [DataRow(["1", "3", "#5"])]
        public void HasAlteredFifth_AlteredFifthStructure_ReturnsTrue(string[] intervals)
        {
            IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseIntervalNameSymbol);
            Assert.IsTrue(NomenclatureHelper.HasAlteredFifth(rawValues));
        }

        [TestMethod]
        [DataRow(["1", "3", "5"])]
        [DataRow(["1", "b3", "5"])]
        public void HasAlteredFifth_NonAlteredFifthStructure_ReturnsFalse(string[] intervals)
        {
            IEnumerable<(int, int)> rawValues = intervals.Select(SymbolHelper.ParseIntervalNameSymbol);
            Assert.IsFalse(NomenclatureHelper.HasAlteredFifth(rawValues));
        }

        [TestMethod]
        [DataRow(new[] { 0, 2, 4, 6, 8, 9 })]
        [DataRow(new[] { 0, 2, 4, 6, 6, 9, 11 })]
        public void CreateAsHeptatonic_NonHeptatonicScale_ThrowsErrorWithIllegalValue(int[] intervals)
        {
            Assert.Throws<AristoxenusException>(() => IntervalStructure.CreateAsHeptatonic(intervals));
        }

        [TestMethod]
        [DataRow([4, 1, 4, 6])]
        [DataRow([1, 1, 1, 1])]
        [DataRow([5, -1, 5, 6])]
        [DataRow([2, -1, 2, 1])]
        [DataRow([2, 1, 2, 3])]
        [DataRow([3, -1, 3, 3])]
        [DataRow([3, 1, 3, 5])]
        public void GetIntervalNameFromExpectedValue_ValidValues_CorrectOutputs(int expectedDegree, int expectedAccidentals, int degree, int value)
        {
            (int, int) actualResult = NomenclatureHelper.GetIntervalNameFromExpectedValue(degree, value);
            Assert.AreEqual((expectedDegree, expectedAccidentals), actualResult);
        }

        [TestMethod]
        [DataRow([new[] { "G", "A", "B", "C", "D", "E", "F#" }, "G", new[] { 0, 2, 4, 5, 7, 9, 11 }])]
        [DataRow([new[] { "G", "A", "Bb", "C", "D", "E", "F" }, "G", new[] { 0, 2, 3, 5, 7, 9, 10 }])]
        [DataRow([new[] { "G", "A", "Bb", "C", "D", "Eb", "F" }, "G", new[] { 0, 2, 3, 5, 7, 8, 10 }])]
        [DataRow([new[] { "A", "B", "C#", "D", "E", "F#", "G#" }, "A", new[] { 0, 2, 4, 5, 7, 9, 11 }])]
        [DataRow([new[] { "Bb", "C", "D", "Eb", "F", "G", "A" }, "Bb", new[] { 0, 2, 4, 5, 7, 9, 11 }])]
        [DataRow([new[] { "E", "F#", "G#", "A", "B", "C#", "D#", }, "E", new[] { 0, 2, 4, 5, 7, 9, 11 }])]
        [DataRow([new[] { "B#", "C##", "D##", "E#", "F##", "G##", "A##"}, "B#", new[] { 0, 2, 4, 5, 7, 9, 11 }])]
        [DataRow([new[] { "Cb", "Dbb", "Ebb", "Fb", "Gbb", "Abb", "Bb"}, "Cb", new[] { 0, 2, 4, 5, 7, 9, 11 }])]
        public void CalculateHeptatonicNames_ValidValues_CorrectOutputs(string[] expectedNoteNames, string noteNameSymbol, int[] pattern)
        {

        }
    }
}