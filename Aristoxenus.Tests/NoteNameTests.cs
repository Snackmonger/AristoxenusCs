using Aristoxenus.Core.Domains.Theory.Atoms;

namespace Aristoxenus.Tests
{
    [TestClass]
    public sealed class NoteNameTests
    {
        private readonly NoteName NoteNameG = NoteName.FromSymbol("G");
        private readonly NoteName NoteNameA = NoteName.FromSymbol("A");
        private readonly NoteName NoteNameF = NoteName.FromSymbol("F");

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
            Assert.AreEqual(expectedNoteName, NoteNameG.NoteNameAbove(intervalName).ToSymbol());
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
            Assert.AreEqual(expectedNoteName, NoteNameA.NoteNameAbove(intervalName).ToSymbol());

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
            Assert.AreEqual(expectedNoteName, NoteNameF.NoteNameAbove(intervalName).ToSymbol());

        }

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
        public void IntervalAbove_SingleString_ReturnsCorrectIntervalAbove(string expectedInterval, string noteName)
        {
            // Assuming this should test IntervalAbove method
            Assert.AreEqual(expectedInterval, NoteNameG.IntervalAbove(noteName).Name.ToSymbol());
        }
    }
}